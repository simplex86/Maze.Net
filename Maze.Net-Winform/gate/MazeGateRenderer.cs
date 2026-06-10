using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class MazeGateRenderer
    {
        private MazeField? field;
        private MazeGate gate;
        private CoordinateTransform transform = new CoordinateTransform();

        public MazeGateRenderer SetSize(int width, int height)
        {
            transform.width = width;
            transform.height = height;
            return this;
        }

        public MazeGateRenderer SetThickness(int thickness)
        {
            transform.scale = thickness;
            return this;
        }

        public MazeGateRenderer SetOffset(int dx, int dy)
        {
            transform.dx = dx;
            transform.dy = dy;
            return this;
        }

        public MazeGateRenderer SetField(MazeField? field)
        {
            this.field = field;
            return this;
        }

        public MazeGateRenderer SetGate(MazeGate gate)
        {
            this.gate = gate;
            return this;
        }

        public void Draw(Graphics grap)
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
            var shape = field!.GetCellShape(vertex);

            switch (shape.Type)
            {
                case CellShapeType.Polygon:
                    DrawPolygonMarker(grap, shape.Vertices, color, bounds, offsetx, offsety, flipy);
                    break;
                case CellShapeType.AnnularSector:
                    DrawAnnularSectorMarker(grap, shape.Sector, color, bounds, offsetx, offsety, flipy);
                    break;
                case CellShapeType.CurvedTriangle:
                    DrawCurvedTriangleMarker(grap, shape.CurvedTriangle, color, bounds, offsetx, offsety, flipy);
                    break;
            }
        }

        private void DrawPolygonMarker(Graphics grap, Vertex[] vertices, Color color, CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
        {
            if (vertices == null || vertices.Length == 0) return;

            // 计算中心点
            double cx = 0, cy = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                cx += vertices[i].X;
                cy += vertices[i].Y;
            }
            cx /= vertices.Length;
            cy /= vertices.Length;

            // 收缩多边形
            var shrink = 1.0 - 1.0 / transform.scale;
            if (shrink <= 0) return;

            var points = new PointF[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vx = cx + (vertices[i].X - cx) * shrink;
                var vy = cy + (vertices[i].Y - cy) * shrink;

                var xx = transform.TransformX(vx, bounds, offsetx);
                var yy = transform.TransformY(vy, bounds, offsety, flipy);

                points[i] = new PointF(xx, yy);
            }

            using var brush = new SolidBrush(color);
            grap.FillPolygon(brush, points);
        }

        private void DrawAnnularSectorMarker(Graphics grap, AnnularSector sector, Color color, CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
        {
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

        private void DrawCurvedTriangleMarker(Graphics grap, CurvedTriangle ct, Color color, CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
        {
            var centerx = transform.TransformX(0, bounds, offsetx);
            var centery = transform.TransformY(0, bounds, offsety, flipy);

            var arcr = (float)(ct.ArcRadius * transform.scale) - 1;
            if (arcr <= 0) return;

            var angleShrink = arcr > 0 ? 1.0f / arcr : 0;
            var adjustedArcStart = (float)(ct.ArcStartAngle + angleShrink);
            var adjustedArcSweep = (float)(ct.ArcSweepAngle - 2 * angleShrink);
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

            if (ct.Upward)
            {
                var innerr = ct.InnerRadius > 0 ? (float)(ct.InnerRadius * transform.scale) + 1 : 0;

                if (innerr <= 0)
                {
                    grap.FillPie(brush, centerx - arcr, centery - arcr, arcr * 2, arcr * 2, arcStartDeg, arcSweepDeg);
                }
                else
                {
                    var angle = (float)ct.InnerAngle;
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
                var outerr = (float)(ct.OuterRadius * transform.scale) - 1;
                if (outerr <= 0) return;

                var angle = (float)ct.OuterAngle;
                var outerx = centerx + outerr * (float)Math.Cos(flipy ? -angle : angle);
                var outery = centery + outerr * (float)Math.Sin(flipy ? -angle : angle);

                path.AddArc(centerx - arcr, centery - arcr, arcr * 2, arcr * 2, arcStartDeg, arcSweepDeg);
                path.AddLine(outerx, outery, outerx, outery);
                path.CloseFigure();
                grap.FillPath(brush, path);
            }
        }
    }
}
