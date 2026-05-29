using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class TriangularGateRenderer : GateRenderer<TriangularMazeField>
    {
        public override void Draw(Graphics grap)
        {
            if (field == null || field.VertexCount == 0) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            var offsetx = transform.GetOffsetX(bounds);
            var offsety = transform.GetOffsetY(bounds);
            var flipy = field.FlipY;

            if (gate.Entrance >= 0) DrawVertexMarker(grap, gate.Entrance, Color.Green,  bounds, offsetx, offsety, flipy);
            if (gate.Exit     >= 0) DrawVertexMarker(grap, gate.Exit,     Color.Yellow, bounds, offsetx, offsety, flipy);
        }

        private void DrawVertexMarker(Graphics grap, int vertex, Color color, CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
        {
            var triangle = GetVertexTriangle(field!, vertex);

            var a = triangle.A;
            var b = triangle.B;
            var c = triangle.C;

            var centerx = (a.X + b.X + c.X) / 3;
            var centery = (a.Y + b.Y + c.Y) / 3;

            var shrink = 1.0 - 1.0 / transform.scale;
            if (shrink <= 0) return;

            var points = new PointF[3];
            var vertices = new[] { a, b, c };
            for (int i = 0; i < 3; i++)
            {
                var vx = centerx + (vertices[i].X - centerx) * shrink;
                var vy = centery + (vertices[i].Y - centery) * shrink;

                var xx = transform.TransformX(vx, bounds, offsetx);
                var yy = transform.TransformY(vy, bounds, offsety, flipy);

                points[i] = new PointF(xx, yy);
            }

            using var brush = new SolidBrush(color);
            grap.FillPolygon(brush, points);
        }

        private static readonly double Sqrt3Over2 = Math.Sqrt(3) / 2;

        private static Triangle GetVertexTriangle(TriangularMazeField field, int vertex)
        {
            var row = 0;
            while ((row + 1) * (row + 1) <= vertex)
                row++;

            var col = vertex - row * row;
            var upward = (col % 2 == 0);

            Vertex p1, p2, p3;

            if (upward)
            {
                var topX = (field.Order - row) / 2.0 + col / 2.0;
                var topY = row * Sqrt3Over2;
                var blX = (field.Order - row - 1) / 2.0 + col / 2.0;
                var blY = (row + 1) * Sqrt3Over2;
                var brX = blX + 1;
                var brY = blY;
                p1 = new Vertex(topX, topY);
                p2 = new Vertex(blX, blY);
                p3 = new Vertex(brX, brY);
            }
            else
            {
                var k = (col - 1) / 2;
                var tlX = (field.Order - row) / 2.0 + k;
                var tlY = row * Sqrt3Over2;
                var trX = tlX + 1;
                var trY = tlY;
                var bX = (field.Order - row - 1) / 2.0 + k + 1;
                var bY = (row + 1) * Sqrt3Over2;
                p1 = new Vertex(tlX, tlY);
                p2 = new Vertex(trX, trY);
                p3 = new Vertex(bX, bY);
            }

            if (field.Orientation == TriangleOrientation.Downward)
            {
                var maxY = field.Order * Sqrt3Over2;
                p1 = new Vertex(p1.X, maxY - p1.Y);
                p2 = new Vertex(p2.X, maxY - p2.Y);
                p3 = new Vertex(p3.X, maxY - p3.Y);
            }

            return new Triangle(p1, p2, p3);
        }
    }
}
