using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class CircularMazeRenderer : MazeRenderer<CircularMazeField>
    {
        protected override void DrawField(Graphics grap, CircularMazeField field)
        {
            if (field.rings == 0 || thickness <= 0 || width <= 0 || height <= 0)
                return;

            float centerX = width / 2.0f + offsetx;
            float centerY = height / 2.0f + offsety;

            var pen = new Pen(Color.Black);

            IterateBorders(field, border =>
            {
                if (border is LineBorder line)
                {
                    float x1 = centerX + (float)(line.X1 * thickness);
                    float y1 = centerY + (float)(line.Y1 * thickness);
                    float x2 = centerX + (float)(line.X2 * thickness);
                    float y2 = centerY + (float)(line.Y2 * thickness);
                    grap.DrawLine(pen, x1, y1, x2, y2);
                }
                else if (border is ArcBorder arc)
                {
                    float cx = centerX + (float)(arc.CenterX * thickness);
                    float cy = centerY + (float)(arc.CenterY * thickness);
                    float radius = (float)(arc.Radius * thickness);
                    if (radius <= 0) return;

                    float startAngleDeg = (float)(arc.StartAngle * 180 / Math.PI);
                    float sweepAngleDeg = (float)(arc.SweepAngle * 180 / Math.PI);
                    grap.DrawArc(pen, cx - radius, cy - radius, radius * 2, radius * 2, startAngleDeg, sweepAngleDeg);
                }
            });

            pen.Dispose();
        }
    }
}
