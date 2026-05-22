using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    /// <summary>
    /// 矩形迷宫渲染器
    /// 基于墙的存在状态进行绘制
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
        public void Draw(Graphics grap, RectangularMazeField field)
        {
            DrawBackground(grap);
            DrawField(grap, field);
        }

        /// <summary>
        /// 画背景
        /// </summary>
        private void DrawBackground(Graphics grap)
        {
            var brush = new SolidBrush(Color.White);
            grap.FillRectangle(brush, 0, 0, width, height);
        }

        /// <summary>
        /// 绘制迷宫
        /// </summary>
        private void DrawField(Graphics grap, RectangularMazeField field)
        {
            if (field.width == 0 || field.height == 0)
            {
                return;
            }

            // 计算迷宫在画布上的居中位置
            int mazeWidth = field.width * thickness;
            int mazeHeight = field.height * thickness;
            int cx = (width - mazeWidth) / 2 + offsetx;
            int cy = (height - mazeHeight) / 2 + offsety;

            var pen = new Pen(Color.Black);

            // 1. 绘制内部横向墙（只绘制 y 从 1 到 height-1）
            for (int y = 1; y < field.height; y++)
            {
                for (int x = 0; x < field.width; x++)
                {
                    if (field.GetHorizontalWall(x, y))
                    {
                        int x1 = cx + x * thickness;
                        int x2 = cx + (x + 1) * thickness;
                        int yPos = cy + y * thickness;
                        grap.DrawLine(pen, x1, yPos, x2, yPos);
                    }
                }
            }

            // 2. 绘制内部纵向墙（只绘制 x 从 1 到 width-1）
            for (int y = 0; y < field.height; y++)
            {
                for (int x = 1; x < field.width; x++)
                {
                    if (field.GetVerticalWall(x, y))
                    {
                        int xPos = cx + x * thickness;
                        int y1 = cy + y * thickness;
                        int y2 = cy + (y + 1) * thickness;
                        grap.DrawLine(pen, xPos, y1, xPos, y2);
                    }
                }
            }

            // 3. 绘制外边界（完整矩形）
            grap.DrawRectangle(pen, cx, cy, mazeWidth, mazeHeight);
        }
    }
}
