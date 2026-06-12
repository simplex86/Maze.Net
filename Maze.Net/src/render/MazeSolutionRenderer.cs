using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class MazeSolutionRenderer
    {
        private MazeField field;
        private MazeSolution solution;
        private MazeGate gate;
        private CoordinateTransform transform = new CoordinateTransform();

        public MazeSolutionRenderer SetField(MazeField field)
        {
            this.field = field;
            return this;
        }

        public MazeSolutionRenderer SetSolution(MazeSolution solution)
        {
            this.solution = solution;
            return this;
        }

        public MazeSolutionRenderer SetGate(MazeGate gate)
        {
            this.gate = gate;
            return this;
        }

        public MazeSolutionRenderer SetSize(int width, int height)
        {
            transform.Width = width;
            transform.Height = height;
            return this;
        }

        public MazeSolutionRenderer SetThickness(int thickness)
        {
            transform.ScaleX = thickness;
            transform.ScaleY = thickness;
            return this;
        }

        public MazeSolutionRenderer SetThickness(float scaleX, float scaleY)
        {
            transform.ScaleX = scaleX;
            transform.ScaleY = scaleY;
            return this;
        }

        public MazeSolutionRenderer SetOffset(int dx, int dy)
        {
            transform.Dx = dx;
            transform.Dy = dy;
            return this;
        }

        public MazeSolutionRenderer SetPadding(int x, int y)
        {
            transform.PaddingX = x;
            transform.PaddingY = y;
            return this;
        }

        public void Draw(IGraphicsContext context)
        {
            if (field == null || field.VertexCount == 0) return;
            if (solution.Count < 2) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            var offsetx = transform.GetOffsetX(bounds);
            var offsety = transform.GetOffsetY(bounds);
            var flipy = field.FlipY;

            var points = new List<MazePoint>();

            foreach (var vertex in solution)
            {
                var centroid = ComputeCellCentroid(vertex);
                points.Add(new MazePoint(
                    transform.TransformX(centroid.X, bounds, offsetx),
                    transform.TransformY(centroid.Y, bounds, offsety, flipy)));
            }

            if (points.Count < 2) return;

            var width = Math.Max(2f, Math.Min(Math.Min(transform.ScaleX, transform.ScaleY), 2));
            for (int i = 1; i < points.Count; i++)
            {
                context.DrawLine(points[i - 1], points[i], MazeColor.Red, width);
            }
        }

        private Vertex ComputeCellCentroid(int vertex)
        {
            double sumX = 0, sumY = 0;
            int count = 0;

            foreach (var edge in field.Graph[vertex])
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
}
