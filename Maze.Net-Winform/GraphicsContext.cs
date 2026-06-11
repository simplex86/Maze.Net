using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class GraphicsContext : IGraphicsContext
    {
        private readonly Graphics _graphics;

        public GraphicsContext(Graphics graphics)
        {
            _graphics = graphics;
        }

        public void DrawLine(MazePoint a, MazePoint b, MazeColor color, double width)
        {
            using var pen = new Pen(ToColor(color), (float)width);
            _graphics.DrawLine(pen, new PointF(a.X, a.Y), new PointF(b.X, b.Y));
        }

        public void DrawArc(MazePoint center, double radius, double startAngleDeg, double sweepAngleDeg, MazeColor color, double width)
        {
            using var pen = new Pen(ToColor(color), (float)width);
            var rect = new RectangleF((float)(center.X - radius), (float)(center.Y - radius),
                                       (float)(radius * 2), (float)(radius * 2));
            _graphics.DrawArc(pen, rect, (float)startAngleDeg, (float)sweepAngleDeg);
        }

        public void FillRectangle(MazePoint pt, MazeSize size, MazeColor color)
        {
            using var brush = new SolidBrush(ToColor(color));
            _graphics.FillRectangle(brush, pt.X, pt.Y, size.Width, size.Height);
        }

        public void FillPolygon(List<MazePoint> points, MazeColor color)
        {
            using var brush = new SolidBrush(ToColor(color));
            var pts = new PointF[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                pts[i] = new PointF(points[i].X, points[i].Y);
            }
            _graphics.FillPolygon(brush, pts);
        }

        public void FillAnnularSector(MazePoint center, double outerRadius, double innerRadius, double startAngleDeg, double sweepAngleDeg, MazeColor color)
        {
            using var brush = new SolidBrush(ToColor(color));

            if (innerRadius <= 0)
            {
                _graphics.FillPie(brush,
                    (float)(center.X - outerRadius), (float)(center.Y - outerRadius),
                    (float)(outerRadius * 2), (float)(outerRadius * 2),
                    (float)startAngleDeg, (float)sweepAngleDeg);
            }
            else
            {
                using var path = new GraphicsPath();
                path.AddArc((float)(center.X - outerRadius), (float)(center.Y - outerRadius),
                    (float)(outerRadius * 2), (float)(outerRadius * 2),
                    (float)startAngleDeg, (float)sweepAngleDeg);
                path.AddArc((float)(center.X - innerRadius), (float)(center.Y - innerRadius),
                    (float)(innerRadius * 2), (float)(innerRadius * 2),
                    (float)(startAngleDeg + sweepAngleDeg), (float)(-sweepAngleDeg));
                path.CloseFigure();
                _graphics.FillPath(brush, path);
            }
        }

        public void FillArcWedge(MazePoint center, double arcRadius, double startAngleDeg, double sweepAngleDeg, MazePoint closingPoint, MazeColor color)
        {
            using var brush = new SolidBrush(ToColor(color));
            using var path = new GraphicsPath();
            path.AddArc((float)(center.X - arcRadius), (float)(center.Y - arcRadius),
                (float)(arcRadius * 2), (float)(arcRadius * 2),
                (float)startAngleDeg, (float)sweepAngleDeg);
            path.AddLine(new PointF(closingPoint.X, closingPoint.Y), new PointF(closingPoint.X, closingPoint.Y));
            path.CloseFigure();
            _graphics.FillPath(brush, path);
        }

        private static Color ToColor(MazeColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
