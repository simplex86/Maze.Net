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
            width = Utils.Odd(width);
            height = Utils.Odd(height);

            var field = new RectangularMazeField(width, height);

            // 计算路径格子数量（奇数坐标）
            int cols = width / 2;
            int rows = height / 2;
            int totalCells = cols * rows;

            // 标记单元格是否已访问
            bool[,] visited = new bool[cols, rows];

            // 方向数组：上、下、左、右
            // 每个方向包含：dx, dy, wallOffsetX, wallOffsetY
            var directions = new List<(int dx, int dy, int wallX, int wallY)>
            {
                (0, -1, 0, -1),   // 上：墙在当前格子上方
                (0, 1, 0, 1),     // 下：墙在当前格子下方
                (-1, 0, -1, 0),   // 左：墙在当前格子左侧
                (1, 0, 1, 0)      // 右：墙在当前格子右侧
            };

            // 随机选择起点（在路径格子坐标系统中）
            int currentCol = random.Next(cols);
            int currentRow = random.Next(rows);
            visited[currentCol, currentRow] = true;

            // 统计已访问的单元格数量
            int visitedCount = 1;

            // 开始随机游走
            while (visitedCount < totalCells)
            {
                // 随机选择一个方向
                int dirIdx = random.Next(4);
                var dir = directions[dirIdx];

                // 计算新位置
                int newCol = currentCol + dir.dx;
                int newRow = currentRow + dir.dy;

                // 检查新位置是否在边界内
                if (newCol >= 0 && newCol < cols && newRow >= 0 && newRow < rows)
                {
                    // 如果新位置未访问
                    if (!visited[newCol, newRow])
                    {
                        // 标记新位置为已访问
                        visited[newCol, newRow] = true;
                        visitedCount++;

                        // 计算路径格子坐标（奇数坐标）
                        int currentPathX = currentCol * 2 + 1;
                        int currentPathY = currentRow * 2 + 1;
                        int newPathX = newCol * 2 + 1;
                        int newPathY = newRow * 2 + 1;

                        // 标记路径格子
                        field[currentPathX, currentPathY] = TileType.Path;
                        field[newPathX, newPathY] = TileType.Path;

                        // 计算墙的位置并打通
                        int wallX = currentPathX + dir.wallX;
                        int wallY = currentPathY + dir.wallY;
                        field[wallX, wallY] = TileType.Path;
                    }

                    // 移动到新位置（无论是否已访问）
                    currentCol = newCol;
                    currentRow = newRow;
                }
            }

            return field;
        }
    }
}