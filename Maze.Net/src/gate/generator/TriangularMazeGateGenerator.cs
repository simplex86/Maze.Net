using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class TriangularMazeGateGenerator : MazeGateGenerator<TriangularMazeField>
    {
        public TriangularMazeGateGenerator()
        {

        }

        public TriangularMazeGateGenerator(Random random)
            : base(random)
        {

        }

        public override MazeGate Generate(TriangularMazeField field)
        {
            var order = field.Order;
            var mid = (order + 1) / 2;

            // 将三角形的三条边各分成两段，共6段（顺时针从左下角开始）：
            // 0: 底边左半, 1: 底边右半
            // 2: 右边下半, 3: 右边上半
            // 4: 左边上半, 5: 左边下半
            var segments = new List<int>[6];
            for (int i = 0; i < 6; i++) segments[i] = new List<int>();

            // 底边：row = order-1, col 为偶数
            for (int c = 0; c < 2 * order - 1; c += 2)
            {
                var v = VertexIndex(order - 1, c);
                if (c / 2 < mid)
                    segments[0].Add(v);
                else
                    segments[1].Add(v);
            }

            // 右边：col = 2*row
            for (int r = 0; r < order; r++)
            {
                var v = VertexIndex(r, 2 * r);
                if (r >= mid)
                    segments[2].Add(v);
                else
                    segments[3].Add(v);
            }

            // 左边：col = 0
            for (int r = 0; r < order; r++)
            {
                var v = VertexIndex(r, 0);
                if (r < mid)
                    segments[4].Add(v);
                else
                    segments[5].Add(v);
            }

            // 收集所有不相邻的段对
            var validPairs = new List<(int, int)>();
            for (int i = 0; i < 6; i++)
                for (int j = i + 1; j < 6; j++)
                    if (!IsAdjacent(i, j) && segments[i].Count > 0 && segments[j].Count > 0)
                        validPairs.Add((i, j));

            int entrance, exit;
            do
            {
                var pair = validPairs[random.Next(validPairs.Count)];
                var es = pair.Item1;
                var xs = pair.Item2;

                if (random.Next(2) == 0)
                    (es, xs) = (xs, es);

                entrance = segments[es][random.Next(segments[es].Count)];
                exit = segments[xs][random.Next(segments[xs].Count)];
            } while (entrance == exit);

            return new MazeGate(entrance, exit)
            {
                EntranceBorder = PickOuterBorder(field, entrance),
                ExitBorder = PickOuterBorder(field, exit)
            };
        }

        /// <summary>
        /// 判断两段是否相邻（在周长环上相邻）
        /// </summary>
        private static bool IsAdjacent(int a, int b)
        {
            var diff = Math.Abs(a - b);
            return diff == 1 || diff == 5;
        }

        private static int VertexIndex(int row, int col)
        {
            return row * row + col;
        }
    }
}
