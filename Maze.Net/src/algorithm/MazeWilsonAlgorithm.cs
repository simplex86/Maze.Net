using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// Wilson迷宫生成算法（环擦除随机游走）
    /// </summary>
    internal class MazeWilsonAlgorithm : IMazeAlgorithm
    {
        private Random random = new Random();

        /// <summary>
        /// 算法
        /// </summary>
        public MazeAlgorithm algorithm => MazeAlgorithm.Wilson;

        /// <summary>
        /// 在给定的图上生成随机生成树（Wilson方式）
        /// </summary>
        /// <param name="vertexCount">顶点数</param>
        /// <param name="graph">邻接表</param>
        /// <returns>生成树边集</returns>
        public List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
        {
            var spanningTree = new List<(int, int)>();
            var visited = new bool[vertexCount];

            int start = random.Next(vertexCount);
            visited[start] = true;
            int visitedCount = 1;

            while (visitedCount < vertexCount)
            {
                int walkStart = PickUnvisited(visited, vertexCount);
                var path = RandomWalkToVisited(graph, visited, walkStart);

                for (int i = 0; i < path.Count - 1; i++)
                {
                    spanningTree.Add((path[i], path[i + 1]));
                    if (!visited[path[i]])
                    {
                        visited[path[i]] = true;
                        visitedCount++;
                    }
                }

                int last = path[path.Count - 1];
                if (!visited[last])
                {
                    visited[last] = true;
                    visitedCount++;
                }
            }

            return spanningTree;
        }

        /// <summary>
        /// 随机选取一个未访问的顶点
        /// </summary>
        private int PickUnvisited(bool[] visited, int vertexCount)
        {
            int vertex;
            do
            {
                vertex = random.Next(vertexCount);
            } while (visited[vertex]);

            return vertex;
        }

        /// <summary>
        /// 从start出发随机游走，直到到达已访问顶点，返回路径（环擦除）
        /// </summary>
        private List<int> RandomWalkToVisited(List<List<Edge>> graph, bool[] visited, int start)
        {
            var direction = new Dictionary<int, int>();
            int current = start;

            while (!visited[current])
            {
                var neighbors = new List<int>();
                foreach (var edge in graph[current])
                {
                    if (edge.Neighbor != -1)
                        neighbors.Add(edge.Neighbor);
                }
                int next = neighbors[random.Next(neighbors.Count)];
                direction[current] = next;
                current = next;
            }

            // 从start回溯，环擦除
            var path = new List<int>();
            int trace = start;
            path.Add(trace);
            while (!visited[trace])
            {
                trace = direction[trace];
                path.Add(trace);
            }

            return path;
        }
    }
}
