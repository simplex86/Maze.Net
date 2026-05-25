using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 深度优先搜索迷宫生成算法
    /// </summary>
    internal class MazeDfsAlgorithm : IMazeAlgorithm
    {
        private Random random = new Random();

        /// <summary>
        /// 算法
        /// </summary>
        public MazeAlgorithm algorithm => MazeAlgorithm.DFS;

        /// <summary>
        /// 在给定的图上生成随机生成树（DFS方式）
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

            var stack = new Stack<int>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                int current = stack.Peek();

                // 收集当前顶点的未访问邻居
                var unvisited = new List<int>();
                foreach (var edge in graph[current])
                {
                    if (edge.Neighbor != -1 && !visited[edge.Neighbor])
                        unvisited.Add(edge.Neighbor);
                }

                if (unvisited.Count > 0)
                {
                    int next = unvisited[random.Next(unvisited.Count)];
                    spanningTree.Add((current, next));
                    visited[next] = true;
                    stack.Push(next);
                }
                else
                {
                    stack.Pop();
                }
            }

            return spanningTree;
        }
    }
}
