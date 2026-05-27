using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class TriangularGateRenderer : GateRenderer<TriangularMazeField>
    {
        public override void Draw(Graphics grap)
        {
            if (field == null || field.Count == 0) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            var scale = thickness;
            var offsetX = (float)((width - bounds.Width * scale) / 2) + offsetx;
            var offsetY = (float)((height - bounds.Height * scale) / 2) + offsety;
            var flipY = field.FlipY;

            if (gate.entrance >= 0) DrawVertexMarker(grap, gate.entrance, Color.Green,  bounds, scale, offsetX, offsetY, flipY);
            if (gate.exit     >= 0) DrawVertexMarker(grap, gate.exit,     Color.Yellow, bounds, scale, offsetX, offsetY, flipY);
        }

        private void DrawVertexMarker(Graphics grap, int vertex, Color color, CoordinateBounds bounds, float scale, float offsetX, float offsetY, bool flipY)
        {
            var triangle = GetVertexTriangle(field!, vertex);

            var a = triangle.a;
            var b = triangle.b;
            var c = triangle.c;

            var centerX = (a.x + b.x + c.x) / 3;
            var centerY = (a.y + b.y + c.y) / 3;

            var shrink = 1.0 - 1.0 / scale;
            if (shrink <= 0) return;

            var points = new PointF[3];
            var vertices = new[] { a, b, c };
            for (int i = 0; i < 3; i++)
            {
                var vx = centerX + (vertices[i].x - centerX) * shrink;
                var vy = centerY + (vertices[i].y - centerY) * shrink;
                points[i] = new PointF(TransformX(vx, bounds, scale, offsetX), TransformY(vy, bounds, scale, offsetY, flipY));
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
                p1 = new Vertex(p1.x, maxY - p1.y);
                p2 = new Vertex(p2.x, maxY - p2.y);
                p3 = new Vertex(p3.x, maxY - p3.y);
            }

            return new Triangle(p1, p2, p3);
        }
    }
}
