using System;

namespace PolygonUnion
{
    public class Vec2D
    {
        public Vec2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        // Coordinates of this point
        public double X { get; set; }
        public double Y { get; set; }

        public static Vec2D Subtract(Vec2D p1, Vec2D p2)
        {
            return new Vec2D(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Vec2D Add(Vec2D p1, Vec2D p2)
        {
            return new Vec2D(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static double Distance(Vec2D p1, Vec2D p2)
        {
            return (double)Math.Sqrt(Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2));
        }

        public static double CrossMultiply(Vec2D p1, Vec2D p2)
        {
            return p1.X * p2.Y - p1.Y * p2.X;
        }

        public override bool Equals(object o)
        {
            Vec2D data = (Vec2D) o;
            return NearlyEqual(data.X, this.X) && NearlyEqual(data.Y, this.Y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool NearlyEqual(double f1, double f2)
        {
            // Equal if they are within 0.00001 of each other
            return Math.Abs(f1 - f2) < 0.00001;
        }
    }
}
