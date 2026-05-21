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
        public RectangularField Create(int width, int height)
        {
            width = Utils.Odd(width);
            height = Utils.Odd(height);

            var field = new RectangularField(width, height);

            // 随机选择起点（必须是奇数坐标）
            int x = random.Next(1, width / 2) * 2 + 1;
            int y = random.Next(1, height / 2) * 2 + 1;
            field[x, y] = TileType.Path;

            // 使用队列实现广度优先搜索
            var currentLevel = new Queue<RectangularTile>();
            currentLevel.Enqueue(new RectangularTile(x, y));

            var visited = new bool[width, height];
            visited[x, y] = true;

            while (currentLevel.Count > 0)
            {
                // 收集当前层的所有邻居
                var nextLevel = new List<NeighborInfo>();

                foreach (var tile in currentLevel)
                {
                    var neighbors = GetUnvisitedNeighbors(field, visited, tile.x, tile.y);
                    nextLevel.AddRange(neighbors);
                }

                // 随机打乱下一层的顺序，增加迷宫的随机性
                Shuffle(nextLevel);

                // 处理下一层
                var newCurrentLevel = new Queue<RectangularTile>();
                foreach (var neighbor in nextLevel)
                {
                    if (!visited[neighbor.x, neighbor.y])
                    {
                        // 打通中间的墙
                        int midX = (neighbor.x + neighbor.parentX) / 2;
                        int midY = (neighbor.y + neighbor.parentY) / 2;
                        field[midX, midY] = TileType.Path;

                        // 标记邻居为路径
                        field[neighbor.x, neighbor.y] = TileType.Path;
                        visited[neighbor.x, neighbor.y] = true;

                        // 加入新的当前层
                        newCurrentLevel.Enqueue(new RectangularTile(neighbor.x, neighbor.y));
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
        private List<NeighborInfo> GetUnvisitedNeighbors(RectangularField field, bool[,] visited, int x, int y)
        {
            var neighbors = new List<NeighborInfo>();

            // 上（隔一格）
            if (y - 2 >= 1 && !visited[x, y - 2] && Utils.IsWall(field, x, y - 2))
            {
                neighbors.Add(new NeighborInfo(x, y - 2, x, y));
            }
            // 下（隔一格）
            if (y + 2 < field.height - 1 && !visited[x, y + 2] && Utils.IsWall(field, x, y + 2))
            {
                neighbors.Add(new NeighborInfo(x, y + 2, x, y));
            }
            // 左（隔一格）
            if (x - 2 >= 1 && !visited[x - 2, y] && Utils.IsWall(field, x - 2, y))
            {
                neighbors.Add(new NeighborInfo(x - 2, y, x, y));
            }
            // 右（隔一格）
            if (x + 2 < field.width - 1 && !visited[x + 2, y] && Utils.IsWall(field, x + 2, y))
            {
                neighbors.Add(new NeighborInfo(x + 2, y, x, y));
            }

            return neighbors;
        }

        /// <summary>
        /// Fisher-Yates 洗牌算法
        /// </summary>
        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
