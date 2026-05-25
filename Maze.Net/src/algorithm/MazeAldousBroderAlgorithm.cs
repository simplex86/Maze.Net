using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazeAldousBroderAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm algorithm => EMazeAlgorithm.AldousBroder;

        public MazeAldousBroderAlgorithm(Random random) : base(random) { }

        public override List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
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
