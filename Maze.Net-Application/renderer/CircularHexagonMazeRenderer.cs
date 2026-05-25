using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class CircularHexagonMazeRenderer
    {
        private int width = 0;
        private int height = 0;
        private int thickness = 10;
        private int offsetx = 0;
        private int offsety = 0;

        public CircularHexagonMazeRenderer SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        public CircularHexagonMazeRenderer SetThickness(int thickness)
        {
            this.thickness = thickness;
            return this;
        }

        public CircularHexagonMazeRenderer SetOffset(int x, int y)
        {
            this.offsetx = x;
            this.offsety = y;
            return this;
        }

        public void Draw(Graphics grap, CircularHexagonMazeField? field)
        {
            DrawBackground(grap);

            if (field == null) return;
            DrawField(grap, field);
        }

        private void DrawBackground(Graphics grap)
        {
            var brush = new SolidBrush(Color.White);
            grap.FillRectangle(brush, 0, 0, width, height);
            brush.Dispose();
        }

        private void DrawField(Graphics grap, CircularHexagonMazeField field)
        {
            if (field.size == 0 || thickness <= 0 || width <= 0 || height <= 0)
                return;

            float centerX = width / 2.0f + offsetx;
            float centerY = height / 2.0f + offsety;

            var pen = new Pen(Color.Black);

            var graph = field.graph;
            for (int v = 0; v < graph.Count; v++)
            {
                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor != -1 && edge.Neighbor <= v)
                        continue;

                    if (edge.Border is LineBorder line)
                    {
                        float x1 = centerX + (float)(line.X1 * thickness);
                        float y1 = centerY - (float)(line.Y1 * thickness);
                        float x2 = centerX + (float)(line.X2 * thickness);
                        float y2 = centerY - (float)(line.Y2 * thickness);
                        grap.DrawLine(pen, x1, y1, x2, y2);
                    }
                    else if (edge.Border is ArcBorder arc)
                    {
                        float cx = centerX + (float)(arc.CenterX * thickness);
                        float cy = centerY - (float)(arc.CenterY * thickness);
                        float radius = (float)(arc.Radius * thickness);
                        if (radius <= 0) continue;

                        float startAngleDeg = (float)(-arc.StartAngle * 180.0 / Math.PI);
                        float sweepAngleDeg = (float)(-arc.SweepAngle * 180.0 / Math.PI);
                        grap.DrawArc(pen, cx - radius, cy - radius, radius * 2, radius * 2, startAngleDeg, sweepAngleDeg);
                    }
                }
            }

            pen.Dispose();
        }
    }
}
