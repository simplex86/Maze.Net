using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 广度优先搜索迷宫生成算法
    /// </summary>
    internal class MazeBfsAlgorithm : IMazeAlgorithm
    {
        private Random random = null;

        /// <summary>
        /// 算法
        /// </summary>
        public EMazeAlgorithm algorithm => EMazeAlgorithm.BFS;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="random"></param>
        public MazeBfsAlgorithm(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// 在给定的图上生成随机生成树（BFS方式）
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

            var currentLevel = new List<int> { start };

            while (currentLevel.Count > 0)
            {
                var nextEdges = new List<(int parent, int child)>();

                foreach (int vertex in currentLevel)
                {
                    foreach (var edge in graph[vertex])
                    {
                        if (edge.Neighbor != -1 && !visited[edge.Neighbor])
                            nextEdges.Add((vertex, edge.Neighbor));
                    }
                }

                nextEdges.Shuffle(random);

                var nextLevel = new List<int>();

                foreach (var (parent, child) in nextEdges)
                {
                    if (!visited[child])
                    {
                        spanningTree.Add((parent, child));
                        visited[child] = true;
                        nextLevel.Add(child);
                    }
                }

                currentLevel = nextLevel;
            }

            return spanningTree;
        }
    }
}
