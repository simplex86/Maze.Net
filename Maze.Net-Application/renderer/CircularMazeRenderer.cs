using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    /// <summary>
    /// 圆形迷宫渲染器
    /// </summary>
    internal class CircularMazeRenderer
    {
        private int width = 0;
        private int height = 0;
        private int thickness = 1;
        private int offsetx = 0;
        private int offsety = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public CircularMazeRenderer SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public CircularMazeRenderer SetThickness(int thickness)
        {
            this.thickness = thickness;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public CircularMazeRenderer SetOffset(int x, int y)
        {
            this.offsetx = x;
            this.offsety = y;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grap"></param>
        /// <param name="field"></param>
        public void Draw(Graphics grap, CircularMazeField field)
        {
            grap.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

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
        /// 画迷宫
        /// </summary>
        private void DrawField(Graphics grap, CircularMazeField field)
        {
            if (field.rings == 0 || thickness <= 0 || width <= 0 || height <= 0)
                return;

            // 计算画布中心（圆心与画布中心对齐）
            var centerX = width / 2.0f + offsetx;
            var centerY = height / 2.0f + offsety;

            var pen = new Pen(Color.Black);

            // 获取最大的扇形数（通常是最外圈），用于所有径向墙对齐
            var maxSectors = 0;
            for (var r = 0; r < field.rings; r++)
            {
                var s = field.GetSectorsInRing(r);
                if (s > maxSectors) maxSectors = s;
            }
            var maxAngleStep = 2 * Math.PI / maxSectors;

            // 1. 绘制所有内圈墙（圆弧墙，分隔相邻圈）
            for (var r = 0; r < field.rings - 1; r++)
            {
                // 对于分隔圈 r 和 r+1 的内圈墙，应该使用外圈（r+1）的扇形数
                var sectorsInOuterRing = field.GetSectorsInRing(r + 1);
                var angleStep = 2 * Math.PI / sectorsInOuterRing;
                var outerRadius = (r + 1) * thickness;

                for (var s = 0; s < sectorsInOuterRing; s++)
                {
                    // 将外圈的扇形映射到内圈的对应扇形
                    var mappedInnerSector = field.MapSector(r + 1, s, r);
                    
                    // 如果墙存在，绘制它
                    if (field.GetInnerWall(r, mappedInnerSector))
                    {
                        var startAngle = s * angleStep - Math.PI / 2;
                        DrawArc(grap, pen, centerX, centerY, outerRadius, startAngle, angleStep);
                    }
                }
            }

            // 2. 绘制所有径向墙（直线墙，分隔同一圈相邻扇形）
            for (var r = 0; r < field.rings; r++)
            {
                var sectorsInRing = field.GetSectorsInRing(r);
                var innerRadius = r * thickness;
                var outerRadius = (r + 1) * thickness;

                var drawn = new bool[sectorsInRing]; // 避免重复绘制

                // 遍历每一个可能的对齐位置（使用maxSectors）
                for (var align = 0; align < maxSectors; align++)
                {
                    // 计算当前对齐角度对应的该圈扇形
                    var s = (align * sectorsInRing) / maxSectors;
                    
                    // 检查墙是否存在且未绘制
                    if (field.GetRadialWall(r, s) && !drawn[s])
                    {
                        // 用最大扇形数的角度来绘制，确保对齐！
                        var angle = align * maxAngleStep - Math.PI / 2;
                        DrawRadialLine(grap, pen, centerX, centerY, innerRadius, outerRadius, angle);
                        drawn[s] = true;
                    }
                }
            }

            // 3. 绘制边界（最内圈和最外圈总是可见）
            var innermostRadius = 0;
            var outermostRadius = field.rings * thickness;

            // 最内圈（完整圆）
            if (innermostRadius > 0)
            {
                DrawArc(grap, pen, centerX, centerY, innermostRadius, -Math.PI / 2, 2 * Math.PI);
            }

            // 最外圈（完整圆）
            if (outermostRadius > 0)
            {
                DrawArc(grap, pen, centerX, centerY, outermostRadius, -Math.PI / 2, 2 * Math.PI);
            }
        }

        /// <summary>
        /// 绘制圆弧
        /// </summary>
        private void DrawArc(Graphics grap, Pen pen, float centerX, float centerY, int radius, double startAngle, double sweepAngle)
        {
            if (radius <= 0)
                return;

            var x = centerX - radius;
            var y = centerY - radius;
            var diameter = radius * 2;

            // 将弧度转换为角度（GDI+使用角度）
            var startAngleDeg = (float)(startAngle * 180 / Math.PI);
            var sweepAngleDeg = (float)(sweepAngle * 180 / Math.PI);

            grap.DrawArc(pen, x, y, diameter, diameter, startAngleDeg, sweepAngleDeg);
        }

        /// <summary>
        /// 绘制径向线（从内圈到外圈的直线）
        /// </summary>
        private void DrawRadialLine(Graphics grap, Pen pen, float centerX, float centerY, int innerRadius, int outerRadius, double angle)
        {
            if (innerRadius < 0 || outerRadius <= innerRadius)
                return;

            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);

            // 使用float来提高精度，减少整数转换误差
            var x1 = centerX + innerRadius * cos;
            var y1 = centerY + innerRadius * sin;
            var x2 = centerX + outerRadius * cos;
            var y2 = centerY + outerRadius * sin;

            grap.DrawLine(pen, x1, y1, x2, y2);
        }
    }
}
