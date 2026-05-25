using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    /// <summary>
    /// 蜂窝迷宫渲染器
    /// 基于邻接表中的 CellBorder 几何数据进行绘制
    /// </summary>
    internal class HoneycombMazeRenderer
    {
        private int width = 0;
        private int height = 0;
        private int thickness = 10;
        private int offsetx = 0;
        private int offsety = 0;

        /// <summary>
        /// 设置绘制尺寸
        /// </summary>
        public HoneycombMazeRenderer SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        /// <summary>
        /// 设置格子厚度
        /// </summary>
        public HoneycombMazeRenderer SetThickness(int thickness)
        {
            this.thickness = thickness;
            return this;
        }

        /// <summary>
        /// 设置偏移量
        /// </summary>
        public HoneycombMazeRenderer SetOffset(int x, int y)
        {
            this.offsetx = x;
            this.offsety = y;
            return this;
        }

        /// <summary>
        /// 绘制迷宫
        /// </summary>
        public void Draw(Graphics grap, HoneycombMazeField? field)
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
        private void DrawField(Graphics grap, HoneycombMazeField field)
        {
            if (field.length == 0 || thickness <= 0 || width <= 0 || height <= 0)
                return;

            double xlim = Math.Sqrt(3) * (field.length - 0.5);
            double ylim = 1.5 * field.length - 0.5;

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
                }
            }

            pen.Dispose();
        }
    }
}
