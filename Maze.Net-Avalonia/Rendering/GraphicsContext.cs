using System;
using System.Collections.Generic;
using SimplexLab.Maze;
using AvaloniaPoint = global::Avalonia.Point;
using AvaloniaSize = global::Avalonia.Size;
using AvaloniaRect = global::Avalonia.Rect;
using AvaloniaMediaColor = global::Avalonia.Media.Color;

namespace Maze.Avalonia.Rendering;

internal class GraphicsContext : IGraphicsContext
{
    private readonly global::Avalonia.Media.DrawingContext _context;

    public GraphicsContext(global::Avalonia.Media.DrawingContext context)
    {
        _context = context;
    }

    public void DrawLine(MazePoint a, MazePoint b, MazeColor color, double width)
    {
        var pen = new global::Avalonia.Media.Pen(ToBrush(color), width);
        _context.DrawLine(pen, new AvaloniaPoint(a.X, a.Y), new AvaloniaPoint(b.X, b.Y));
    }

    public void DrawArc(MazePoint center, double radius, double startAngleDeg, double sweepAngleDeg, MazeColor color, double width)
    {
        var pen = new global::Avalonia.Media.Pen(ToBrush(color), width);

        var startRad = startAngleDeg * Math.PI / 180.0;
        var endRad = (startAngleDeg + sweepAngleDeg) * Math.PI / 180.0;

        var startPoint = new AvaloniaPoint(center.X + radius * Math.Cos(startRad),
                                           center.Y + radius * Math.Sin(startRad));
        var endPoint = new AvaloniaPoint(center.X + radius * Math.Cos(endRad),
                                         center.Y + radius * Math.Sin(endRad));

        var geometry = new global::Avalonia.Media.StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(startPoint, false);
            ctx.ArcTo(endPoint, new AvaloniaSize(radius, radius), 0,
                Math.Abs(sweepAngleDeg) > 180,
                sweepAngleDeg < 0 ? global::Avalonia.Media.SweepDirection.CounterClockwise : global::Avalonia.Media.SweepDirection.Clockwise);
            ctx.EndFigure(false);
        }
        _context.DrawGeometry(null, pen, geometry);
    }

    public void FillRectangle(MazePoint pt, MazeSize size, MazeColor color)
    {
        _context.DrawRectangle(ToBrush(color), null,
            new AvaloniaRect(pt.X, pt.Y, size.Width, size.Height));
    }

    public void FillPolygon(List<MazePoint> points, MazeColor color)
    {
        var geometry = new global::Avalonia.Media.StreamGeometry();
        using (var ctx = geometry.Open())
        {
            for (int i = 0; i < points.Count; i++)
            {
                var pt = new AvaloniaPoint(points[i].X, points[i].Y);
                if (i == 0)
                    ctx.BeginFigure(pt, true);
                else
                    ctx.LineTo(pt);
            }
            ctx.EndFigure(true);
        }
        _context.DrawGeometry(ToBrush(color), null, geometry);
    }

    public void FillAnnularSector(MazePoint center, double outerRadius, double innerRadius, double startAngleDeg, double sweepAngleDeg, MazeColor color)
    {
        var startRad = startAngleDeg * Math.PI / 180.0;
        var endRad = (startAngleDeg + sweepAngleDeg) * Math.PI / 180.0;
        var isLargeArc = Math.Abs(sweepAngleDeg) > 180;
        var sweepDir = sweepAngleDeg < 0 ? global::Avalonia.Media.SweepDirection.CounterClockwise : global::Avalonia.Media.SweepDirection.Clockwise;

        var geometry = new global::Avalonia.Media.StreamGeometry();
        using (var ctx = geometry.Open())
        {
            if (innerRadius <= 0)
            {
                ctx.BeginFigure(new AvaloniaPoint(center.X, center.Y), true);
                ctx.LineTo(new AvaloniaPoint(center.X + outerRadius * Math.Cos(startRad),
                                             center.Y + outerRadius * Math.Sin(startRad)));
                ctx.ArcTo(new AvaloniaPoint(center.X + outerRadius * Math.Cos(endRad),
                                            center.Y + outerRadius * Math.Sin(endRad)),
                    new AvaloniaSize(outerRadius, outerRadius), 0, isLargeArc, sweepDir);
                ctx.EndFigure(true);
            }
            else
            {
                ctx.BeginFigure(new AvaloniaPoint(center.X + outerRadius * Math.Cos(startRad),
                                                   center.Y + outerRadius * Math.Sin(startRad)), true);
                ctx.ArcTo(new AvaloniaPoint(center.X + outerRadius * Math.Cos(endRad),
                                            center.Y + outerRadius * Math.Sin(endRad)),
                    new AvaloniaSize(outerRadius, outerRadius), 0, isLargeArc, sweepDir);
                ctx.LineTo(new AvaloniaPoint(center.X + innerRadius * Math.Cos(endRad),
                                             center.Y + innerRadius * Math.Sin(endRad)));
                ctx.ArcTo(new AvaloniaPoint(center.X + innerRadius * Math.Cos(startRad),
                                            center.Y + innerRadius * Math.Sin(startRad)),
                    new AvaloniaSize(innerRadius, innerRadius), 0, isLargeArc,
                    sweepDir == global::Avalonia.Media.SweepDirection.Clockwise ? global::Avalonia.Media.SweepDirection.CounterClockwise : global::Avalonia.Media.SweepDirection.Clockwise);
                ctx.EndFigure(true);
            }
        }
        _context.DrawGeometry(ToBrush(color), null, geometry);
    }

    public void FillArcWedge(MazePoint center, double arcRadius, double startAngleDeg, double sweepAngleDeg, MazePoint closingPoint, MazeColor color)
    {
        var startRad = startAngleDeg * Math.PI / 180.0;
        var endRad = (startAngleDeg + sweepAngleDeg) * Math.PI / 180.0;
        var isLargeArc = Math.Abs(sweepAngleDeg) > 180;
        var sweepDir = sweepAngleDeg < 0 ? global::Avalonia.Media.SweepDirection.CounterClockwise : global::Avalonia.Media.SweepDirection.Clockwise;

        var geometry = new global::Avalonia.Media.StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(new AvaloniaPoint(center.X + arcRadius * Math.Cos(startRad),
                                               center.Y + arcRadius * Math.Sin(startRad)), true);
            ctx.ArcTo(new AvaloniaPoint(center.X + arcRadius * Math.Cos(endRad),
                                        center.Y + arcRadius * Math.Sin(endRad)),
                new AvaloniaSize(arcRadius, arcRadius), 0, isLargeArc, sweepDir);
            ctx.LineTo(new AvaloniaPoint(closingPoint.X, closingPoint.Y));
            ctx.EndFigure(true);
        }
        _context.DrawGeometry(ToBrush(color), null, geometry);
    }

    private static global::Avalonia.Media.IBrush ToBrush(MazeColor color)
    {
        return new global::Avalonia.Media.SolidColorBrush(AvaloniaMediaColor.FromArgb(color.A, color.R, color.G, color.B));
    }
}
