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

        public RectangularField Create(int width, int height)
        {
            width = Utils.Odd(width);
            height = Utils.Odd(height);

            var field = new RectangularField(width, height);
            int cols = width / 2;
            int rows = height / 2;

            // 当前行每个单元格所属的集合编号
            // 使用字典支持动态集合ID
            Dictionary<int, int> parent = new Dictionary<int, int>();

            // 处理每一行
            for (int r = 0; r < rows; r++)
            {
                // ========== 水平连接阶段 ==========
                for (int c = 0; c < cols - 1; c++)
                {
                    int currentId = r * cols + c;
                    int nextId = r * cols + (c + 1);

                    // 确保ID存在于字典中
                    if (!parent.ContainsKey(currentId)) parent[currentId] = currentId;
                    if (!parent.ContainsKey(nextId)) parent[nextId] = nextId;

                    int root1 = FindRoot(parent, currentId);
                    int root2 = FindRoot(parent, nextId);

                    // 如果不在同一集合且随机决定连接
                    if (root1 != root2 && random.Next(2) == 0)
                    {
                        // 合并集合
                        parent[root1] = root2;

                        // 打通水平墙
                        int wallX = c * 2 + 2;
                        int wallY = r * 2 + 1;
                        field[wallX, wallY] = TileType.Path;
                    }
                }

                // ========== 垂直连接阶段 ==========
                // 记录哪些单元格有垂直连接
                bool[] hasVertical = new bool[cols];

                // 确保每个集合至少有一个垂直连接（除非是最后一行）
                if (r < rows - 1)
                {
                    HashSet<int> processedRoots = new HashSet<int>();

                    for (int c = 0; c < cols; c++)
                    {
                        int cellId = r * cols + c;
                        int root = FindRoot(parent, cellId);

                        // 如果这个集合还没有垂直连接，必须添加一个
                        if (!processedRoots.Contains(root))
                        {
                            processedRoots.Add(root);

                            // 在这个集合中随机选择一个单元格建立垂直连接
                            List<int> cellsInSet = new List<int>();
                            for (int i = 0; i < cols; i++)
                            {
                                if (FindRoot(parent, r * cols + i) == root)
                                {
                                    cellsInSet.Add(i);
                                }
                            }

                            if (cellsInSet.Count > 0)
                            {
                                int selected = cellsInSet[random.Next(cellsInSet.Count)];
                                hasVertical[selected] = true;
                            }
                        }
                    }

                    // 随机添加额外的垂直连接
                    for (int c = 0; c < cols; c++)
                    {
                        if (!hasVertical[c] && random.Next(2) == 0)
                        {
                            hasVertical[c] = true;
                        }
                    }
                }

                // 执行垂直连接并标记路径
                for (int c = 0; c < cols; c++)
                {
                    // 标记当前单元格为路径
                    int pathX = c * 2 + 1;
                    int pathY = r * 2 + 1;
                    field[pathX, pathY] = TileType.Path;

                    // 如果有垂直连接且不是最后一行，打通垂直墙
                    if (hasVertical[c] && r < rows - 1)
                    {
                        int wallX = c * 2 + 1;
                        int wallY = r * 2 + 2;
                        field[wallX, wallY] = TileType.Path;
                    }
                }

                // ========== 准备下一行 ==========
                if (r < rows - 1)
                {
                    // 重置下一行的集合
                    for (int c = 0; c < cols; c++)
                    {
                        int currentId = r * cols + c;
                        int nextId = (r + 1) * cols + c;

                        if (hasVertical[c])
                        {
                            // 有垂直连接的单元格继承上一行的集合
                            int root = FindRoot(parent, currentId);
                            parent[nextId] = root;
                        }
                        else
                        {
                            // 没有垂直连接的单元格创建新集合
                            parent[nextId] = nextId;
                        }
                    }
                }
            }

            // 处理最后一行：确保所有单元格连通（必须连接）
            for (int c = 0; c < cols - 1; c++)
            {
                int cell1 = (rows - 1) * cols + c;
                int cell2 = (rows - 1) * cols + (c + 1);

                if (!parent.ContainsKey(cell1)) parent[cell1] = cell1;
                if (!parent.ContainsKey(cell2)) parent[cell2] = cell2;

                int root1 = FindRoot(parent, cell1);
                int root2 = FindRoot(parent, cell2);

                if (root1 != root2)
                {
                    parent[root1] = root2;

                    // 打通水平墙
                    int wallX = c * 2 + 2;
                    int wallY = (rows - 1) * 2 + 1;
                    field[wallX, wallY] = TileType.Path;
                }
            }

            return field;
        }

        /// <summary>
        /// 查找单元格所属集合的根（带路径压缩）
        /// </summary>
        private int FindRoot(Dictionary<int, int> parent, int cell)
        {
            if (!parent.ContainsKey(cell))
            {
                parent[cell] = cell;
            }

            if (parent[cell] != cell)
            {
                // 路径压缩：直接指向根节点
                parent[cell] = FindRoot(parent, parent[cell]);
            }
            return parent[cell];
        }
    }
}