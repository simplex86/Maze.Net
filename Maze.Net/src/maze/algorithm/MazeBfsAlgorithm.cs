using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazeBfsAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm algorithm => EMazeAlgorithm.BFS;

        public MazeBfsAlgorithm(Random random) : base(random) { }

        public override List<SpanningTreeEdge> GenerateSpanningTree(int vertexCount, List<List<Adjacency>> graph)
        {
            var spanningTree = new List<SpanningTreeEdge>();
            var visited = new bool[vertexCount];

            int start = random.Next(vertexCount);
            visited[start] = true;

            var currentLevel = new List<int> { start };

            while (currentLevel.Count > 0)
            {
                var nextEdges = new List<SpanningTreeEdge>();

                foreach (int vertex in currentLevel)
                {
                    foreach (var edge in graph[vertex])
                    {
                        if (edge.Neighbor != -1 && !visited[edge.Neighbor])
                            nextEdges.Add(new SpanningTreeEdge(vertex, edge.Neighbor));
                    }
                }

                nextEdges.Shuffle(random);

                var nextLevel = new List<int>();

                foreach (var edge in nextEdges)
                {
                    if (!visited[edge.v])
                    {
                        spanningTree.Add(edge);
                        visited[edge.v] = true;
                        nextLevel.Add(edge.v);
                    }
                }

                currentLevel = nextLevel;
            }

            return spanningTree;
        }
    }
}
