using System;
using Avalonia;
using Avalonia.Media;
using SimplexLab.Maze;

namespace Maze.Avalonia.Rendering;

internal class MazeRenderer
{
    private MazeField? _field;
    private MazeGate _gate;
    private readonly CoordinateTransform _transform = new();

    public MazeRenderer SetSize(int width, int height)
    {
        _transform.Width = width;
        _transform.Height = height;
        return this;
    }

    public MazeRenderer SetThickness(int thickness)
    {
        _transform.Scale = thickness;
        return this;
    }

    public MazeRenderer SetOffset(int dx, int dy)
    {
        _transform.Dx = dx;
        _transform.Dy = dy;
        return this;
    }

    public MazeRenderer SetField(MazeField field)
    {
        _field = field;
        return this;
    }

    public MazeRenderer SetGate(MazeGate gate)
    {
        _gate = gate;
        return this;
    }

    public void Draw(DrawingContext context)
    {
        DrawBackground(context);
        DrawField(context);
    }

    private void DrawBackground(DrawingContext context)
    {
        context.DrawRectangle(Brushes.White, null, new Rect(0, 0, _transform.Width, _transform.Height));
    }

    private void DrawField(DrawingContext context)
    {
        if (_field == null || _field.VertexCount == 0) return;

        var bounds = _field.Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0) return;

        var offsetx = _transform.GetOffsetX(bounds);
        var offsety = _transform.GetOffsetY(bounds);
        var flipy = _field.FlipY;

        var pen = new Pen(Brushes.Black);

        IterateBorders(border =>
        {
            if (border is LineBorder line)
            {
                var x1 = _transform.TransformX(line.X1, bounds, offsetx);
                var y1 = _transform.TransformY(line.Y1, bounds, offsety, flipy);
                var x2 = _transform.TransformX(line.X2, bounds, offsetx);
                var y2 = _transform.TransformY(line.Y2, bounds, offsety, flipy);
                context.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
            }
            else if (border is ArcBorder arc)
            {
                var cx = _transform.TransformX(arc.CenterX, bounds, offsetx);
                var cy = _transform.TransformY(arc.CenterY, bounds, offsety, flipy);
                var radius = arc.Radius * _transform.Scale;
                if (radius <= 0) return;

                double startAngleDeg, sweepAngleDeg;
                if (flipy)
                {
                    startAngleDeg = -arc.StartAngle * 180.0 / Math.PI;
                    sweepAngleDeg = -arc.SweepAngle * 180.0 / Math.PI;
                }
                else
                {
                    startAngleDeg = arc.StartAngle * 180.0 / Math.PI;
                    sweepAngleDeg = arc.SweepAngle * 180.0 / Math.PI;
                }

                var startRad = startAngleDeg * Math.PI / 180.0;
                var endRad = (startAngleDeg + sweepAngleDeg) * Math.PI / 180.0;

                var startPoint = new Point(cx + radius * Math.Cos(startRad),
                                           cy + radius * Math.Sin(startRad));
                var endPoint = new Point(cx + radius * Math.Cos(endRad),
                                        cy + radius * Math.Sin(endRad));

                var geometry = new StreamGeometry();
                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(startPoint, false);
                    ctx.ArcTo(endPoint, new Size(radius, radius), 0,
                        Math.Abs(sweepAngleDeg) > 180,
                        sweepAngleDeg < 0 ? SweepDirection.CounterClockwise : SweepDirection.Clockwise);
                    ctx.EndFigure(false);
                }
                context.DrawGeometry(null, pen, geometry);
            }
        });
    }

    private void IterateBorders(Action<IMazeBorder> onBorder)
    {
        if (_field == null) return;
        var graph = _field.Graph;

        for (int v = 0; v < graph.Count; v++)
        {
            foreach (var edge in graph[v])
            {
                if (edge.Neighbor != -1 && edge.Neighbor <= v)
                    continue;

                if (edge.Neighbor == -1 && v == _gate.Entrance && edge.Border == _gate.EntranceBorder)
                    continue;

                if (edge.Neighbor == -1 && v == _gate.Exit && edge.Border == _gate.ExitBorder)
                    continue;

                if (!edge.IsOpen && edge.Border != null)
                    onBorder(edge.Border);
            }
        }
    }
}
