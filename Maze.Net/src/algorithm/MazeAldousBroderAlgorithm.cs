using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// Aldous-Broder迷宫生成算法
    /// </summary>
    internal class MazeAldousBroderAlgorithm : IMazeAlgorithm
    {
        private Random random = null;

        /// <summary>
        /// 算法
        /// </summary>
        public EMazeAlgorithm algorithm => EMazeAlgorithm.AldousBroder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="random"></param>
        public MazeAldousBroderAlgorithm(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// 在给定的图上生成随机生成树（Aldous-Broder方式）
        /// </summary>
        /// <param name="vertexCount">顶点数</param>
        /// <param name="graph">邻接表</param>
        /// <returns>生成树边集</returns>
        public List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
        {
            var spanningTree = new List<(int, int)>();
            var visited = new bool[vertexCount];

            int current = random.Next(vertexCount);
            visited[current] = true;
            int visitedCount = 1;

            while (visitedCount < vertexCount)
            {
                var neighbors = new List<int>();
                foreach (var edge in graph[current])
                {
                    if (edge.Neighbor != -1)
                        neighbors.Add(edge.Neighbor);
                }

                int next = neighbors[random.Next(neighbors.Count)];

                if (!visited[next])
                {
                    spanningTree.Add((current, next));
                    visited[next] = true;
                    visitedCount++;
                }

                current = next;
            }

            return spanningTree;
        }
    }
}
