using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class CircularGateRenderer : GateRenderer<CircularMazeField>
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
            var sector = GetVertexSector(field!, vertex);

            var centerX = TransformX(0, bounds, scale, offsetX);
            var centerY = TransformY(0, bounds, scale, offsetY, flipY);

            var innerR = sector.innerRadius > 0 ? (float)(sector.innerRadius * scale) + 1 : 0;
            var outerR = (float)(sector.outerRadius * scale) - 1;

            if (outerR <= 0) return;

            var midR = (float)((sector.innerRadius + sector.outerRadius) / 2 * scale);
            var angleShrink = midR > 0 ? 1.0f / midR : 0;
            var adjustedStartAngle = (float)(sector.startAngle + angleShrink);
            var adjustedSweepAngle = (float)(sector.sweepAngle - 2 * angleShrink);

            if (adjustedSweepAngle <= 0) return;

            var startAngleDeg = 0f;
            var sweepAngleDeg = 0f;

            if (flipY)
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

            if (innerR <= 0)
            {
                grap.FillPie(brush, centerX - outerR, centerY - outerR, outerR * 2, outerR * 2, startAngleDeg, sweepAngleDeg);
            }
            else
            {
                using var path = new GraphicsPath();
                path.AddArc(centerX - outerR, centerY - outerR, outerR * 2, outerR * 2, startAngleDeg, sweepAngleDeg);
                path.AddArc(centerX - innerR, centerY - innerR, innerR * 2, innerR * 2, startAngleDeg + sweepAngleDeg, -sweepAngleDeg);
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
