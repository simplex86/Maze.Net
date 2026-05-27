using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazeAldousBroderAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm Algorithm => EMazeAlgorithm.AldousBroder;

        public MazeAldousBroderAlgorithm(Random random) 
            : base(random) 
        {
        
        }

        public override List<SpanningTreeEdge> GenerateSpanningTree(int vertexCount, List<List<Adjacency>> graph)
        {
            var spanningTree = new List<SpanningTreeEdge>();
            var visited = new bool[vertexCount];

            var current = random.Next(vertexCount);
            visited[current] = true;
            var visitedCount = 1;

            while (visitedCount < vertexCount)
            {
                var neighbors = new List<int>();
                foreach (var edge in graph[current])
                {
                    if (edge.Neighbor != -1)
                        neighbors.Add(edge.Neighbor);
                }

                var next = neighbors[random.Next(neighbors.Count)];

                if (!visited[next])
                {
                    spanningTree.Add(new SpanningTreeEdge(current, next));
                    visited[next] = true;
                    visitedCount++;
                }

                current = next;
            }

            return spanningTree;
        }
    }
}
