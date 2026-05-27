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

            if (gate.entrance >= 0) DrawVertexMarker(grap, gate.entrance, Color.Green, bounds, scale, offsetX, offsetY, flipY);
            if (gate.exit     >= 0) DrawVertexMarker(grap, gate.exit,     Color.Gold,  bounds, scale, offsetX, offsetY, flipY);
        }

        private void DrawVertexMarker(Graphics grap, int vertex, Color color, CoordinateBounds bounds, float scale, float offsetX, float offsetY, bool flipY)
        {
            var triangle = field!.GetVertexTriangle(vertex);

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
    }
}
