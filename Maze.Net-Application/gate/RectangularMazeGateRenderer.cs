using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class RectangularMazeGateRenderer : MazeGateRenderer<RectangularMazeField>
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
            var cx = vertex % field!.Width;
            var cy = vertex / field.Width;

            var x1 = transform.TransformX(cx,     bounds, offsetx);
            var x2 = transform.TransformX(cx + 1, bounds, offsetx);
            var y1 = transform.TransformY(cy,     bounds, offsety, flipy);
            var y2 = transform.TransformY(cy + 1, bounds, offsety, flipy);

            var left   = Math.Min(x1, x2) + 1;
            var right  = Math.Max(x1, x2) - 1;
            var top    = Math.Min(y1, y2) + 1;
            var bottom = Math.Max(y1, y2) - 1;

            using var brush = new SolidBrush(color);
            grap.FillRectangle(brush, left, top, right - left, bottom - top);
        }
    }
}
