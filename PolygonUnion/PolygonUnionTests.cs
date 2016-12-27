namespace PolygonUnion.Tests
{
    using NUnit.Framework;
    class PolygonUnionTests
    {
        // Simple test for two unit squares
        [Test]
        public void TestInsertIntersections()
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

            // Insert intersections

            PolygonUnion.InsertIntersections(poly1, poly2);

            // Traverse values
            Node<Vec2D> startNode = poly2.tail;
            Node<Vec2D> currNode = poly2.tail;

            do
            {
                currNode = currNode.next;

                if (currNode.data.Equals(new Vec2D(1, 0, false)))
                {
                    Assert.True(currNode.data.IsIntersection);
                }
                if (currNode.data.Equals(new Vec2D(0, -1, false)))
                {
                    Assert.True(currNode.data.IsIntersection);
                }
            } while (!currNode.Equals(startNode));

        }
    }
}
