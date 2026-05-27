using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class CircularHexagonGateRenderer : GateRenderer<CircularHexagonMazeField>
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
            var shape = GetVertexSectorShape(field!, vertex);

            var centerX = TransformX(0, bounds, scale, offsetX);
            var centerY = TransformY(0, bounds, scale, offsetY, flipY);

            var arcR = (float)(shape.arcRadius * scale) - 1;
            if (arcR <= 0) return;

            var angleShrink = arcR > 0 ? 1.0f / arcR : 0;
            var adjustedArcStart = (float)(shape.arcStartAngle + angleShrink);
            var adjustedArcSweep = (float)(shape.arcSweepAngle - 2 * angleShrink);
            if (adjustedArcSweep <= 0) return;

            var arcStartDeg = 0.0f;
            var arcSweepDeg = 0.0f;

            if (flipY)
            {
                arcStartDeg = (float)(-adjustedArcStart * 180.0 / Math.PI);
                arcSweepDeg = (float)(-adjustedArcSweep * 180.0 / Math.PI);
            }
            else
            {
                arcStartDeg = (float)(adjustedArcStart * 180.0 / Math.PI);
                arcSweepDeg = (float)(adjustedArcSweep * 180.0 / Math.PI);
            }

            using var brush = new SolidBrush(color);
            using var path = new GraphicsPath();

            if (shape.upward)
            {
                var innerR = shape.innerRadius > 0 ? (float)(shape.innerRadius * scale) + 1 : 0;

                if (innerR <= 0)
                {
                    grap.FillPie(brush, centerX - arcR, centerY - arcR, arcR * 2, arcR * 2, arcStartDeg, arcSweepDeg);
                }
                else
                {
                    var innerAngle = (float)shape.innerAngle;
                    var innerX = centerX + innerR * (float)Math.Cos(flipY ? -innerAngle : innerAngle);
                    var innerY = centerY + innerR * (float)Math.Sin(flipY ? -innerAngle : innerAngle);

                    path.AddArc(centerX - arcR, centerY - arcR, arcR * 2, arcR * 2, arcStartDeg, arcSweepDeg);
                    path.AddLine(innerX, innerY, innerX, innerY);
                    path.CloseFigure();
                    grap.FillPath(brush, path);
                }
            }
            else
            {
                var outerR = (float)(shape.outerRadius * scale) - 1;
                if (outerR <= 0) return;

                var outerAngle = (float)shape.outerAngle;
                var outerX = centerX + outerR * (float)Math.Cos(flipY ? -outerAngle : outerAngle);
                var outerY = centerY + outerR * (float)Math.Sin(flipY ? -outerAngle : outerAngle);

                path.AddArc(centerX - arcR, centerY - arcR, arcR * 2, arcR * 2, arcStartDeg, arcSweepDeg);
                path.AddLine(outerX, outerY, outerX, outerY);
                path.CloseFigure();
                grap.FillPath(brush, path);
            }
        }

        private static CurvedTriangle GetVertexSectorShape(CircularHexagonMazeField field, int vertex)
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

            var sectorStart = (sector - 2) * Math.PI / 3;

            if (updown == 0)
            {
                var innerAngle = row > 0 ? sectorStart + column * Math.PI / 3 / row : 0;
                var arcStartAngle = sectorStart + column * Math.PI / 3 / (row + 1);
                var arcSweepAngle = Math.PI / 3 / (row + 1);
                return new CurvedTriangle(true, row, innerAngle, row + 1, arcStartAngle, arcSweepAngle, 0, 0);
            }
            else
            {
                var arcStartAngle = sectorStart + column * Math.PI / 3 / (row + 1);
                var arcSweepAngle = Math.PI / 3 / (row + 1);
                var outerAngle = sectorStart + (column + 1) * Math.PI / 3 / (row + 2);
                return new CurvedTriangle(false, 0, 0, row + 1, arcStartAngle, arcSweepAngle, row + 2, outerAngle);
            }
        }
    }
}
