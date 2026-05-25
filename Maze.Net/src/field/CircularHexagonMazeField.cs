using System;

namespace SimplexLab.Maze
{
    public class CircularHexagonMazeField : HexagonalMazeField
    {
        public CircularHexagonMazeField(int size) : base(size) { }

        protected override IMazeBorder GetEdge(int sector, int row, int column, int edge)
        {
            if (edge == 0)
            {
                double startAngle = (sector - 2) * Math.PI / 3 + column * Math.PI / 3 / (row + 1);
                double sweepAngle = Math.PI / 3 / (row + 1);
                return new ArcBorder(0, 0, row + 1, startAngle, sweepAngle);
            }

            double ex1, ey1, ex2, ey2;
            if (edge == 1)
            {
                double theta1 = (sector - 2) * Math.PI / 3;
                double theta2 = (sector - 2) * Math.PI / 3;
                if (row > 0) theta1 += column * Math.PI / 3 / row;
                theta2 += (column + 1) * Math.PI / 3 / (row + 1);

                ex1 = row * Math.Cos(theta1);
                ey1 = row * Math.Sin(theta1);
                ex2 = (row + 1) * Math.Cos(theta2);
                ey2 = (row + 1) * Math.Sin(theta2);
            }
            else
            {
                double theta1 = (sector - 2) * Math.PI / 3;
                double theta2 = (sector - 2) * Math.PI / 3;
                if (row > 0) theta1 += column * Math.PI / 3 / row;
                theta2 += column * Math.PI / 3 / (row + 1);

                ex1 = row * Math.Cos(theta1);
                ey1 = row * Math.Sin(theta1);
                ex2 = (row + 1) * Math.Cos(theta2);
                ey2 = (row + 1) * Math.Sin(theta2);
            }

            return new LineBorder(ex1, ey1, ex2, ey2);
        }
    }
}
