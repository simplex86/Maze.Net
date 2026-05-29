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

            AddGatePoint(gate.Entrance, bounds, offsetx, offsety, flipy, points);

            for (int i = 0; i < solution.Count - 1; i++)
            {
                if (ComputeBorderMidpoint(solution[i], solution[i + 1], out var mid))
                {
                    points.Add(new PointF(
                        transform.TransformX(mid.X, bounds, offsetx),
                        transform.TransformY(mid.Y, bounds, offsety, flipy)));
                }
            }

            AddGatePoint(gate.Exit, bounds, offsetx, offsety, flipy, points);

            if (points.Count < 2) return;

            using var pen = new Pen(Color.Red, Math.Max(2f, Math.Min(transform.scale, 2)));
            for (int i = 1; i < points.Count; i++)
            {
                grap.DrawLine(pen, points[i - 1], points[i]);
            }
        }

        private void AddGatePoint(int gateVertex, CoordinateBounds bounds, float offsetx, float offsety, bool flipy, List<PointF> points)
        {
            if (gateVertex < 0) return;

            foreach (var edge in field.Graph[gateVertex])
            {
                if (edge.Neighbor != -1) continue;

                if (ComputeBorderMidpoint(edge.Border, out var mid))
                {
                    points.Add(new PointF(
                        transform.TransformX(mid.X, bounds, offsetx),
                        transform.TransformY(mid.Y, bounds, offsety, flipy)));
                    return;
                }
            }
        }

        private bool ComputeBorderMidpoint(int from, int to, out Vertex mid)
        {
            foreach (var edge in field.Graph[from])
            {
                if (edge.Neighbor == to && edge.IsOpen)
                {
                    return ComputeBorderMidpoint(edge.Border, out mid);
                }
            }

            mid = new Vertex(0, 0);
            return false;
        }

        private bool ComputeBorderMidpoint(IMazeBorder? border, out Vertex mid)
        {
            if (border == null)
            {
                mid = new Vertex(0, 0);
                return false;
            }

            if (border is LineBorder line)
            {
                mid = new Vertex((line.X1 + line.X2) / 2, (line.Y1 + line.Y2) / 2);
                return true;
            }

            if (border is ArcBorder arc)
            {
                var midAngle = arc.StartAngle + arc.SweepAngle / 2;
                mid = new Vertex(arc.CenterX + arc.Radius * Math.Cos(midAngle),
                                 arc.CenterY + arc.Radius * Math.Sin(midAngle));
                return true;
            }

            mid = new Vertex(0, 0);
            return false;
        }
    }
}
