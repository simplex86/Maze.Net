using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class CircularHexagonMazeGateRenderer : MazeGateRenderer<CircularHexagonMazeField>
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
            var shape = GetVertexSectorShape(field!, vertex);

            var centerx = transform.TransformX(0, bounds, offsetx);
            var centery = transform.TransformY(0, bounds, offsety, flipy);

            var arcr = (float)(shape.ArcRadius * transform.scale) - 1;
            if (arcr <= 0) return;

            var angleShrink = arcr > 0 ? 1.0f / arcr : 0;
            var adjustedArcStart = (float)(shape.ArcStartAngle + angleShrink);
            var adjustedArcSweep = (float)(shape.ArcSweepAngle - 2 * angleShrink);
            if (adjustedArcSweep <= 0) return;

            var arcStartDeg = 0.0f;
            var arcSweepDeg = 0.0f;

            if (flipy)
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

            if (shape.Upward)
            {
                var innerr = shape.InnerRadius > 0 ? (float)(shape.InnerRadius * transform.scale) + 1 : 0;

                if (innerr <= 0)
                {
                    grap.FillPie(brush, centerx - arcr, centery - arcr, arcr * 2, arcr * 2, arcStartDeg, arcSweepDeg);
                }
                else
                {
                    var angle = (float)shape.InnerAngle;
                    var innerx = centerx + innerr * (float)Math.Cos(flipy ? -angle : angle);
                    var innery = centery + innerr * (float)Math.Sin(flipy ? -angle : angle);

                    path.AddArc(centerx - arcr, centery - arcr, arcr * 2, arcr * 2, arcStartDeg, arcSweepDeg);
                    path.AddLine(innerx, innery, innerx, innery);
                    path.CloseFigure();
                    grap.FillPath(brush, path);
                }
            }
            else
            {
                var outerr = (float)(shape.OuterRadius * transform.scale) - 1;
                if (outerr <= 0) return;

                var angle = (float)shape.OuterAngle;
                var outerx = centerx + outerr * (float)Math.Cos(flipy ? -angle : angle);
                var outery = centery + outerr * (float)Math.Sin(flipy ? -angle : angle);

                path.AddArc(centerx - arcr, centery - arcr, arcr * 2, arcr * 2, arcStartDeg, arcSweepDeg);
                path.AddLine(outerx, outery, outerx, outery);
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
