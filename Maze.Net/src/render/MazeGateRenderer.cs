using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class MazeGateRenderer
    {
        private MazeField field;
        private MazeGate gate;
        private CoordinateTransform transform = new CoordinateTransform();

        public MazeGateRenderer SetSize(int width, int height)
        {
            transform.Width = width;
            transform.Height = height;
            return this;
        }

        public MazeGateRenderer SetThickness(int thickness)
        {
            transform.ScaleX = thickness;
            transform.ScaleY = thickness;
            return this;
        }

        public MazeGateRenderer SetThickness(float scaleX, float scaleY)
        {
            transform.ScaleX = scaleX;
            transform.ScaleY = scaleY;
            return this;
        }

        public MazeGateRenderer SetOffset(int dx, int dy)
        {
            transform.Dx = dx;
            transform.Dy = dy;
            return this;
        }

        public MazeGateRenderer SetPadding(int x, int y)
        {
            transform.PaddingX = x;
            transform.PaddingY = y;
            return this;
        }

        public MazeGateRenderer SetField(MazeField field)
        {
            this.field = field;
            return this;
        }

        public MazeGateRenderer SetGate(MazeGate gate)
        {
            this.gate = gate;
            return this;
        }

        public void Draw(IGraphicsContext context)
        {
            if (field == null || field.VertexCount == 0) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            var offsetx = transform.GetOffsetX(bounds);
            var offsety = transform.GetOffsetY(bounds);
            var flipy = field.FlipY;

            if (gate.Entrance >= 0)
                DrawVertexMarker(context, gate.Entrance, MazeColor.Green, bounds, offsetx, offsety, flipy);
            if (gate.Exit >= 0)
                DrawVertexMarker(context, gate.Exit, MazeColor.Yellow, bounds, offsetx, offsety, flipy);
        }

        private void DrawVertexMarker(IGraphicsContext context, int vertex, MazeColor color,
            CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
        {
            var shape = field.GetCellShape(vertex);

            switch (shape.Type)
            {
                case CellShapeType.Polygon:
                    DrawPolygonMarker(context, shape.Vertices, color, bounds, offsetx, offsety, flipy);
                    break;
                case CellShapeType.AnnularSector:
                    DrawAnnularSectorMarker(context, shape.Sector, color, bounds, offsetx, offsety, flipy);
                    break;
                case CellShapeType.CurvedTriangle:
                    DrawCurvedTriangleMarker(context, shape.CurvedTriangle, color, bounds, offsetx, offsety, flipy);
                    break;
            }
        }

        private void DrawPolygonMarker(IGraphicsContext context, Vertex[] vertices, MazeColor color,
            CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
        {
            if (vertices == null || vertices.Length == 0) return;

            double cx = 0, cy = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                cx += vertices[i].X;
                cy += vertices[i].Y;
            }
            cx /= vertices.Length;
            cy /= vertices.Length;

            var shrinkX = 1.0 - 1.0 / transform.ScaleX;
            var shrinkY = 1.0 - 1.0 / transform.ScaleY;
            if (shrinkX <= 0 || shrinkY <= 0) return;

            var points = new List<MazePoint>();
            for (int i = 0; i < vertices.Length; i++)
            {
                var vx = cx + (vertices[i].X - cx) * shrinkX;
                var vy = cy + (vertices[i].Y - cy) * shrinkY;

                var xx = transform.TransformX(vx, bounds, offsetx);
                var yy = transform.TransformY(vy, bounds, offsety, flipy);

                points.Add(new MazePoint(xx, yy));
            }

            context.FillPolygon(points, color);
        }

        private void DrawAnnularSectorMarker(IGraphicsContext context, AnnularSector sector, MazeColor color,
            CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
        {
            var centerx = transform.TransformX(0, bounds, offsetx);
            var centery = transform.TransformY(0, bounds, offsety, flipy);

            var innerr = sector.InnerRadius > 0 ? (float)(sector.InnerRadius * transform.ScaleX) + 1 : 0;
            var outerr = (float)(sector.OuterRadius * transform.ScaleX) - 1;

            if (outerr <= 0) return;

            var midr = (float)((sector.InnerRadius + sector.OuterRadius) / 2 * transform.ScaleX);
            var angleShrink = midr > 0 ? 1.0f / midr : 0;
            var adjustedStartAngle = (float)(sector.StartAngle + angleShrink);
            var adjustedSweepAngle = (float)(sector.SweepAngle - 2 * angleShrink);

            if (adjustedSweepAngle <= 0) return;

            double startAngleDeg, sweepAngleDeg;
            if (flipy)
            {
                startAngleDeg = -adjustedStartAngle * 180.0 / Math.PI;
                sweepAngleDeg = -adjustedSweepAngle * 180.0 / Math.PI;
            }
            else
            {
                startAngleDeg = adjustedStartAngle * 180.0 / Math.PI;
                sweepAngleDeg = adjustedSweepAngle * 180.0 / Math.PI;
            }

            context.FillAnnularSector(new MazePoint(centerx, centery), outerr, innerr, startAngleDeg, sweepAngleDeg, color);
        }

        private void DrawCurvedTriangleMarker(IGraphicsContext context, CurvedTriangle ct, MazeColor color,
            CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
        {
            var centerx = transform.TransformX(0, bounds, offsetx);
            var centery = transform.TransformY(0, bounds, offsety, flipy);

            var arcr = (float)(ct.ArcRadius * transform.ScaleX) - 1;
            if (arcr <= 0) return;

            var angleShrink = arcr > 0 ? 1.0f / arcr : 0;
            var adjustedArcStart = (float)(ct.ArcStartAngle + angleShrink);
            var adjustedArcSweep = (float)(ct.ArcSweepAngle - 2 * angleShrink);
            if (adjustedArcSweep <= 0) return;

            double arcStartDeg, arcSweepDeg;
            if (flipy)
            {
                arcStartDeg = -adjustedArcStart * 180.0 / Math.PI;
                arcSweepDeg = -adjustedArcSweep * 180.0 / Math.PI;
            }
            else
            {
                arcStartDeg = adjustedArcStart * 180.0 / Math.PI;
                arcSweepDeg = adjustedArcSweep * 180.0 / Math.PI;
            }

            if (ct.Upward)
            {
                var innerr = ct.InnerRadius > 0 ? (float)(ct.InnerRadius * transform.ScaleX) + 1 : 0;

                if (innerr <= 0)
                {
                    context.FillAnnularSector(new MazePoint(centerx, centery), arcr, 0, arcStartDeg, arcSweepDeg, color);
                }
                else
                {
                    var angle = (float)ct.InnerAngle;
                    var innerx = centerx + innerr * (float)Math.Cos(flipy ? -angle : angle);
                    var innery = centery + innerr * (float)Math.Sin(flipy ? -angle : angle);

                    context.FillArcWedge(new MazePoint(centerx, centery), arcr, arcStartDeg, arcSweepDeg,
                        new MazePoint(innerx, innery), color);
                }
            }
            else
            {
                var outerr = (float)(ct.OuterRadius * transform.ScaleX) - 1;
                if (outerr <= 0) return;

                var angle = (float)ct.OuterAngle;
                var outerx = centerx + outerr * (float)Math.Cos(flipy ? -angle : angle);
                var outery = centery + outerr * (float)Math.Sin(flipy ? -angle : angle);

                context.FillArcWedge(new MazePoint(centerx, centery), arcr, arcStartDeg, arcSweepDeg,
                    new MazePoint(outerx, outery), color);
            }
        }
    }
}
