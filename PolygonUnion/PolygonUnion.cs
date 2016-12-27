using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PolygonUnion
{
    class PolygonUnion
    {
        public static List<Node<Vec2D>> AllPoints { set; get; }

        public static List<Node<Vec2D>> GetPolygonUnion(Polygon poly1, Polygon poly2)
        {
            AllPoints = new List<Node<Vec2D>>();
            List<Node<Vec2D>> UnionedPolygon = new List<Node<Vec2D>>();

            InsertIntersections(poly1, poly2);
            MarkAsEntryOrExit(poly1, poly2);
            MarkAsEntryOrExit(poly2, poly1);


            Polygon currPoly = poly1;
            Node<Vec2D> currNode = currPoly.tail;
            bool forward = true;
            
            // Traverse the polygons with the correct intersection flags to get union
            do
            {
                if (currNode.IsIntersection)
                {
                    // If the intersection is an exit, enter the next polygon
                    if (!currNode.IsEntry)
                    {
                        currNode = currNode.friend;
                        AllPoints.Remove(currNode);

                        /*
                         * If the previous point of the intersection 
                         * is in the previous poly, move forward
                         * else move backwards
                         */
                        if (PointInPolygon(currNode.prev.data, currPoly))
                        {
                            forward = true;
                        }
                        else
                        {
                            forward = false;
                        }

                        // Update current polygon
                        currPoly = currPoly == poly1 ? poly2 : poly1;
                    }
                }

                // Traverse in specified direction
                currNode = forward ? currNode.next : currNode.prev;
                UnionedPolygon.Add(currNode);
                AllPoints.Remove(currNode);

            } while (AllPoints.Count != 0);

            return UnionedPolygon;
        }
        // Insert intersections of polygon 1 and polygon 2 into both polygon linked lists
        public static void InsertIntersections(Polygon poly1, Polygon poly2)
        {

            // Pairwise edge intersection calculations
            Node<Vec2D> p1Start = poly1.tail;
            Node<Vec2D> p1Node = poly1.tail;

            do
            {
                Node<Vec2D> p2Start = poly2.tail;
                Node<Vec2D> p2Node = poly2.tail;

                do
                {
                    // First line segment points
                    Vec2D p1 = p1Node.data;
                    Vec2D p2 = p1Node.next.data;

                    // Second line segment points
                    Vec2D p3 = p2Node.data;
                    Vec2D p4 = p2Node.next.data;

                    Vec2D intersection = GetIntersection(p1, p2, p3, p4);
                    if (OnLineSegment(intersection, p1, p2) && OnLineSegment(intersection, p3, p4))
                    {
                        /*
                         * if pNode distance to next node is less than intersection's distance, 
                         * intersection is inserted before
                         */
                        Node<Vec2D> poly1Intersection = new Node<Vec2D>(intersection);
                        Node<Vec2D> poly2Intersection = new Node<Vec2D>(intersection);
                        poly1Intersection.IsIntersection = true;
                        poly2Intersection.IsIntersection = true;

                        // Friend pointer helps to traverse to the next polygon
                        poly1Intersection.friend = poly2Intersection;
                        poly2Intersection.friend = poly1Intersection;

                        /*
                         * Insert intersection in the correct order based on distance to
                         * the next point.
                         */
                        if (Vec2D.Distance(p1, p2) < Vec2D.Distance(intersection, p2))
                        {
                            poly1.InsertBefore(p1Node, poly1Intersection);
                        }
                        else
                        {
                            poly1.InsertAfter(p1Node, poly1Intersection);
                        }

                        if (Vec2D.Distance(p3, p4) < Vec2D.Distance(intersection, p4))
                        {
                            poly2.InsertBefore(p2Node, poly2Intersection);
                        }
                        else
                        {
                            poly2.InsertAfter(p2Node, poly2Intersection);
                        }
                    }

                    // Get next non intersection point
                    do
                    {
                        p2Node = p2Node.next;
                    } while (p2Node.IsIntersection);
                } while (!p2Node.Equals(p2Start));

                // Get next non intersection point
                do
                {
                    p1Node = p1Node.next;
                } while (p1Node.IsIntersection);
            } while (!p1Node.Equals(p1Start));
        }

        // Tests to see if the intersection point is within the bounds of the line segment
        static bool OnLineSegment(Vec2D point, Vec2D p1, Vec2D p2)
        {
            double x = point.X;
            double y = point.Y;

            if (double.IsInfinity(x) || double.IsInfinity(y))
            {
                return false;
            }
            bool onLine = x >= Math.Min(p1.X, p2.X) && x <= Math.Max(p1.X, p2.X)
                   && y >= Math.Min(p1.Y, p2.Y) && y <= Math.Max(p1.Y, p2.Y);
            return onLine;
        }

        // Returns infinity,  infinity if there is no intersection
        static Vec2D GetIntersection(Vec2D p1, Vec2D p2, Vec2D p3, Vec2D p4)
        {
            // Second line segment points
            double A1 = p2.Y - p1.Y;
            double B1 = p1.X - p2.X;
            double C1 = A1 * p1.X + B1 * p1.Y;

            double A2 = p4.Y - p3.Y;
            double B2 = p3.X - p4.X;
            double C2 = A2 * p3.X + B2 * p3.Y;

            double det = A1 * B2 - A2 * B1;


            if (Vec2D.NearlyEqual(det, 0.0f))
            {
                // parallel lines
                return new Vec2D(double.PositiveInfinity, double.PositiveInfinity);
            }
            else
            {
                // point of intersection
                double x = (B2 * C1 - B1 * C2) / det;
                double y = (A1 * C2 - A2 * C1) / det;

                return new Vec2D(x, y);
            }
        }

        // Tests to see if a point is in polygon
        // Used to mark intersections as entry or exit into polygons
        public static bool PointInPolygon(Vec2D point, Polygon poly)
        {
            Node<Vec2D> startNode = poly.tail;
            Node<Vec2D> currNode = startNode;

            // Count intersection
            int numIntersections = 0;

            // Create a point outside of the polygon
            double epsilon = (poly.GetXValues().Max() - poly.GetXValues().Min()) / 100;
            Vec2D p1 = new Vec2D(poly.GetXValues().Min() - epsilon, point.Y);
            Vec2D p2 = point;
            do
            {
                Vec2D p3 = currNode.data;
                Vec2D p4 = currNode.next.data;

                Vec2D intersection = GetIntersection(p1, p2, p3, p4);
                if (OnLineSegment(intersection, p1, p2) && OnLineSegment(intersection, p3, p4))
                {
                    numIntersections++;
                }

                currNode = currNode.next;

            } while (!currNode.Equals(startNode));

            return numIntersections % 2 == 1;
        }

        // Marks intersections of p1 as entry or exit into p2
        public static void MarkAsEntryOrExit(Polygon p1, Polygon p2)
        {
            /*
             * If first point of p1 is not inside p2, then the first
             * intersection is entry
             */

            bool isEntry;
            if (!PointInPolygon(p1.tail.data, p2))
            {
                isEntry = true;
            }
            else
            {
                isEntry = false;
            }

            Node<Vec2D> startNode = p1.tail;
            Node<Vec2D> currNode = p1.tail;

            do
            {
                if (currNode.IsIntersection)
                {
                    currNode.IsEntry = isEntry;
                    isEntry = !isEntry;
                }
                currNode = currNode.next;
            } while (!currNode.Equals(startNode));
        }
    }
}
