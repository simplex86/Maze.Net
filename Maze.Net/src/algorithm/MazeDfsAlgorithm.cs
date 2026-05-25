using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazeDfsAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm algorithm => EMazeAlgorithm.DFS;

        public MazeDfsAlgorithm(Random random) : base(random) { }

        public override List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
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
