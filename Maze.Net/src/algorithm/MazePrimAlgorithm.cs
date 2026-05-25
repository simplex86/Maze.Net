using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// Prim迷宫生成算法
    /// </summary>
    internal class MazePrimAlgorithm : IMazeAlgorithm
    {
        private Random random = new Random();

        /// <summary>
        /// 算法
        /// </summary>
        public MazeAlgorithm algorithm => MazeAlgorithm.Prim;

        /// <summary>
        /// 在给定的图上生成随机生成树（Prim方式）
        /// </summary>
        /// <param name="vertexCount">顶点数</param>
        /// <param name="graph">邻接表</param>
        /// <returns>生成树边集</returns>
        public List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
        {
            var spanningTree = new List<(int, int)>();
            var visited = new bool[vertexCount];
            var frontier = new List<(int from, int to)>();

            int start = random.Next(vertexCount);
            visited[start] = true;
            AddFrontier(graph, visited, frontier, start);

            while (frontier.Count > 0)
            {
                var idx = random.Next(frontier.Count);
                var (from, to) = frontier[idx];
                frontier.RemoveAt(idx);

                if (visited[to])
                    continue;

                spanningTree.Add((from, to));
                visited[to] = true;
                AddFrontier(graph, visited, frontier, to);
            }

            return spanningTree;
        }

        /// <summary>
        /// 将顶点的未访问邻居加入frontier
        /// </summary>
        private void AddFrontier(List<List<Edge>> graph, bool[] visited, List<(int from, int to)> frontier, int vertex)
        {
            foreach (var edge in graph[vertex])
            {
                if (edge.Neighbor != -1 && !visited[edge.Neighbor])
                    frontier.Add((vertex, edge.Neighbor));
            }
        }
    }
}
