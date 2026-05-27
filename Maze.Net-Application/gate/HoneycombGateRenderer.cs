using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class HoneycombGateRenderer : GateRenderer<HoneycombMazeField>
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
            var cv = field!.GetVertexHexagon(vertex);

            var shrinkRadius = 1.0 - 1.0 / scale;
            if (shrinkRadius <= 0) return;

            var points = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                var angle = (i - 2.5) * Math.PI / 3;
                var vx = cv.x + shrinkRadius * Math.Cos(angle);
                var vy = cv.y + shrinkRadius * Math.Sin(angle);
                points[i] = new PointF(TransformX(vx, bounds, scale, offsetX), TransformY(vy, bounds, scale, offsetY, flipY));
            }

            using var brush = new SolidBrush(color);
            grap.FillPolygon(brush, points);
        }
    }
}
