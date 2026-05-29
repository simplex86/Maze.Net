using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class CircularMazeGateRenderer : MazeGateRenderer<CircularMazeField>
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
            var sector = GetVertexSector(field!, vertex);

            var centerx = transform.TransformX(0, bounds, offsetx);
            var centery = transform.TransformY(0, bounds, offsety, flipy);

            var innerr = sector.InnerRadius > 0 ? (float)(sector.InnerRadius * transform.scale) + 1 : 0;
            var outerr = (float)(sector.OuterRadius * transform.scale) - 1;

            if (outerr <= 0) return;

            var midr = (float)((sector.InnerRadius + sector.OuterRadius) / 2 * transform.scale);
            var angleShrink = midr > 0 ? 1.0f / midr : 0;
            var adjustedStartAngle = (float)(sector.StartAngle + angleShrink);
            var adjustedSweepAngle = (float)(sector.SweepAngle - 2 * angleShrink);

            if (adjustedSweepAngle <= 0) return;

            var startAngleDeg = 0f;
            var sweepAngleDeg = 0f;

            if (flipy)
            {
                startAngleDeg = (float)(-adjustedStartAngle * 180.0 / Math.PI);
                sweepAngleDeg = (float)(-adjustedSweepAngle * 180.0 / Math.PI);
            }
            else
            {
                startAngleDeg = (float)(adjustedStartAngle * 180.0 / Math.PI);
                sweepAngleDeg = (float)(adjustedSweepAngle * 180.0 / Math.PI);
            }

            using var brush = new SolidBrush(color);

            if (innerr <= 0)
            {
                grap.FillPie(brush, centerx - outerr, centery - outerr, outerr * 2, outerr * 2, startAngleDeg, sweepAngleDeg);
            }
            else
            {
                using var path = new GraphicsPath();
                path.AddArc(centerx - outerr, centery - outerr, outerr * 2, outerr * 2, startAngleDeg, sweepAngleDeg);
                path.AddArc(centerx - innerr, centery - innerr, innerr * 2, innerr * 2, startAngleDeg + sweepAngleDeg, -sweepAngleDeg);
                path.CloseFigure();
                grap.FillPath(brush, path);
            }
        }

        private static AnnularSector GetVertexSector(CircularMazeField field, int vertex)
        {
            var remaining = vertex;
            for (var r = 0; r < field.Rings; r++)
            {
                if (remaining < field.SectorsPerRing[r])
                {
                    var n = field.SectorsPerRing[r];
                    var angleStep = 2 * Math.PI / n;
                    var startAngle = remaining * angleStep - Math.PI / 2;
                    return new AnnularSector(r, r + 1, startAngle, angleStep);
                }
                remaining -= field.SectorsPerRing[r];
            }
            return new AnnularSector(0, 0, 0, 0);
        }
    }
}
