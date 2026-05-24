using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于广度优先搜索算法生成随机迷宫
    /// 特点：生成的迷宫具有较短的分支，相对均匀的分布
    /// </summary>
    public class RectangularMazeBfsProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.BFS;

        // 用于临时存储邻居信息的结构体
        private struct NeighborInfo
        {
            public int x;
            public int y;
            public int parentX;
            public int parentY;

            public NeighborInfo(int x, int y, int parentX, int parentY)
            {
                this.x = x;
                this.y = y;
                this.parentX = parentX;
                this.parentY = parentY;
            }
        }

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);

            // 随机选择起点
            int x = random.Next(width);
            int y = random.Next(height);

            // 使用队列实现广度优先搜索
            var currentLevel = new Queue<Tile>();
            currentLevel.Enqueue(new Tile(x, y));

            var visited = new bool[height][];
            for (int i = 0; i < height; i++)
            {
                visited[i] = new bool[width];
            }
            visited[y][x] = true;

            while (currentLevel.Count > 0)
            {
                // 收集当前层的所有邻居
                var nextLevel = new List<NeighborInfo>();

                foreach (var tile in currentLevel)
                {
                    var neighbors = GetUnvisitedNeighbors(field, visited, tile.lateral, tile.radial);
                    nextLevel.AddRange(neighbors);
                }

                // 随机打乱下一层的顺序，增加迷宫的随机性
                nextLevel.Shuffle(random);

                // 处理下一层
                var newCurrentLevel = new Queue<Tile>();
                foreach (var neighbor in nextLevel)
                {
                    if (!visited[neighbor.y][neighbor.x])
                    {
                        // 打通与父格子之间的墙
                        field.RemoveWallBetween(neighbor.x, neighbor.y, neighbor.parentX, neighbor.parentY);

                        // 标记邻居为已访问
                        visited[neighbor.y][neighbor.x] = true;

                        // 加入新的当前层
                        newCurrentLevel.Enqueue(new Tile(neighbor.x, neighbor.y));
                    }
                }

                // 更新当前层为新的一层
                currentLevel = newCurrentLevel;
            }

            return field;
        }

        /// <summary>
        /// 获取未访问的邻居列表
        /// </summary>
        private List<NeighborInfo> GetUnvisitedNeighbors(RectangularMazeField field, bool[][] visited, int x, int y)
        {
            var neighbors = new List<NeighborInfo>();

            // 上
            if (y > 0 && !visited[y - 1][x])
            {
                neighbors.Add(new NeighborInfo(x, y - 1, x, y));
            }
            // 下
            if (y < field.height - 1 && !visited[y + 1][x])
            {
                neighbors.Add(new NeighborInfo(x, y + 1, x, y));
            }
            // 左
            if (x > 0 && !visited[y][x - 1])
            {
                neighbors.Add(new NeighborInfo(x - 1, y, x, y));
            }
            // 右
            if (x < field.width - 1 && !visited[y][x + 1])
            {
                neighbors.Add(new NeighborInfo(x + 1, y, x, y));
            }

            return neighbors;
        }
    }
}
