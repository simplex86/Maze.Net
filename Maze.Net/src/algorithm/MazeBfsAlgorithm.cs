using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazeBfsAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm algorithm => EMazeAlgorithm.BFS;

        public MazeBfsAlgorithm(Random random) : base(random) { }

        public override List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
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
