using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class MazeRenderer
    {
        private MazeField field;
        private MazeGate gate;
        protected CoordinateTransform transform = new CoordinateTransform();

        public MazeRenderer SetSize(int width, int height)
        {
            transform.width = width;
            transform.height = height;
            return this;
        }

        public MazeRenderer SetThickness(int thickness)
        {
            transform.scale = thickness;
            return this;
        }

        public MazeRenderer SetOffset(int dx, int dy)
        {
            transform.dx = dx;
            transform.dy = dy;
            return this;
        }

        public MazeRenderer SetField(MazeField field)
        {
            this.field = field;
            return this;
        }

        public MazeRenderer SetGate(MazeGate gate)
        {
            this.gate = gate;
            return this;
        }

        public void Draw(Graphics grap)
        {
            DrawBackground(grap);
            DrawField(grap);
        }

        private void DrawBackground(Graphics grap)
        {
            using var brush = new SolidBrush(Color.White);
            grap.FillRectangle(brush, 0, 0, transform.width, transform.height);
        }

        /// <summary>
        /// 通用绘制：根据 Bounds 和 FlipY 统一计算变换，遍历所有边界绘制
        /// </summary>
        protected virtual void DrawField(Graphics grap)
        {
            if (field == null || field.VertexCount == 0) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            var offsetx = transform.GetOffsetX(bounds);
            var offsety = transform.GetOffsetY(bounds);
            var flipy = field.FlipY;

            using var pen = new Pen(Color.Black);

            IterateBorders(field, border =>
            {
                if (border is LineBorder line)
                {
                    var x1 = transform.TransformX(line.X1, bounds, offsetx);
                    var y1 = transform.TransformY(line.Y1, bounds, offsety, flipy);
                    var x2 = transform.TransformX(line.X2, bounds, offsetx);
                    var y2 = transform.TransformY(line.Y2, bounds, offsety, flipy);
                    grap.DrawLine(pen, x1, y1, x2, y2);
                }
                else if (border is ArcBorder arc)
                {
                    var cx = transform.TransformX(arc.CenterX, bounds, offsetx);
                    var cy = transform.TransformY(arc.CenterY, bounds, offsety, flipy);
                    var radius = (float)(arc.Radius * transform.scale);
                    if (radius <= 0) return;

                    var startAngleDeg = 0.0f;
                    var sweepAngleDeg = 0.0f;

                    if (flipy)
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
        /// 遍历所有边界（去重：仅绘制 boundary 边和 neighbor > v 的内部边）
        /// </summary>
        protected void IterateBorders(MazeField field, Action<IMazeBorder> onBorder)
        {
            var graph = field.Graph;
            for (int v = 0; v < graph.Count; v++)
            {
                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor != -1 && edge.Neighbor <= v)
                        continue;

                    if (edge.Neighbor == -1 && (v == gate.Entrance || v == gate.Exit))
                        continue;

                    if (!edge.IsOpen) 
                        onBorder(edge.Border);
                }
            }
        }
    }
}
