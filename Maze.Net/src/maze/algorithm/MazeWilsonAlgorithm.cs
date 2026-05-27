using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazeWilsonAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm algorithm => EMazeAlgorithm.Wilson;

        public MazeWilsonAlgorithm(Random random) : base(random) { }

        public override List<SpanningTreeEdge> GenerateSpanningTree(int vertexCount, List<List<Adjacency>> graph)
        {
            var spanningTree = new List<SpanningTreeEdge>();
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
                    spanningTree.Add(new SpanningTreeEdge(path[i], path[i + 1]));
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

        private int PickUnvisited(bool[] visited, int vertexCount)
        {
            int vertex;
            do
            {
                vertex = random.Next(vertexCount);
            } while (visited[vertex]);

            return vertex;
        }

        private List<int> RandomWalkToVisited(List<List<Adjacency>> graph, bool[] visited, int start)
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
