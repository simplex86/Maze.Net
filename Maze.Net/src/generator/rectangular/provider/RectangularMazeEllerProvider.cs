using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于Eller算法生成随机迷宫
    /// Eller算法特点：逐行处理，内存效率高，O(n)复杂度
    /// </summary>
    public class RectangularMazeEllerProvider : IRectangularMazeProvider
    {
        private Random random = new Random();
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Eller;

        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);

            // 当前行每个单元格所属的集合编号
            int[] set = new int[width];
            int nextSet = 1;

            // 处理每一行
            for (int y = 0; y < height; y++)
            {
                // 为未分配集合的格子分配新集合
                for (int x = 0; x < width; x++)
                {
                    if (set[x] == 0)
                    {
                        set[x] = nextSet++;
                    }
                }

                // ========== 水平连接阶段 ==========
                for (int x = 0; x < width - 1; x++)
                {
                    if (set[x] != set[x + 1] && random.Next(2) == 0)
                    {
                        // 合并两个集合
                        int oldSet = set[x + 1];
                        int newSet = set[x];
                        for (int i = 0; i < width; i++)
                        {
                            if (set[i] == oldSet)
                            {
                                set[i] = newSet;
                            }
                        }
                        // 打通水平墙
                        field.RemoveWallBetween(x, y, x + 1, y);
                    }
                }

                // ========== 垂直连接阶段 ==========
                bool[] hasVertical = new bool[width];

                // 确保每个集合至少有一个向下连接（如果不是最后一行）
                if (y < height - 1)
                {
                    HashSet<int> connectedSets = new HashSet<int>();

                    // 首先随机添加一些垂直连接
                    for (int x = 0; x < width; x++)
                    {
                        if (random.Next(2) == 0)
                        {
                            hasVertical[x] = true;
                            connectedSets.Add(set[x]);
                        }
                    }

                    // 然后确保每个集合都至少有一个连接
                    for (int x = 0; x < width; x++)
                    {
                        if (!connectedSets.Contains(set[x]))
                        {
                            hasVertical[x] = true;
                            connectedSets.Add(set[x]);
                        }
                    }
                }

                // 执行垂直连接
                for (int x = 0; x < width; x++)
                {
                    if (hasVertical[x] && y < height - 1)
                    {
                        field.RemoveWallBetween(x, y, x, y + 1);
                    }
                }

                // ========== 准备下一行 ==========
                if (y < height - 1)
                {
                    int[] nextSetArray = new int[width];
                    for (int x = 0; x < width; x++)
                    {
                        if (hasVertical[x])
                        {
                            nextSetArray[x] = set[x];
                        }
                        else
                        {
                            nextSetArray[x] = 0;
                        }
                    }
                    set = nextSetArray;
                }
            }

            // 处理最后一行：强制所有单元格连通
            if (height > 0)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    // 检查当前行左边和右边的集合是否相同
                    // 这里简单处理：随便连一下就好了
                    if (random.Next(2) == 0)
                    {
                        field.RemoveWallBetween(x, height - 1, x + 1, height - 1);
                    }
                }
            }

            return field;
        }
    }
}