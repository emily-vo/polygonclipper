using System.Collections;
using System.Collections.Generic;

namespace PolygonUnion
{

    public class Polygon : CircularDoublyLinkedList<Vec2D>
    {
        // Lists used to get max and min of x and y values to create points outside
        private List<double> xPoints;
        private List<double> yPoints;
        public Polygon() : base()
        {
            xPoints = new List<double>();
            yPoints = new List<double>();
        }

        public void AddPoints(Vec2D[] points)
        {
            foreach (Vec2D point in points)
            {
                this.InsertEnd(new Node<Vec2D>(point));
                xPoints.Add(point.X);
                yPoints.Add(point.Y);
            }
        }

        public void AddPoint(double x, double y)
        {
            this.InsertEnd(new Node<Vec2D>(new Vec2D(x, y)));
            xPoints.Add(x);
            yPoints.Add(y);
        }

        public void AddPointAfter(Node<Vec2D> node, double  x, double y)
        {
            this.InsertAfter(node, new Node<Vec2D>(new Vec2D(x, y)));
        }

        public double[] GetXValues()
        {
            return xPoints.ToArray();
        }

        public double[] GetYValues()
        {
            return yPoints.ToArray();
        }
        // Create functional type on iterative 
    }
}
