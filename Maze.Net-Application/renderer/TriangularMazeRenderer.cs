using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    /// <summary>
    /// 三角形迷宫渲染器
    /// 基于邻接表中的 CellBorder 几何数据进行绘制
    /// </summary>
    internal class TriangularMazeRenderer
    {
        private int width = 0;
        private int height = 0;
        private int thickness = 10;
        private int offsetx = 0;
        private int offsety = 0;

        /// <summary>
        /// 设置绘制尺寸
        /// </summary>
        public TriangularMazeRenderer SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        /// <summary>
        /// 设置格子厚度
        /// </summary>
        public TriangularMazeRenderer SetThickness(int thickness)
        {
            this.thickness = thickness;
            return this;
        }

        /// <summary>
        /// 设置偏移量
        /// </summary>
        public TriangularMazeRenderer SetOffset(int x, int y)
        {
            this.offsetx = x;
            this.offsety = y;
            return this;
        }

        /// <summary>
        /// 绘制迷宫
        /// </summary>
        public void Draw(Graphics grap, TriangularMazeField? field)
        {
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
        /// 绘制迷宫
        /// </summary>
        private void DrawField(Graphics grap, TriangularMazeField field)
        {
            if (field.order == 0 || thickness <= 0 || width <= 0 || height <= 0)
                return;

            double mazeWidth = field.order;
            double mazeHeight = field.order * Math.Sqrt(3) / 2;

            float cx = (float)((width - mazeWidth * thickness) / 2.0) + offsetx;
            float cy = (float)((height - mazeHeight * thickness) / 2.0) + offsety;

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
                        float x1 = cx + (float)(line.X1 * thickness);
                        float y1 = cy + (float)(line.Y1 * thickness);
                        float x2 = cx + (float)(line.X2 * thickness);
                        float y2 = cy + (float)(line.Y2 * thickness);
                        grap.DrawLine(pen, x1, y1, x2, y2);
                    }
                }
            }

            pen.Dispose();
        }
    }
}
