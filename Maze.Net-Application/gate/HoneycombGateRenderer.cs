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

            var offsetx = transform.GetOffsetX(bounds);
            var offsety = transform.GetOffsetY(bounds);
            var flipy = field.FlipY;

            if (gate.Entrance >= 0) DrawVertexMarker(grap, gate.Entrance, Color.Green,  bounds, offsetx, offsety, flipy);
            if (gate.Exit     >= 0) DrawVertexMarker(grap, gate.Exit,     Color.Yellow, bounds, offsetx, offsety, flipy);
        }

        private void DrawVertexMarker(Graphics grap, int vertex, Color color, CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
        {
            var cv = GetVertexHexagon(field!, vertex);

            var shrinkRadius = 1.0 - 1.0 / transform.scale;
            if (shrinkRadius <= 0) return;

            var points = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                var angle = (i - 2.5) * Math.PI / 3;
                var vx = cv.X + shrinkRadius * Math.Cos(angle);
                var vy = cv.Y + shrinkRadius * Math.Sin(angle);

                var xx = transform.TransformX(vx, bounds, offsetx);
                var yy = transform.TransformY(vy, bounds, offsety, flipy);

                points[i] = new PointF(xx, yy);
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
