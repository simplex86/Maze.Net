using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    /// <summary>
    /// 圆形迷宫渲染器
    /// 基于邻接表中的 CellBorder 几何数据进行绘制
    /// </summary>
    internal class CircularMazeRenderer
    {
        private int width = 0;
        private int height = 0;
        private int thickness = 1;
        private int offsetx = 0;
        private int offsety = 0;

        /// <summary>
        /// 设置绘制尺寸
        /// </summary>
        public CircularMazeRenderer SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        /// <summary>
        /// 设置格子厚度
        /// </summary>
        public CircularMazeRenderer SetThickness(int thickness)
        {
            this.thickness = thickness;
            return this;
        }

        /// <summary>
        /// 设置偏移量
        /// </summary>
        public CircularMazeRenderer SetOffset(int x, int y)
        {
            this.offsetx = x;
            this.offsety = y;
            return this;
        }

        /// <summary>
        /// 绘制迷宫
        /// </summary>
        public void Draw(Graphics grap, CircularMazeField? field)
        {
            grap.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            DrawBackground(grap);

            if (field == null) return;
            DrawField(grap, field);
        }

        /// <summary>
        /// 画背景
        /// </summary>
        private void DrawBackground(Graphics grap)
        {
            var brush = new SolidBrush(Color.White);
            grap.FillRectangle(brush, 0, 0, width, height);
            brush.Dispose();
        }

        /// <summary>
        /// 画迷宫
        /// </summary>
        private void DrawField(Graphics grap, CircularMazeField field)
        {
            if (field.rings == 0 || thickness <= 0 || width <= 0 || height <= 0)
                return;

            float centerX = width / 2.0f + offsetx;
            float centerY = height / 2.0f + offsety;

            var pen = new Pen(Color.Black);

            // 遍历邻接表，绘制所有未移除的边界
            var graph = field.graph;
            for (int v = 0; v < graph.Count; v++)
            {
                foreach (var edge in graph[v])
                {
                    // 避免重复绘制：边界边始终绘制，内部边仅当 neighbor > v 时绘制
                    if (edge.Neighbor != -1 && edge.Neighbor <= v)
                        continue;

                    if (edge.Border is LineBorder line)
                    {
                        float x1 = centerX + (float)(line.X1 * thickness);
                        float y1 = centerY + (float)(line.Y1 * thickness);
                        float x2 = centerX + (float)(line.X2 * thickness);
                        float y2 = centerY + (float)(line.Y2 * thickness);
                        grap.DrawLine(pen, x1, y1, x2, y2);
                    }
                    else if (edge.Border is ArcBorder arc)
                    {
                        float cx = centerX + (float)(arc.CenterX * thickness);
                        float cy = centerY + (float)(arc.CenterY * thickness);
                        float radius = (float)(arc.Radius * thickness);
                        if (radius <= 0) continue;

                        float startAngleDeg = (float)(arc.StartAngle * 180 / Math.PI);
                        float sweepAngleDeg = (float)(arc.SweepAngle * 180 / Math.PI);

                        grap.DrawArc(pen, cx - radius, cy - radius, radius * 2, radius * 2, startAngleDeg, sweepAngleDeg);
                    }
                }
            }

            pen.Dispose();
        }
    }
}
