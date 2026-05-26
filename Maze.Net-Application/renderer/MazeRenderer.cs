using System;
using System.Collections.Generic;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal abstract class MazeRenderer<TField> where TField : MazeField
    {
        protected int width;
        protected int height;
        protected int thickness;
        protected int offsetx;
        protected int offsety;

        public MazeRenderer<TField> SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        public MazeRenderer<TField> SetThickness(int thickness)
        {
            this.thickness = thickness;
            return this;
        }

        public MazeRenderer<TField> SetOffset(int x, int y)
        {
            this.offsetx = x;
            this.offsety = y;
            return this;
        }

        public void Draw(Graphics grap, TField? field)
        {
            DrawBackground(grap);

            if (field == null) return;
            DrawField(grap, field);
        }

        private void DrawBackground(Graphics grap)
        {
            using var brush = new SolidBrush(Color.White);
            grap.FillRectangle(brush, 0, 0, width, height);
        }

        /// <summary>
        /// 通用绘制：根据 Bounds 和 FlipY 统一计算变换，遍历所有边界绘制
        /// </summary>
        protected virtual void DrawField(Graphics grap, TField field)
        {
            if (field.count == 0) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            float scale = thickness;
            float offsetX = (float)((width - bounds.Width * scale) / 2) + offsetx;
            float offsetY = (float)((height - bounds.Height * scale) / 2) + offsety;
            bool flipY = field.FlipY;

            using var pen = new Pen(Color.Black);

            IterateBorders(field, border =>
            {
                if (border is LineBorder line)
                {
                    float x1 = TransformX(line.X1, bounds, scale, offsetX);
                    float y1 = TransformY(line.Y1, bounds, scale, offsetY, flipY);
                    float x2 = TransformX(line.X2, bounds, scale, offsetX);
                    float y2 = TransformY(line.Y2, bounds, scale, offsetY, flipY);
                    grap.DrawLine(pen, x1, y1, x2, y2);
                }
                else if (border is ArcBorder arc)
                {
                    float cx = TransformX(arc.CenterX, bounds, scale, offsetX);
                    float cy = TransformY(arc.CenterY, bounds, scale, offsetY, flipY);
                    float radius = (float)(arc.Radius * scale);
                    if (radius <= 0) return;

                    float startAngleDeg, sweepAngleDeg;
                    if (flipY)
                    {
                        startAngleDeg = (float)(-arc.StartAngle * 180.0 / Math.PI);
                        sweepAngleDeg = (float)(-arc.SweepAngle * 180.0 / Math.PI);
                    }
                    else
                    {
                        startAngleDeg = (float)(arc.StartAngle * 180.0 / Math.PI);
                        sweepAngleDeg = (float)(arc.SweepAngle * 180.0 / Math.PI);
                    }

                    grap.DrawArc(pen, cx - radius, cy - radius, radius * 2, radius * 2, startAngleDeg, sweepAngleDeg);
                }
            });
        }

        /// <summary>
        /// X坐标变换：field坐标 → 屏幕坐标
        /// </summary>
        protected float TransformX(double x, CoordinateBounds bounds, float scale, float offsetX)
        {
            return (float)((x - bounds.MinX) * scale) + offsetX;
        }

        /// <summary>
        /// Y坐标变换：field坐标 → 屏幕坐标
        /// </summary>
        protected float TransformY(double y, CoordinateBounds bounds, float scale, float offsetY, bool flipY)
        {
            if (flipY)
                return (float)((bounds.MaxY - y) * scale) + offsetY;
            else
                return (float)((y - bounds.MinY) * scale) + offsetY;
        }

        /// <summary>
        /// 遍历所有边界（去重：仅绘制 boundary 边和 neighbor > v 的内部边）
        /// </summary>
        protected void IterateBorders(TField field, Action<IMazeBorder> onBorder)
        {
            var graph = field.graph;
            for (int v = 0; v < graph.Count; v++)
            {
                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor != -1 && edge.Neighbor <= v)
                        continue;

                    if (edge.Border != null)
                        onBorder(edge.Border);
                }
            }
        }
    }
}
