using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫场地（邻接表方案）
    /// </summary>
    public class RectangularMazeField : MazeField
    {
        public int width { get; }
        public int height { get; }

        /// <summary>
        /// 出入口在对边：上↔下 或 左↔右
        /// </summary>
        public override MazeGate GenerateOppositeEdgeGate(Random random)
        {
            // 4条边：0=上, 1=下, 2=左, 3=右
            var sides = new List<int>[4];
            for (int i = 0; i < 4; i++) sides[i] = new List<int>();

            for (int x = 0; x < width; x++)
            {
                sides[0].Add(x);                         // 上边 y=0
                sides[1].Add((height - 1) * width + x);  // 下边 y=height-1
            }
            for (int y = 0; y < height; y++)
            {
                sides[2].Add(y * width);                  // 左边 x=0
                sides[3].Add(y * width + width - 1);      // 右边 x=width-1
            }

            // 随机选对边组：0=上下, 1=左右
            var pair = random.Next(2);
            var entranceSide = pair * 2;
            var exitSide = pair * 2 + 1;

            // 随机交换入口/出口所在边
            if (random.Next(2) == 0)
                (entranceSide, exitSide) = (exitSide, entranceSide);

            var entrance = sides[entranceSide][random.Next(sides[entranceSide].Count)];
            var exit = sides[exitSide][random.Next(sides[exitSide].Count)];
            return new MazeGate(entrance, exit);
        }

        public RectangularMazeField(int width, int height)
        {
            this.width = Math.Max(1, width);
            this.height = Math.Max(1, height);
            Count = width * height;
            Graph = BuildGraph();
        }

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(Count);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var edges = new List<Adjacency>();

                    // 右邻居
                    if (x < width - 1)
                    {
                        var neighbor = y * width + (x + 1);
                        edges.Add(new Adjacency(neighbor, new LineBorder(x + 1, y, x + 1, y + 1)));
                    }
                    // 左邻居
                    if (x > 0)
                    {
                        var neighbor = y * width + (x - 1);
                        edges.Add(new Adjacency(neighbor, new LineBorder(x, y, x, y + 1)));
                    }
                    // 下邻居
                    if (y < height - 1)
                    {
                        var neighbor = (y + 1) * width + x;
                        edges.Add(new Adjacency(neighbor, new LineBorder(x, y + 1, x + 1, y + 1)));
                    }
                    // 上邻居
                    if (y > 0)
                    {
                        var neighbor = (y - 1) * width + x;
                        edges.Add(new Adjacency(neighbor, new LineBorder(x, y, x + 1, y)));
                    }

                    // 边界边
                    if (x == 0)          edges.Add(new Adjacency(-1, new LineBorder(0, y, 0, y + 1)));
                    if (x == width - 1)  edges.Add(new Adjacency(-1, new LineBorder(width, y, width, y + 1)));
                    if (y == 0)          edges.Add(new Adjacency(-1, new LineBorder(x, 0, x + 1, 0)));
                    if (y == height - 1) edges.Add(new Adjacency(-1, new LineBorder(x, height, x + 1, height)));

                    g.Add(edges);
                }
            }

            return g;
        }
    }
}
