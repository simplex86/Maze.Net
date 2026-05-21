using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于PRIM算法
    /// </summary>
    public class RectangularMazePrimProvider : IRectangularMazeProvider
    {
        // 随机数
        private Random random = new Random();
        // 开链表
        private List<Edge> openlist = new List<Edge>();

        private struct Edge
        {
            public int x, y; // 当前格子
            public int nx, ny; // 邻居格子

            public Edge(int x, int y, int nx, int ny)
            {
                this.x = x;
                this.y = y;
                this.nx = nx;
                this.ny = ny;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Prim;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <returns></returns>
        public RectangularMazeField Create(int width, int height)
        {
            openlist.Clear();

            var field = new RectangularMazeField(width, height);

            var visited = new bool[height][];
            for (int i = 0; i < height; i++)
            {
                visited[i] = new bool[width];
            }

            // 随机起点
            var x = random.Next(width);
            var y = random.Next(height);
            visited[y][x] = true;

            // 从起点开始探索
            SearchNeighbours(x, y, width, height, visited);

            while (openlist.Count > 0)
            {
                var idx = random.Next(0, openlist.Count);
                var cur = openlist[idx];
                openlist.RemoveAt(idx);

                if (!visited[cur.ny][cur.nx])
                {
                    // 打通墙
                    field.RemoveWallBetween(cur.x, cur.y, cur.nx, cur.ny);
                    visited[cur.ny][cur.nx] = true;
                    SearchNeighbours(cur.nx, cur.ny, width, height, visited);
                }
            }

            return field;
        }

        /// <summary>
        /// 获取邻居
        /// </summary>
        private void SearchNeighbours(int x, int y, int width, int height, bool[][] visited)
        {
            // 上
            if (y > 0 && !visited[y - 1][x]) openlist.Add(new Edge(x, y, x, y - 1));
            // 下
            if (y < height - 1 && !visited[y + 1][x]) openlist.Add(new Edge(x, y, x, y + 1));
            // 左
            if (x > 0 && !visited[y][x - 1]) openlist.Add(new Edge(x, y, x - 1, y));
            // 右
            if (x < width - 1 && !visited[y][x + 1]) openlist.Add(new Edge(x, y, x + 1, y));
        }
    }
}