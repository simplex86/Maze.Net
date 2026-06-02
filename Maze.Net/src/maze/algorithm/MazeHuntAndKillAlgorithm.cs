using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazeHuntAndKillAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm Algorithm => EMazeAlgorithm.HuntAndKill;

        public MazeHuntAndKillAlgorithm(Random random)
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
                var unvisited = new List<int>();
                foreach (var edge in graph[current])
                {
                    if (edge.Neighbor != -1 && !visited[edge.Neighbor])
                        unvisited.Add(edge.Neighbor);
                }

                if (unvisited.Count > 0)
                {
                    var next = unvisited[random.Next(unvisited.Count)];
                    spanningTree.Add(new SpanningTreeEdge(current, next));
                    visited[next] = true;
                    visitedCount++;
                    current = next;
                }
                else
                {
                    var found = false;
                    for (int v = 0; v < vertexCount; v++)
                    {
                        if (visited[v])
                            continue;

                        foreach (var edge in graph[v])
                        {
                            if (edge.Neighbor != -1 && visited[edge.Neighbor])
                            {
                                spanningTree.Add(new SpanningTreeEdge(v, edge.Neighbor));
                                visited[v] = true;
                                visitedCount++;
                                current = v;
                                found = true;
                                break;
                            }
                        }

                        if (found)
                            break;
                    }
                }
            }

            return spanningTree;
        }
    }
}
