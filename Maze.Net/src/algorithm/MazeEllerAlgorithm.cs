using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// Eller迷宫生成算法
    /// </summary>
    internal class MazeEllerAlgorithm : IMazeAlgorithm
    {
        private Random random = null;

        /// <summary>
        /// 算法
        /// </summary>
        public EMazeAlgorithm algorithm => EMazeAlgorithm.Eller;

        /// <summary>
        ///
        /// </summary>
        /// <param name="random"></param>
        public MazeEllerAlgorithm(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// 在给定的图上生成随机生成树（Eller方式）
        /// </summary>
        /// <param name="vertexCount">顶点数</param>
        /// <param name="graph">邻接表</param>
        /// <returns>生成树边集</returns>
        public List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
        {
            var spanningTree = new List<(int, int)>();
            var dsu = new DisjointSet(vertexCount);

            // 构建行信息
            var rows = new List<List<int>>();
            int remaining = vertexCount;
            int idx = 0;
            while (remaining > 0)
            {
                var row = new List<int>();
                row.Add(idx);
                remaining--;
                idx++;

                while (remaining > 0 && IsNeighbor(graph, idx - 1, idx))
                {
                    row.Add(idx);
                    remaining--;
                    idx++;
                }

                rows.Add(row);
            }

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                var row = rows[rowIdx];
                var isLastRow = (rowIdx == rows.Count - 1);

                // 尝试合并同行相邻顶点
                for (int i = 0; i < row.Count; i++)
                {
                    int nextI = (i + 1) % row.Count;

                    int vA = row[i];
                    int vB = row[nextI];

                    // 检查是否真的是邻居（自然过滤矩形迷宫首尾不相邻的情况）
                    if (!IsNeighbor(graph, vA, vB))
                        continue;

                    if (!dsu.IsConnected(vA, vB) && (isLastRow || random.Next(2) == 0))
                    {
                        dsu.Union(vA, vB);
                        spanningTree.Add((vA, vB));
                    }
                }

                // 非最后一行：向下连接
                if (!isLastRow)
                {
                    // 预构建下一行的 HashSet，O(1) 查找
                    var nextRowSet = new HashSet<int>(rows[rowIdx + 1]);

                    // 按集合分组，每个集合至少选一个向下连接
                    var sets = new Dictionary<int, List<int>>();
                    for (int i = 0; i < row.Count; i++)
                    {
                        int root = dsu.Find(row[i]);
                        if (!sets.ContainsKey(root))
                            sets[root] = new List<int>();
                        sets[root].Add(i);
                    }

                    var hasVertical = new bool[row.Count];
                    foreach (var members in sets.Values)
                    {
                        hasVertical[members[random.Next(members.Count)]] = true;
                    }

                    for (int i = 0; i < row.Count; i++)
                    {
                        if (!hasVertical[i] && random.Next(2) == 0)
                            hasVertical[i] = true;
                    }

                    for (int i = 0; i < row.Count; i++)
                    {
                        if (!hasVertical[i]) continue;

                        int v = row[i];
                        foreach (var edge in graph[v])
                        {
                            if (edge.Neighbor != -1 && nextRowSet.Contains(edge.Neighbor))
                            {
                                dsu.Union(v, edge.Neighbor);
                                spanningTree.Add((v, edge.Neighbor));
                                break;
                            }
                        }
                    }
                }
            }

            return spanningTree;
        }

        /// <summary>
        /// 检查两个顶点是否为邻居
        /// </summary>
        private bool IsNeighbor(List<List<Edge>> graph, int a, int b)
        {
            foreach (var edge in graph[a])
            {
                if (edge.Neighbor == b)
                    return true;
            }
            return false;
        }
    }
}
