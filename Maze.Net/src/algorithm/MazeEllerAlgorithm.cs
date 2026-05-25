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
        public MazeAlgorithm algorithm => MazeAlgorithm.Eller;

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
            var setOf = new int[vertexCount];
            int nextSetId = 1;

            // 构建行信息
            var rows = new List<List<int>>();
            int remaining = vertexCount;
            int idx = 0;
            while (remaining > 0)
            {
                var row = new List<int>();
                // 收集同一行的顶点：检查graph[idx]中是否有指向idx+1的边且不是跨行
                // 通过判断连续顶点之间是否有边来确定行边界
                row.Add(idx);
                remaining--;
                idx++;

                while (remaining > 0 && HasEdgeInRow(graph, idx - 1, idx))
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

                // 为未分配集合的顶点分配新集合
                for (int i = 0; i < row.Count; i++)
                {
                    if (setOf[row[i]] == 0)
                        setOf[row[i]] = nextSetId++;
                }

                // 尝试合并同行相邻顶点
                for (int i = 0; i < row.Count; i++)
                {
                    int nextI = (i + 1) % row.Count;
                    // 最后一行不环绕，非最后一行也不环绕（与原逻辑一致）
                    if (nextI == 0 && !isLastRow)
                        continue;

                    int vA = row[i];
                    int vB = row[nextI];

                    // 检查是否真的是邻居
                    if (!IsNeighbor(graph, vA, vB))
                        continue;

                    if (setOf[vA] != setOf[vB] && (isLastRow || random.Next(2) == 0))
                    {
                        int oldSet = setOf[vB];
                        int newSet = setOf[vA];
                        for (int k = 0; k < setOf.Length; k++)
                        {
                            if (setOf[k] == oldSet)
                                setOf[k] = newSet;
                        }
                        spanningTree.Add((vA, vB));
                    }
                }

                // 非最后一行：向下连接
                if (!isLastRow)
                {
                    var sets = new Dictionary<int, List<int>>();
                    for (int i = 0; i < row.Count; i++)
                    {
                        int root = setOf[row[i]];
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
                            if (edge.Neighbor != -1 && rows[rowIdx + 1].Contains(edge.Neighbor))
                            {
                                spanningTree.Add((v, edge.Neighbor));
                                setOf[edge.Neighbor] = setOf[v];
                                break;
                            }
                        }
                    }
                }
            }

            return spanningTree;
        }

        /// <summary>
        /// 检查同行中两个连续顶点之间是否有边
        /// </summary>
        private bool HasEdgeInRow(List<List<Edge>> graph, int a, int b)
        {
            foreach (var edge in graph[a])
            {
                if (edge.Neighbor == b)
                    return true;
            }
            return false;
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
