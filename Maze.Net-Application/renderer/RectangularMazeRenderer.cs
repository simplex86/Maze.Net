using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class RectangularMazeRenderer : MazeRenderer<RectangularMazeField>
    {
        protected override void DrawField(Graphics grap, RectangularMazeField field)
        {
            if (field.width == 0 || field.height == 0)
                return;

            int mazeWidth = field.width * thickness;
            int mazeHeight = field.height * thickness;
            int cx = (width - mazeWidth) / 2 + offsetx;
            int cy = (height - mazeHeight) / 2 + offsety;

            var pen = new Pen(Color.Black);

            IterateBorders(field, border =>
            {
                if (border is LineBorder line)
                {
                    int x1 = cx + (int)(line.X1 * thickness);
                    int y1 = cy + (int)(line.Y1 * thickness);
                    int x2 = cx + (int)(line.X2 * thickness);
                    int y2 = cy + (int)(line.Y2 * thickness);
                    grap.DrawLine(pen, x1, y1, x2, y2);
                }
            });

            pen.Dispose();
        }
    }
}
