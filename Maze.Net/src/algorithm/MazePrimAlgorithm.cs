using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazePrimAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm algorithm => EMazeAlgorithm.Prim;

        public MazePrimAlgorithm(Random random) : base(random) { }

        public override List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
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
