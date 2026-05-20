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

            // 计算路径格子数量
            int cols = width / 2;
            int rows = height / 2;
            int totalCells = cols * rows;

            // 标记单元格是否已访问
            bool[,] visited = new bool[cols, rows];
            // 记录随机游走路径的方向（用于回退）
            int[,] pathDir = new int[cols, rows];  // 0:无, 1:上, 2:下, 4:左, 8:右

            // 随机选择一个起点并标记为已访问
            int startCol = random.Next(cols);
            int startRow = random.Next(rows);
            visited[startCol, startRow] = true;

            // 统计已访问的单元格数量
            int visitedCount = 1;

            // 方向数组：上、下、左、右
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };
            int[] dirValues = { 1, 2, 4, 8 };  // 对应 Dir 枚举值

            // 继续直到所有单元格都被访问
            while (visitedCount < totalCells)
            {
                // 随机选择一个未访问的单元格开始随机游走
                int currentCol, currentRow;
                do
                {
                    currentCol = random.Next(cols);
                    currentRow = random.Next(rows);
                } while (visited[currentCol, currentRow]);

                // 开始随机游走，直到碰到已访问的单元格
                List<(int col, int row)> path = new List<(int, int)>();
                path.Add((currentCol, currentRow));

                while (!visited[currentCol, currentRow])
                {
                    // 随机选择一个方向
                    int dirIdx = random.Next(4);
                    int newCol = currentCol + dx[dirIdx];
                    int newRow = currentRow + dy[dirIdx];

                    // 确保新位置在边界内
                    if (newCol >= 0 && newCol < cols && newRow >= 0 && newRow < rows)
                    {
                        // 记录路径方向
                        pathDir[currentCol, currentRow] = dirValues[dirIdx];

                        // 检查是否形成环路（碰到路径中的单元格）
                        bool isLoop = false;
                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            if (path[i].col == newCol && path[i].row == newRow)
                            {
                                // 截断环路：从环路开始位置到当前位置的路径被丢弃
                                path.RemoveRange(i + 1, path.Count - i - 1);
                                currentCol = path[path.Count - 1].col;
                                currentRow = path[path.Count - 1].row;
                                isLoop = true;
                                break;
                            }
                        }

                        if (!isLoop)
                        {
                            // 移动到新位置
                            currentCol = newCol;
                            currentRow = newRow;
                            path.Add((currentCol, currentRow));
                        }
                    }
                }

                // 将路径添加到迷宫中
                for (int i = 0; i < path.Count - 1; i++)
                {
                    int col = path[i].col;
                    int row = path[i].row;

                    // 标记为已访问
                    visited[col, row] = true;
                    visitedCount++;

                    // 标记路径格子（奇数坐标）
                    int pathX = col * 2 + 1;
                    int pathY = row * 2 + 1;
                    field[pathX, pathY] = TileType.Path;

                    // 根据方向打通中间的墙
                    int dir = pathDir[col, row];
                    int wallX = pathX;
                    int wallY = pathY;

                    switch (dir)
                    {
                        case 1: // 上
                            wallY -= 1;
                            break;
                        case 2: // 下
                            wallY += 1;
                            break;
                        case 4: // 左
                            wallX -= 1;
                            break;
                        case 8: // 右
                            wallX += 1;
                            break;
                    }

                    // 打通墙
                    field[wallX, wallY] = TileType.Path;
                }

                // 标记最后一个单元格（已访问的单元格）
                if (!visited[path[path.Count - 1].col, path[path.Count - 1].row])
                {
                    visited[path[path.Count - 1].col, path[path.Count - 1].row] = true;
                    visitedCount++;
                }
            }

            // 标记起点为路径
            field[startCol * 2 + 1, startRow * 2 + 1] = TileType.Path;

            return field;
        }
    }
}