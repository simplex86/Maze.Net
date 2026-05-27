using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class RectangularGateRenderer : GateRenderer<RectangularMazeField>
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
            var cx = vertex % field!.Width;
            var cy = vertex / field.Width;

            var x1 = TransformX(cx, bounds, scale, offsetX);
            var x2 = TransformX(cx + 1, bounds, scale, offsetX);
            var y1 = TransformY(cy, bounds, scale, offsetY, flipY);
            var y2 = TransformY(cy + 1, bounds, scale, offsetY, flipY);

            var left = Math.Min(x1, x2) + 1;
            var right = Math.Max(x1, x2) - 1;
            var top = Math.Min(y1, y2) + 1;
            var bottom = Math.Max(y1, y2) - 1;

            using var brush = new SolidBrush(color);
            grap.FillRectangle(brush, left, top, right - left, bottom - top);
        }
    }
}
