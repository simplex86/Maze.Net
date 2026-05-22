using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于Wilson算法生成随机迷宫
    /// Wilson算法特点：使用随机游走，生成的迷宫具有均匀的随机性
    /// </summary>
    public class RectangularMazeWilsonProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Wilson;

        // 方向数组：上、下、左、右
        private int[] dx = { 0, 0, -1, 1 };
        private int[] dy = { -1, 1, 0, 0 };

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);

            // 标记单元格是否已访问
            bool[][] visited = new bool[height][];
            for (int i = 0; i < height; i++)
            {
                visited[i] = new bool[width];
            }

            // 随机选择起点
            int startX = random.Next(width);
            int startY = random.Next(height);
            visited[startY][startX] = true;

            // 统计已访问的单元格数量
            int visitedCount = 1;

            // 继续直到所有单元格都被访问
            while (visitedCount < width * height)
            {
                // 随机选择一个未访问的单元格开始
                int x, y;
                do
                {
                    x = random.Next(width);
                    y = random.Next(height);
                } while (visited[y][x]);

                // 开始随机游走，记录路径
                List<(int, int)> path = new List<(int, int)>();
                path.Add((x, y));

                // 记录每个位置在路径中的索引，用于检测环路
                int[][] pathIndex = new int[height][];
                for (int i = 0; i < height; i++)
                {
                    pathIndex[i] = new int[width];
                    for (int j = 0; j < width; j++)
                    {
                        pathIndex[i][j] = -1;
                    }
                }
                pathIndex[y][x] = 0;

                while (true)
                {
                    // 随机选择一个方向
                    int dirIdx = random.Next(4);
                    int nx = x + dx[dirIdx];
                    int ny = y + dy[dirIdx];

                    // 确保在边界内
                    if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;

                    // 检查是否撞到已访问的单元格（成功！）
                    if (visited[ny][nx])
                    {
                        // 添加当前位置到路径
                        path.Add((nx, ny));

                        // 将整个路径加入迷宫
                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            var (px, py) = path[i];
                            var (p2x, p2y) = path[i + 1];
                            field.RemoveWallBetween(px, py, p2x, p2y);
                            if (!visited[py][px])
                            {
                                visited[py][px] = true;
                                visitedCount++;
                            }
                        }
                        if (!visited[ny][nx])
                        {
                            visited[ny][nx] = true;
                            visitedCount++;
                        }

                        break;
                    }

                    // 检查是否在当前路径中（环路！）
                    int idx = pathIndex[ny][nx];
                    if (idx != -1)
                    {
                        // 截断路径到idx位置
                        while (path.Count > idx + 1)
                        {
                            var (rx, ry) = path[path.Count - 1];
                            pathIndex[ry][rx] = -1;
                            path.RemoveAt(path.Count - 1);
                        }
                        x = nx;
                        y = ny;
                        continue;
                    }

                    // 继续游走
                    path.Add((nx, ny));
                    pathIndex[ny][nx] = path.Count - 1;
                    x = nx;
                    y = ny;
                }
            }

            return field;
        }
    }
}
