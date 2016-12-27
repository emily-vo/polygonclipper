using System;
using System.Collections.Generic;

namespace PolygonUnion
{
    using NUnit.Framework;
    class PolygonUnionTests
    {
        // Simple test for two unit squares
        // Tests number of intersections added is corrected
        [Test]
        public void TestInsertIntersectionCount()
        {
            // Create first polygon
            Polygon poly1  = new Polygon();
            Vec2D[] points1 = new Vec2D[] {
                new Vec2D(1.0f, 1.0f),
                new Vec2D(1.0f, -1.0f),
                new Vec2D(-1.0f, -1.0f),
                new Vec2D(-1.0f, 1.0f) };
            
            poly1.AddPoints(points1);

            // Create second polygon
            Polygon poly2 = new Polygon();
            Vec2D[] points2 = new Vec2D[]
            {
                new Vec2D(0.0f, 0.0f), 
                new Vec2D(2.0f, 0.0f),
                new Vec2D(2.0f, -2.0f),
                new Vec2D(0.0f, -2.0f)
            };

            poly2.AddPoints(points2);

            Assert.AreEqual(4, poly1.Count());
            Assert.AreEqual(4, poly2.Count());

            // Insert intersections
            PolygonUnion.InsertIntersections(poly1, poly2);

            // Assert correct number of points inserted
            Assert.AreEqual(6, poly1.Count());
            Assert.AreEqual(6, poly2.Count());
        }

        // Simple test for two unit squares
        // Tests isIntersection property
        [Test]
        public void TestIssIntersection()
        {
            // Create first polygon
            Polygon poly1 = new Polygon();
            Vec2D[] points1 = new Vec2D[] {
                new Vec2D(1.0f, 1.0f),
                new Vec2D(1.0f, -1.0f),
                new Vec2D(-1.0f, -1.0f),
                new Vec2D(-1.0f, 1.0f) };

            poly1.AddPoints(points1);

            // Create second polygon
            Polygon poly2 = new Polygon();
            Vec2D[] points2 = new Vec2D[]
            {
                new Vec2D(0.0f, 0.0f),
                new Vec2D(2.0f, 0.0f),
                new Vec2D(2.0f, -2.0f),
                new Vec2D(0.0f, -2.0f)
            };

            poly2.AddPoints(points2);

            // Insert intersections
            PolygonUnion.InsertIntersections(poly1, poly2);

            // Traverse values
            Node<Vec2D> startNode1 = poly1.tail;
            Node<Vec2D> currNode1 = poly1.tail;
            Node<Vec2D> startNode2 = poly2.tail;
            Node<Vec2D> currNode2 = poly2.tail;

            do
            {
                // Assert that intersections are determined correctly in first poly
                if (currNode1.data.Equals(new Vec2D(1, 0)))
                {
                    Assert.True(currNode1.IsIntersection);
                }
                else if (currNode1.data.Equals(new Vec2D(0, -1)))
                {
                    Assert.True(currNode1.IsIntersection);
                }
                else
                {
                    Assert.False(currNode1.IsIntersection);
                }

                // Assert that intersections are determined correctly in second poly
                if (currNode2.data.Equals(new Vec2D(1, 0)))
                {
                    Assert.True(currNode2.IsIntersection);
                }
                else if (currNode2.data.Equals(new Vec2D(0, -1)))
                {
                    Assert.True(currNode2.IsIntersection);
                }
                else
                {
                    Assert.False(currNode2.IsIntersection);
                }

                currNode1 = currNode1.next;
                currNode2 = currNode2.next;
            } while (!currNode2.Equals(startNode2));
        }

        /*
         * Tests if the correct number of points are added.
         * This test case is an intersection between a triangle
         * and a square.
         */
        [Test]
        public void TestInsertIntersectionsCount()
        {
            // Create first polygon
            Polygon poly1 = new Polygon();
            Vec2D[] points1 = new Vec2D[] {
                new Vec2D(1.0f, 1.0f),
                new Vec2D(1.0f, -1.0f),
                new Vec2D(-1.0f, -1.0f),
                new Vec2D(-1.0f, 1.0f) };

            poly1.AddPoints(points1);

            // Create second polygon
            Polygon poly2 = new Polygon();
            Vec2D[] points2 = new Vec2D[]
            {
                new Vec2D(0.0f, 0.0f),
                new Vec2D(2.0f, 0.5f),
                new Vec2D(2.0f, -0.5f),
            };

            poly2.AddPoints(points2);

            Assert.AreEqual(4, poly1.Count());
            Assert.AreEqual(3, poly2.Count());

            // Insert intersections
            PolygonUnion.InsertIntersections(poly1, poly2);

            // Assert correct number of points
            Assert.AreEqual(6, poly1.Count());
            Assert.AreEqual(5, poly2.Count());
        }

