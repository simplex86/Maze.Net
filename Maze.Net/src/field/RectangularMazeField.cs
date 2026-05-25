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

        public RectangularMazeField(int width, int height)
        {
            this.width = Math.Max(1, width);
            this.height = Math.Max(1, height);
            count = width * height;
            graph = BuildGraph();
        }

        private List<List<Edge>> BuildGraph()
        {
            var g = new List<List<Edge>>(count);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var edges = new List<Edge>();

                    // 右邻居
                    if (x < width - 1)
                    {
                        int neighbor = y * width + (x + 1);
                        edges.Add(new Edge(neighbor, new LineBorder(x + 1, y, x + 1, y + 1)));
                    }
                    // 左邻居
                    if (x > 0)
                    {
                        int neighbor = y * width + (x - 1);
                        edges.Add(new Edge(neighbor, new LineBorder(x, y, x, y + 1)));
                    }
                    // 下邻居
                    if (y < height - 1)
                    {
                        int neighbor = (y + 1) * width + x;
                        edges.Add(new Edge(neighbor, new LineBorder(x, y + 1, x + 1, y + 1)));
                    }
                    // 上邻居
                    if (y > 0)
                    {
                        int neighbor = (y - 1) * width + x;
                        edges.Add(new Edge(neighbor, new LineBorder(x, y, x + 1, y)));
                    }

                    // 边界边
                    if (x == 0)
                        edges.Add(new Edge(-1, new LineBorder(0, y, 0, y + 1)));
                    if (x == width - 1)
                        edges.Add(new Edge(-1, new LineBorder(width, y, width, y + 1)));
                    if (y == 0)
                        edges.Add(new Edge(-1, new LineBorder(x, 0, x + 1, 0)));
                    if (y == height - 1)
                        edges.Add(new Edge(-1, new LineBorder(x, height, x + 1, height)));

                    g.Add(edges);
                }
            }

            return g;
        }
    }
}
