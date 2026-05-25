using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class TriangularMazeRenderer : MazeRenderer<TriangularMazeField>
    {
        protected override void DrawField(Graphics grap, TriangularMazeField field)
        {
            if (field.order == 0 || thickness <= 0 || width <= 0 || height <= 0)
                return;

            double mazeWidth = field.order;
            double mazeHeight = field.order * Math.Sqrt(3) / 2;

            float cx = (float)((width - mazeWidth * thickness) / 2.0) + offsetx;
            float cy = (float)((height - mazeHeight * thickness) / 2.0) + offsety;

            var pen = new Pen(Color.Black);

            IterateBorders(field, border =>
            {
                if (border is LineBorder line)
                {
                    float x1 = cx + (float)(line.X1 * thickness);
                    float y1 = cy + (float)(line.Y1 * thickness);
                    float x2 = cx + (float)(line.X2 * thickness);
                    float y2 = cy + (float)(line.Y2 * thickness);
                    grap.DrawLine(pen, x1, y1, x2, y2);
                }
            });

            pen.Dispose();
        }
    }
}
