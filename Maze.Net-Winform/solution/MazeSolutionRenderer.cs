using System;
using System.Collections.Generic;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class MazeSolutionRenderer
    {
        private MazeField field;
        private MazeSolution solution;
        private MazeGate gate;
        protected CoordinateTransform transform = new CoordinateTransform();

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
            transform.width = width;
            transform.height = height;
            return this;
        }

        public MazeSolutionRenderer SetThickness(int thickness)
        {
            transform.scale = thickness;
            return this;
        }

        public MazeSolutionRenderer SetOffset(int dx, int dy)
        {
            transform.dx = dx;
            transform.dy = dy;
            return this;
        }

        public void Draw(Graphics grap)
        {
            if (field == null || field.VertexCount == 0) return;
            if (solution.Count < 2) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            var offsetx = transform.GetOffsetX(bounds);
            var offsety = transform.GetOffsetY(bounds);
            var flipy = field.FlipY;

            var points = new List<PointF>();

            foreach (var vertex in solution)
            {
                var centroid = ComputeCellCentroid(vertex);
                points.Add(new PointF(
                    transform.TransformX(centroid.X, bounds, offsetx),
                    transform.TransformY(centroid.Y, bounds, offsety, flipy)));
            }

            if (points.Count < 2) return;

            using var pen = new Pen(Color.Red, Math.Max(2f, Math.Min(transform.scale, 2)));
            for (int i = 1; i < points.Count; i++)
            {
                grap.DrawLine(pen, points[i - 1], points[i]);
            }
        }

        /// <summary>
        /// 计算格子的质心（所有边框端点的平均值）
        /// </summary>
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
