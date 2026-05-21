using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于深度优先搜索算法生成随机迷宫
    /// </summary>
    public class RectangularMazeDfsProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.DFS;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度（格子数量）</param>
        /// <param name="height">迷宫高度（格子数量）</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height)
        {
            // 创建新的迷宫场地
            var field = new RectangularMazeField(width, height);
            
            // 访问标记数组
            var visited = new bool[height][];
            for (int y = 0; y < height; y++)
            {
                visited[y] = new bool[width];
            }

            // 随机选择起点
            int startX = random.Next(width);
            int startY = random.Next(height);
            visited[startY][startX] = true;

            // 使用栈实现深度优先搜索
            var stack = new Stack<Tile>();
            stack.Push(new Tile(startX, startY));

            while (stack.Count > 0)
            {
                // 获取当前位置
                var current = stack.Peek();
                int cx = current.lateral;
                int cy = current.radial;

                // 获取未访问的邻居列表
                var neighbors = GetUnvisitedNeighbors(field, visited, cx, cy);

                if (neighbors.Count > 0)
                {
                    // 随机选择一个邻居
                    int idx = random.Next(neighbors.Count);
                    var neighbor = neighbors[idx];
                    int nx = neighbor.lateral;
                    int ny = neighbor.radial;

                    // 打通当前格子和邻居之间的墙
                    field.RemoveWallBetween(cx, cy, nx, ny);

                    // 标记邻居为已访问
                    visited[ny][nx] = true;
                    stack.Push(new Tile(nx, ny));
                }
                else
                {
                    // 回溯：没有未访问邻居，弹出当前位置
                    stack.Pop();
                }
            }

            return field;
        }

        /// <summary>
        /// 获取未访问的邻居列表
        /// </summary>
        private List<Tile> GetUnvisitedNeighbors(RectangularMazeField field, bool[][] visited, int x, int y)
        {
            var neighbors = new List<Tile>();

            // 上
            if (y > 0 && !visited[y - 1][x])
            {
                neighbors.Add(new Tile(x, y - 1));
            }
            // 下
            if (y < field.height - 1 && !visited[y + 1][x])
            {
                neighbors.Add(new Tile(x, y + 1));
            }
            // 左
            if (x > 0 && !visited[y][x - 1])
            {
                neighbors.Add(new Tile(x - 1, y));
            }
            // 右
            if (x < field.width - 1 && !visited[y][x + 1])
            {
                neighbors.Add(new Tile(x + 1, y));
            }

            return neighbors;
        }
    }
}
