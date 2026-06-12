using System;

namespace SimplexLab.Maze
{
    public class MazeRenderer
    {
        private MazeField field;
        private MazeGate gate;
        private CoordinateTransform transform = new CoordinateTransform();

        public MazeRenderer SetSize(int width, int height)
        {
            transform.Width = width;
            transform.Height = height;
            return this;
        }

        public MazeRenderer SetThickness(int thickness)
        {
            transform.ScaleX = thickness;
            transform.ScaleY = thickness;
            return this;
        }

        public MazeRenderer SetThickness(float scaleX, float scaleY)
        {
            transform.ScaleX = scaleX;
            transform.ScaleY = scaleY;
            return this;
        }

        public MazeRenderer SetOffset(int dx, int dy)
        {
            transform.Dx = dx;
            transform.Dy = dy;
            return this;
        }

        public MazeRenderer SetPadding(int x, int y)
        {
            transform.PaddingX = x;
            transform.PaddingY = y;
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

        public void Draw(IGraphicsContext context)
        {
            DrawBackground(context);
            DrawField(context);
        }

        private void DrawBackground(IGraphicsContext context)
        {
            context.FillRectangle(new MazePoint(0, 0), new MazeSize(transform.Width, transform.Height), MazeColor.White);
        }

        private void DrawField(IGraphicsContext context)
        {
            if (field == null || field.VertexCount == 0) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            var offsetx = transform.GetOffsetX(bounds);
            var offsety = transform.GetOffsetY(bounds);
            var flipy = field.FlipY;

            IterateBorders(field, border =>
            {
                if (border is LineBorder line)
                {
                    var x1 = transform.TransformX(line.X1, bounds, offsetx);
                    var y1 = transform.TransformY(line.Y1, bounds, offsety, flipy);
                    var x2 = transform.TransformX(line.X2, bounds, offsetx);
                    var y2 = transform.TransformY(line.Y2, bounds, offsety, flipy);
                    context.DrawLine(new MazePoint(x1, y1), new MazePoint(x2, y2), MazeColor.Black, 1);
                }
                else if (border is ArcBorder arc)
                {
                    var cx = transform.TransformX(arc.CenterX, bounds, offsetx);
                    var cy = transform.TransformY(arc.CenterY, bounds, offsety, flipy);
                    var radius = arc.Radius * Math.Min(transform.ScaleX, transform.ScaleY);
                    if (radius <= 0) return;

                    double startAngleDeg, sweepAngleDeg;
                    if (flipy)
                    {
                        startAngleDeg = -arc.StartAngle * 180.0 / Math.PI;
                        sweepAngleDeg = -arc.SweepAngle * 180.0 / Math.PI;
                    }
                    else
                    {
                        startAngleDeg = arc.StartAngle * 180.0 / Math.PI;
                        sweepAngleDeg = arc.SweepAngle * 180.0 / Math.PI;
                    }

                    context.DrawArc(new MazePoint(cx, cy), radius, startAngleDeg, sweepAngleDeg, MazeColor.Black, 1);
                }
            });
        }

        private void IterateBorders(MazeField field, Action<IMazeBorder> onBorder)
        {
            var graph = field.Graph;
            for (int v = 0; v < graph.Count; v++)
            {
                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor != -1 && edge.Neighbor <= v)
                        continue;

                    if (edge.Neighbor == -1 && v == gate.Entrance && edge.Border == gate.EntranceBorder)
                        continue;

                    if (edge.Neighbor == -1 && v == gate.Exit && edge.Border == gate.ExitBorder)
                        continue;

                    if (!edge.IsOpen && edge.Border != null)
                        onBorder(edge.Border);
                }
            }
        }
    }
}
