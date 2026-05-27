using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class CircularHexagonMazeField : HexagonalMazeField
    {
        public CircularHexagonMazeField(int size) : base(size) { }

        /// <summary>
        /// 出入口在直径上：对径扇区(s 和 s+3)，同列号保证对径
        /// </summary>
        public override MazeGate GenerateOppositeEdgeGate(Random random)
        {
            var entranceSector = random.Next(6);
            var exitSector = (entranceSector + 3) % 6;

            // 外环顶点行号 = size-1，必须选同列才能保证对径
            var col = random.Next(size);
            var entrance = VertexIndex(entranceSector, 0, size - 1, col);
            var exit = VertexIndex(exitSector, 0, size - 1, col);

            // 随机交换入口/出口
            return (random.Next(2) == 0) ? new MazeGate(exit, entrance)
                                         : new MazeGate(entrance, exit);
        }

        /// <summary>
        /// 获取顶点所在格子的扇区形状参数
        /// </summary>
        public CurvedTriangle GetVertexSectorShape(int vertex)
        {
            var sectorSize = size * size;
            var sector = vertex / sectorSize;
            var remaining = vertex % sectorSize;
            var updownSize = size * (size + 1) / 2;
            var updown = remaining < updownSize ? 0 : 1;
            var idx = updown == 0 ? remaining : remaining - updownSize;

            var row = 0;
            while ((row + 1) * (row + 2) / 2 <= idx)
                row++;
            var column = idx - row * (row + 1) / 2;

            var sectorStart = (sector - 2) * Math.PI / 3;

            if (updown == 0)
            {
                var innerAngle = row > 0 ? sectorStart + column * Math.PI / 3 / row : 0;
                var arcStartAngle = sectorStart + column * Math.PI / 3 / (row + 1);
                var arcSweepAngle = Math.PI / 3 / (row + 1);
                return new CurvedTriangle(true, row, innerAngle, row + 1, arcStartAngle, arcSweepAngle, 0, 0);
            }
            else
            {
                var arcStartAngle = sectorStart + column * Math.PI / 3 / (row + 1);
                var arcSweepAngle = Math.PI / 3 / (row + 1);
                var outerAngle = sectorStart + (column + 1) * Math.PI / 3 / (row + 2);
                return new CurvedTriangle(false, 0, 0, row + 1, arcStartAngle, arcSweepAngle, row + 2, outerAngle);
            }
        }

        protected override IMazeBorder GetEdge(int sector, int row, int column, int edge)
        {
            if (edge == 0)
            {
                var startAngle = (sector - 2) * Math.PI / 3 + column * Math.PI / 3 / (row + 1);
                var sweepAngle = Math.PI / 3 / (row + 1);
                return new ArcBorder(0, 0, row + 1, startAngle, sweepAngle);
            }

            double ex1, ey1, ex2, ey2;
            if (edge == 1)
            {
                var theta1 = (sector - 2) * Math.PI / 3;
                var theta2 = (sector - 2) * Math.PI / 3;
                if (row > 0) theta1 += column * Math.PI / 3 / row;
                theta2 += (column + 1) * Math.PI / 3 / (row + 1);

                ex1 = row * Math.Cos(theta1);
                ey1 = row * Math.Sin(theta1);
                ex2 = (row + 1) * Math.Cos(theta2);
                ey2 = (row + 1) * Math.Sin(theta2);
            }
            else
            {
                var theta1 = (sector - 2) * Math.PI / 3;
                var theta2 = (sector - 2) * Math.PI / 3;
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