        /*
         * The algorithm must determine the correct order to 
         * insert the intersections into along the edge.
         * 
         * This test case is an intersection between a triangle
         * and a square.
         */
        [Test]
        public void TestInsertIntersectionsInOrder()
        {
            // Create first polygon
            Polygon poly1 = new Polygon();
            Vec2D[] points1 = new Vec2D[] {
                new Vec2D(1.0f, 1.0f),
                new Vec2D(1.0f, -1.0f),
                new Vec2D(-1.0f, -1.0f),
                new Vec2D(-1.0f, 1.0f) };

            poly1.AddPoints(points1);

            // Create second polygon
            Polygon poly2 = new Polygon();
            Vec2D[] points2 = new Vec2D[]
            {
                new Vec2D(0.0f, 0.0f),
                new Vec2D(2.0f, 0.5f),
                new Vec2D(2.0f, -0.5f),
            };

            poly2.AddPoints(points2);


            // Insert intersections
            PolygonUnion.InsertIntersections(poly1, poly2);

            // Assert correct number of points
            Assert.AreEqual(6, poly1.Count());
            Assert.AreEqual(5, poly2.Count());

            // Assert global list of intersections contains the correct number of points

            // Traverse values
            // Intersections are along the same edge in poly1
            Node<Vec2D> startNode1 = poly1.tail;
            Node<Vec2D> currNode1 = poly1.tail;
            bool metFirstInter = false;
            do
            {
                if (!metFirstInter && currNode1.IsIntersection)
                {
                    metFirstInter = true;
                    Assert.True(currNode1.IsIntersection);
                }
                currNode1 = currNode1.next;
            } while (!currNode1.Equals(startNode1));
        }

        [Test]
        public void TestPointInPolygon()
        {
            // Create first polygon
            Polygon poly1 = new Polygon();
            Vec2D[] points1 = new Vec2D[] {
                new Vec2D(1.0f, 1.0f),
                new Vec2D(1.0f, -1.0f),
                new Vec2D(-1.0f, -1.0f),
                new Vec2D(-1.0f, 1.0f) };

            poly1.AddPoints(points1);

            Assert.True(PolygonUnion.PointInPolygon(new Vec2D(0, 0), poly1));
            Assert.False(PolygonUnion.PointInPolygon(new Vec2D(2, 2), poly1));
        }

        [Test]
        public void TestEntryExitMarks()
        {
            // Create first polygon
            Polygon poly1 = new Polygon();
            Vec2D[] points1 = new Vec2D[] {
                new Vec2D(1.0f, 1.0f),
                new Vec2D(1.0f, -1.0f),
                new Vec2D(-1.0f, -1.0f),
                new Vec2D(-1.0f, 1.0f) };

            poly1.AddPoints(points1);

            // Create second polygon
            Polygon poly2 = new Polygon();
            Vec2D[] points2 = new Vec2D[]
            {
                new Vec2D(0.0f, 0.0f),
                new Vec2D(2.0f, 0.5f),
                new Vec2D(2.0f, -0.5f),
            };

            poly2.AddPoints(points2);
            PolygonUnion.InsertIntersections(poly1, poly2);
            PolygonUnion.MarkAsEntryOrExit(poly1, poly2);

            Node<Vec2D> currNode = poly1.tail;
            do
            {
                if (currNode.data.Equals(new Vec2D(1, 0.25)))
                {
                    Assert.True(currNode.IsIntersection);
                    Assert.True(currNode.IsEntry);
                }
                if (currNode.data.Equals(new Vec2D(1, -0.25)))
                {
                    Assert.True(currNode.IsIntersection);
                    Assert.False(currNode.IsEntry);
                }
                currNode = currNode.next;
            } while (!currNode.Equals(poly1.tail));
        }
    }
}
