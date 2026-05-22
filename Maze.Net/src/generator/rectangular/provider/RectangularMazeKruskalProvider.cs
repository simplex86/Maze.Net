using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于Kruskal最小生成树算法生成随机迷宫
    /// </summary>
    public class RectangularMazeKruskalProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 墙及其两侧的路径格子
        /// </summary>
        private struct Edge
        {
            public int x1, y1; // 第一个路径格子坐标
            public int x2, y2; // 第二个路径格子坐标

            public Edge(int x1, int y1, int x2, int y2)
            {
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
            }
        }

        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Kruskal;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);

            var edges = CollectEdges(width, height);
            edges.Shuffle(random);

            var dsu = new DisjointSet(width * height);
            foreach (var edge in edges)
            {
                // 获取墙两侧的路径格子在并查集中的索引
                int a = GetCellIndex(edge.x1, edge.y1, width);
                int b = GetCellIndex(edge.x2, edge.y2, width);

                // 如果两个格子不在同一连通分量，则打通这堵墙
                if (dsu.Union(a, b))
                {
                    field.RemoveWallBetween(edge.x1, edge.y1, edge.x2, edge.y2);
                }

                // 所有格子已连通时提前退出
                if (dsu.Count == 1) break;
            }

            return field;
        }

        /// <summary>
        /// 收集所有边（相邻格子之间的连接）
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>边的列表</returns>
        private List<Edge> CollectEdges(int width, int height)
        {
            var edges = new List<Edge>();

            // 收集水平边（上下相邻的格子）
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    edges.Add(new Edge(x, y, x, y + 1));
                }
            }

            // 收集垂直边（左右相邻的格子）
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    edges.Add(new Edge(x, y, x + 1, y));
                }
            }

            return edges;
        }

        /// <summary>
        /// 将路径格子坐标转换为并查集索引
        /// </summary>
        /// <param name="x">路径格子X坐标</param>
        /// <param name="y">路径格子Y坐标</param>
        /// <param name="width">迷宫宽度</param>
        /// <returns>并查集索引</returns>
        private int GetCellIndex(int x, int y, int width)
        {
            return x + y * width;
        }
    }
}