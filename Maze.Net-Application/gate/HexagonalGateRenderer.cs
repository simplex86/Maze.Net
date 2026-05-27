using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class HexagonalGateRenderer : GateRenderer<HexagonalMazeField>
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
            var t = GetVertexTriangle(field!, vertex);
            var a = t.a;
            var b = t.b;
            var c = t.c;

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

        private static Triangle GetVertexTriangle(HexagonalMazeField field, int vertex)
        {
            var sectorSize = field.Size * field.Size;
            var sector = vertex / sectorSize;
            var remaining = vertex % sectorSize;
            var updownSize = field.Size * (field.Size + 1) / 2;
            var updown = remaining < updownSize ? 0 : 1;
            var idx = updown == 0 ? remaining : remaining - updownSize;

            var row = 0;
            while ((row + 1) * (row + 2) / 2 <= idx)
                row++;
            var column = idx - row * (row + 1) / 2;

            var x1 = 0;
            var y1 = 0;
            var x2 = -field.Size / 2.0;
            var y2 = Math.Sqrt(3) * x2;
            var x3 = -x2;
            var y3 = y2;
            var dx12 = (x2 - x1) / field.Size;
            var dy12 = (y2 - y1) / field.Size;
            var dx23 = (x3 - x2) / field.Size;
            var dy23 = (y3 - y2) / field.Size;

            Vertex pa, pb, pc;

            if (updown == 0)
            {
                var topX = dx12 * row + dx23 * column;
                var topY = dy12 * row + dy23 * column;
                var blX = dx12 * (row + 1) + dx23 * column;
                var blY = dy12 * (row + 1) + dy23 * column;
                var brX = dx12 * (row + 1) + dx23 * (column + 1);
                var brY = dy12 * (row + 1) + dy23 * (column + 1);
                pa = new Vertex(topX, topY);
                pb = new Vertex(blX, blY);
                pc = new Vertex(brX, brY);
            }
            else
            {
                var tlX = dx12 * (row + 1) + dx23 * column;
                var tlY = dy12 * (row + 1) + dy23 * column;
                var trX = dx12 * (row + 1) + dx23 * (column + 1);
                var trY = dy12 * (row + 1) + dy23 * (column + 1);
                var bX = dx12 * (row + 2) + dx23 * (column + 1);
                var bY = dy12 * (row + 2) + dy23 * (column + 1);
                pa = new Vertex(tlX, tlY);
                pb = new Vertex(trX, trY);
                pc = new Vertex(bX, bY);
            }

            var theta = sector * Math.PI / 3;
            var cosTheta = Math.Cos(theta);
            var sinTheta = Math.Sin(theta);

            return new Triangle(new Vertex(pa.x * cosTheta - pa.y * sinTheta, pa.x * sinTheta + pa.y * cosTheta),
                                new Vertex(pb.x * cosTheta - pb.y * sinTheta, pb.x * sinTheta + pb.y * cosTheta),
                                new Vertex(pc.x * cosTheta - pc.y * sinTheta, pc.x * sinTheta + pc.y * cosTheta));
        }
    }
}
