using System;
using Avalonia;
using Avalonia.Media;
using SimplexLab.Maze;

namespace Maze.Avalonia.Rendering;

internal class MazeGateRenderer
{
    private MazeField? _field;
    private MazeGate _gate;
    private readonly CoordinateTransform _transform = new();

    public MazeGateRenderer SetSize(int width, int height)
    {
        _transform.Width = width;
        _transform.Height = height;
        return this;
    }

    public MazeGateRenderer SetThickness(int thickness)
    {
        _transform.Scale = thickness;
        return this;
    }

    public MazeGateRenderer SetOffset(int dx, int dy)
    {
        _transform.Dx = dx;
        _transform.Dy = dy;
        return this;
    }

    public MazeGateRenderer SetField(MazeField? field)
    {
        _field = field;
        return this;
    }

    public MazeGateRenderer SetGate(MazeGate gate)
    {
        _gate = gate;
        return this;
    }

    public void Draw(DrawingContext context)
    {
        if (_field == null || _field.VertexCount == 0) return;

        var bounds = _field.Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0) return;

        var offsetx = _transform.GetOffsetX(bounds);
        var offsety = _transform.GetOffsetY(bounds);
        var flipy = _field.FlipY;

        if (_gate.Entrance >= 0)
            DrawVertexMarker(context, _gate.Entrance, Brushes.Green, bounds, offsetx, offsety, flipy);
        if (_gate.Exit >= 0)
            DrawVertexMarker(context, _gate.Exit, Brushes.Yellow, bounds, offsetx, offsety, flipy);
    }

    private void DrawVertexMarker(DrawingContext context, int vertex, IBrush brush,
        CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
    {
        var shape = _field!.GetCellShape(vertex);

        switch (shape.Type)
        {
            case CellShapeType.Polygon:
                DrawPolygonMarker(context, shape.Vertices, brush, bounds, offsetx, offsety, flipy);
                break;
            case CellShapeType.AnnularSector:
                DrawAnnularSectorMarker(context, shape.Sector, brush, bounds, offsetx, offsety, flipy);
                break;
            case CellShapeType.CurvedTriangle:
                DrawCurvedTriangleMarker(context, shape.CurvedTriangle, brush, bounds, offsetx, offsety, flipy);
                break;
        }
    }

    private void DrawPolygonMarker(DrawingContext context, Vertex[] vertices, IBrush brush,
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

        var shrink = 1.0 - 1.0 / _transform.Scale;
        if (shrink <= 0) return;

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                var vx = cx + (vertices[i].X - cx) * shrink;
                var vy = cy + (vertices[i].Y - cy) * shrink;

                var xx = _transform.TransformX(vx, bounds, offsetx);
                var yy = _transform.TransformY(vy, bounds, offsety, flipy);

                if (i == 0)
                    ctx.BeginFigure(new Point(xx, yy), true);
                else
                    ctx.LineTo(new Point(xx, yy));
            }
            ctx.EndFigure(true);
        }
        context.DrawGeometry(brush, null, geometry);
    }

    private void DrawAnnularSectorMarker(DrawingContext context, AnnularSector sector, IBrush brush,
        CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
    {
        var centerx = _transform.TransformX(0, bounds, offsetx);
        var centery = _transform.TransformY(0, bounds, offsety, flipy);

        var innerr = sector.InnerRadius > 0 ? (float)(sector.InnerRadius * _transform.Scale) + 1 : 0;
        var outerr = (float)(sector.OuterRadius * _transform.Scale) - 1;

        if (outerr <= 0) return;

        var midr = (float)((sector.InnerRadius + sector.OuterRadius) / 2 * _transform.Scale);
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

        var startRad = startAngleDeg * Math.PI / 180.0;
        var endRad = (startAngleDeg + sweepAngleDeg) * Math.PI / 180.0;
        var isLargeArc = Math.Abs(sweepAngleDeg) > 180;
        var sweepDir = sweepAngleDeg < 0 ? SweepDirection.CounterClockwise : SweepDirection.Clockwise;

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            if (innerr <= 0)
            {
                // Pie shape: center -> outer arc start -> arc -> back to center
                ctx.BeginFigure(new Point(centerx, centery), true);
                ctx.LineTo(new Point(centerx + outerr * Math.Cos(startRad),
                                     centery + outerr * Math.Sin(startRad)));
                ctx.ArcTo(new Point(centerx + outerr * Math.Cos(endRad),
                                    centery + outerr * Math.Sin(endRad)),
                    new Size(outerr, outerr), 0, isLargeArc, sweepDir);
                ctx.EndFigure(true);
            }
            else
            {
                // Annular sector: outer arc + inner arc (reversed)
                ctx.BeginFigure(new Point(centerx + outerr * Math.Cos(startRad),
                                          centery + outerr * Math.Sin(startRad)), true);
                ctx.ArcTo(new Point(centerx + outerr * Math.Cos(endRad),
                                    centery + outerr * Math.Sin(endRad)),
                    new Size(outerr, outerr), 0, isLargeArc, sweepDir);
                ctx.LineTo(new Point(centerx + innerr * Math.Cos(endRad),
                                     centery + innerr * Math.Sin(endRad)));
                ctx.ArcTo(new Point(centerx + innerr * Math.Cos(startRad),
                                    centery + innerr * Math.Sin(startRad)),
                    new Size(innerr, innerr), 0, isLargeArc,
                    sweepDir == SweepDirection.Clockwise ? SweepDirection.CounterClockwise : SweepDirection.Clockwise);
                ctx.EndFigure(true);
            }
        }
        context.DrawGeometry(brush, null, geometry);
    }

    private void DrawCurvedTriangleMarker(DrawingContext context, CurvedTriangle ct, IBrush brush,
        CoordinateBounds bounds, float offsetx, float offsety, bool flipy)
    {
        var centerx = _transform.TransformX(0, bounds, offsetx);
        var centery = _transform.TransformY(0, bounds, offsety, flipy);

        var arcr = (float)(ct.ArcRadius * _transform.Scale) - 1;
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

        var startRad = arcStartDeg * Math.PI / 180.0;
        var endRad = (arcStartDeg + arcSweepDeg) * Math.PI / 180.0;
        var isLargeArc = Math.Abs(arcSweepDeg) > 180;
        var sweepDir = arcSweepDeg < 0 ? SweepDirection.CounterClockwise : SweepDirection.Clockwise;

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            if (ct.Upward)
            {
                var innerr = ct.InnerRadius > 0 ? (float)(ct.InnerRadius * _transform.Scale) + 1 : 0;

                ctx.BeginFigure(new Point(centerx + arcr * Math.Cos(startRad),
                                          centery + arcr * Math.Sin(startRad)), true);
                ctx.ArcTo(new Point(centerx + arcr * Math.Cos(endRad),
                                    centery + arcr * Math.Sin(endRad)),
                    new Size(arcr, arcr), 0, isLargeArc, sweepDir);

                if (innerr <= 0)
                {
                    ctx.LineTo(new Point(centerx, centery));
                }
                else
                {
                    var angle = (float)ct.InnerAngle;
                    var innerx = centerx + innerr * (float)Math.Cos(flipy ? -angle : angle);
                    var innery = centery + innerr * (float)Math.Sin(flipy ? -angle : angle);
                    ctx.LineTo(new Point(innerx, innery));
                }
                ctx.EndFigure(true);
            }
            else
            {
                var outerr = (float)(ct.OuterRadius * _transform.Scale) - 1;
                if (outerr <= 0) return;

                var angle = (float)ct.OuterAngle;
                var outerx = centerx + outerr * (float)Math.Cos(flipy ? -angle : angle);
                var outery = centery + outerr * (float)Math.Sin(flipy ? -angle : angle);

                ctx.BeginFigure(new Point(centerx + arcr * Math.Cos(startRad),
                                          centery + arcr * Math.Sin(startRad)), true);
                ctx.ArcTo(new Point(centerx + arcr * Math.Cos(endRad),
                                    centery + arcr * Math.Sin(endRad)),
                    new Size(arcr, arcr), 0, isLargeArc, sweepDir);
                ctx.LineTo(new Point(outerx, outery));
                ctx.EndFigure(true);
            }
        }
        context.DrawGeometry(brush, null, geometry);
    }
}
