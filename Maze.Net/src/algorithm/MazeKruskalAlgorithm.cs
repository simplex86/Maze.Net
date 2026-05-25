using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// Kruskal迷宫生成算法
    /// </summary>
    internal class MazeKruskalAlgorithm : IMazeAlgorithm
    {
        private Random random = new Random();

        /// <summary>
        /// 算法
        /// </summary>
        public MazeAlgorithm algorithm => MazeAlgorithm.Kruskal;

        /// <summary>
        /// 在给定的图上生成随机生成树（Kruskal方式）
        /// </summary>
        /// <param name="vertexCount">顶点数</param>
        /// <param name="graph">邻接表</param>
        /// <returns>生成树边集</returns>
        public List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
        {
            var spanningTree = new List<(int, int)>();

            var edges = CollectEdges(graph);
            edges.Shuffle(random);

            var dsu = new DisjointSet(vertexCount);

            foreach (var (a, b) in edges)
            {
                if (dsu.Union(a, b))
                {
                    spanningTree.Add((a, b));
                    if (dsu.Count == 1) break;
                }
            }

            return spanningTree;
        }

        /// <summary>
        /// 收集所有内部边（去重，仅保留 i < j）
        /// </summary>
        private List<(int a, int b)> CollectEdges(List<List<Edge>> graph)
        {
            var edges = new List<(int a, int b)>();

            for (int i = 0; i < graph.Count; i++)
            {
                foreach (var edge in graph[i])
                {
                    if (edge.Neighbor != -1 && edge.Neighbor > i)
                        edges.Add((i, edge.Neighbor));
                }
            }

            return edges;
        }
    }
}
