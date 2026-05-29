using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class HexagonalGateRenderer : GateRenderer<HexagonalMazeField>
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
            var t = GetVertexTriangle(field!, vertex);
            var a = t.A;
            var b = t.B;
            var c = t.C;

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

            return new Triangle(new Vertex(pa.X * cosTheta - pa.Y * sinTheta, pa.X * sinTheta + pa.Y * cosTheta),
                                new Vertex(pb.X * cosTheta - pb.Y * sinTheta, pb.X * sinTheta + pb.Y * cosTheta),
                                new Vertex(pc.X * cosTheta - pc.Y * sinTheta, pc.X * sinTheta + pc.Y * cosTheta));
        }
    }
}
