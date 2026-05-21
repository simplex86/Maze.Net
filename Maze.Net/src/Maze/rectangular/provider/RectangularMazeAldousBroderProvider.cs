using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于Aldous-Broder算法生成随机迷宫
    /// Aldous-Broder算法特点：最简单的随机迷宫算法，使用纯粹的随机游走
    /// </summary>
    public class RectangularMazeAldousBroderProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.AldousBroder;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);

            int totalCells = width * height;

            // 标记单元格是否已访问
            bool[][] visited = new bool[height][];
            for (int i = 0; i < height; i++)
            {
                visited[i] = new bool[width];
            }

            // 方向数组：上、下、左、右
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };

            // 随机选择起点
            int currentX = random.Next(width);
            int currentY = random.Next(height);
            visited[currentY][currentX] = true;

            // 统计已访问的单元格数量
            int visitedCount = 1;

            // 开始随机游走
            while (visitedCount < totalCells)
            {
                // 随机选择一个方向
                int dirIdx = random.Next(4);

                // 计算新位置
                int newX = currentX + dx[dirIdx];
                int newY = currentY + dy[dirIdx];

                // 检查新位置是否在边界内
                if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                {
                    // 如果新位置未访问
                    if (!visited[newY][newX])
                    {
                        // 标记新位置为已访问
                        visited[newY][newX] = true;
                        visitedCount++;

                        // 打通墙
                        field.RemoveWallBetween(currentX, currentY, newX, newY);
                    }

                    // 移动到新位置（无论是否已访问）
                    currentX = newX;
                    currentY = newY;
                }
            }

            return field;
        }
    }
}