using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    /// <summary>
    /// 矩形迷宫渲染器
    /// 基于邻接表中的 CellBorder 几何数据进行绘制
    /// </summary>
    internal class RectangularMazeRenderer
    {
        private int width = 0;
        private int height = 0;
        private int thickness = 10;
        private int offsetx = 0;
        private int offsety = 0;

        /// <summary>
        /// 设置绘制尺寸
        /// </summary>
        public RectangularMazeRenderer SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        /// <summary>
        /// 设置格子厚度
        /// </summary>
        public RectangularMazeRenderer SetThickness(int thickness)
        {
            this.thickness = thickness;
            return this;
        }

        /// <summary>
        /// 设置偏移量
        /// </summary>
        public RectangularMazeRenderer SetOffset(int x, int y)
        {
            this.offsetx = x;
            this.offsety = y;
            return this;
        }

        /// <summary>
        /// 绘制迷宫
        /// </summary>
        public void Draw(Graphics grap, RectangularMazeField? field)
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
        private void DrawField(Graphics grap, RectangularMazeField field)
        {
            if (field.width == 0 || field.height == 0)
                return;

            // 计算迷宫在画布上的居中位置
            int mazeWidth = field.width * thickness;
            int mazeHeight = field.height * thickness;
            int cx = (width - mazeWidth) / 2 + offsetx;
            int cy = (height - mazeHeight) / 2 + offsety;

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
                        int x1 = cx + (int)(line.X1 * thickness);
                        int y1 = cy + (int)(line.Y1 * thickness);
                        int x2 = cx + (int)(line.X2 * thickness);
                        int y2 = cy + (int)(line.Y2 * thickness);
                        grap.DrawLine(pen, x1, y1, x2, y2);
                    }
                }
            }

            pen.Dispose();
        }
    }
}
