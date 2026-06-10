using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using SimplexLab.Maze;

namespace Maze.Avalonia.Rendering;

internal class MazeSolutionRenderer
{
    private MazeField? _field;
    private MazeSolution _solution;
    private MazeGate _gate;
    private readonly CoordinateTransform _transform = new();

    public MazeSolutionRenderer SetField(MazeField field)
    {
        _field = field;
        return this;
    }

    public MazeSolutionRenderer SetSolution(MazeSolution solution)
    {
        _solution = solution;
        return this;
    }

    public MazeSolutionRenderer SetGate(MazeGate gate)
    {
        _gate = gate;
        return this;
    }

    public MazeSolutionRenderer SetSize(int width, int height)
    {
        _transform.Width = width;
        _transform.Height = height;
        return this;
    }

    public MazeSolutionRenderer SetThickness(int thickness)
    {
        _transform.Scale = thickness;
        return this;
    }

    public MazeSolutionRenderer SetOffset(int dx, int dy)
    {
        _transform.Dx = dx;
        _transform.Dy = dy;
        return this;
    }

    public void Draw(DrawingContext context)
    {
        if (_field == null || _field.VertexCount == 0) return;
        if (_solution.Count < 2) return;

        var bounds = _field.Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0) return;

        var offsetx = _transform.GetOffsetX(bounds);
        var offsety = _transform.GetOffsetY(bounds);
        var flipy = _field.FlipY;

        var points = new List<Point>();
        for (int i = 0; i < _solution.Count; i++)
        {
            var vertex = _solution[i];
            var centroid = ComputeCellCentroid(vertex);
            points.Add(new Point(
                _transform.TransformX(centroid.X, bounds, offsetx),
                _transform.TransformY(centroid.Y, bounds, offsety, flipy)));
        }

        if (points.Count < 2) return;

        var pen = new Pen(Brushes.Red, Math.Max(2f, Math.Min(_transform.Scale, 2)));
        for (int i = 1; i < points.Count; i++)
        {
            context.DrawLine(pen, points[i - 1], points[i]);
        }
    }

    private Vertex ComputeCellCentroid(int vertex)
    {
        if (_field == null) return new Vertex(0, 0);

        double sumX = 0, sumY = 0;
        int count = 0;

        foreach (var edge in _field.Graph[vertex])
        {
            if (edge.Border is LineBorder line)
            {
                sumX += line.X1 + line.X2;
                sumY += line.Y1 + line.Y2;
                count += 2;
            }
            else if (edge.Border is ArcBorder arc)
            {
                var startX = arc.CenterX + arc.Radius * Math.Cos(arc.StartAngle);
                var startY = arc.CenterY + arc.Radius * Math.Sin(arc.StartAngle);
                var endAngle = arc.StartAngle + arc.SweepAngle;
                var endX = arc.CenterX + arc.Radius * Math.Cos(endAngle);
                var endY = arc.CenterY + arc.Radius * Math.Sin(endAngle);
                sumX += startX + endX;
                sumY += startY + endY;
                count += 2;
            }
        }

        return count > 0 ? new Vertex(sumX / count, sumY / count) : new Vertex(0, 0);
    }
}
