using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class HoneycombGateRenderer : GateRenderer<HoneycombMazeField>
    {
        public override void Draw(Graphics grap)
        {
            if (field == null || field.VertexCount == 0) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            var scale = thickness;
            var offsetX = (float)((width - bounds.Width * scale) / 2) + offsetx;
            var offsetY = (float)((height - bounds.Height * scale) / 2) + offsety;
            var flipY = field.FlipY;

            if (gate.Entrance >= 0) DrawVertexMarker(grap, gate.Entrance, Color.Green,  bounds, scale, offsetX, offsetY, flipY);
            if (gate.Exit     >= 0) DrawVertexMarker(grap, gate.Exit,     Color.Yellow, bounds, scale, offsetX, offsetY, flipY);
        }

        private void DrawVertexMarker(Graphics grap, int vertex, Color color, CoordinateBounds bounds, float scale, float offsetX, float offsetY, bool flipY)
        {
            var cv = GetVertexHexagon(field!, vertex);

            var shrinkRadius = 1.0 - 1.0 / scale;
            if (shrinkRadius <= 0) return;

            var points = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                var angle = (i - 2.5) * Math.PI / 3;
                var vx = cv.X + shrinkRadius * Math.Cos(angle);
                var vy = cv.Y + shrinkRadius * Math.Sin(angle);
                points[i] = new PointF(TransformX(vx, bounds, scale, offsetX), TransformY(vy, bounds, scale, offsetY, flipY));
            }

            using var brush = new SolidBrush(color);
            grap.FillPolygon(brush, points);
        }

        private static Vertex GetVertexHexagon(HoneycombMazeField field, int vertex)
        {
            var totalUp = field.Length * (3 * field.Length - 1) / 2;
            if (vertex < totalUp)
            {
                for (int u = -field.Length + 1; u <= 0; u++)
                {
                    var (vmin, vmax) = VExtent(field.Length, u);
                    var rowSize = vmax - vmin + 1;
                    if (vertex < rowSize)
                    {
                        int v = vmin + vertex;
                        return ComputeCenter(u, v);
                    }
                    vertex -= rowSize;
                }
            }
            else
            {
                vertex -= totalUp;
                for (int u = 1; u < field.Length; u++)
                {
                    var (vmin, vmax) = VExtent(field.Length, u);
                    int rowSize = vmax - vmin + 1;
                    if (vertex < rowSize)
                    {
                        int v = vmin + vertex;
                        return ComputeCenter(u, v);
                    }
                    vertex -= rowSize;
                }
            }
            return new Vertex(0, 0);
        }

        private static (int min, int max) VExtent(int length, int u)
        {
            return (u < 0) ? (-length - u + 1, length - 1)
                           : (-length + 1,     length - 1 - u);
        }

        private static Vertex ComputeCenter(int u, int v)
        {
            var dxu = Math.Sqrt(3) / 2;
            var dyu = 1.5;
            var dxv = Math.Sqrt(3);
            var dyv = 0;

            return new Vertex(dxu * u + dxv * v, dyu * u + dyv * v);
        }
    }
}
