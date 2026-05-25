using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazeKruskalAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm algorithm => EMazeAlgorithm.Kruskal;

        public MazeKruskalAlgorithm(Random random) : base(random) { }

        public override List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
        {
            var spanningTree = new List<(int, int)>();

            var edges = CollectEdges(graph);
            edges.Shuffle(random);

            var dsu = new DisjointSet(vertexCount);

            foreach (var (a, b) in edges)
            {
                if (dsu.Union(a, b))
                {
                    spanningTree.Add((a, b));
                    if (dsu.Count == 1) break;
                }
            }

            return spanningTree;
        }

        private List<(int a, int b)> CollectEdges(List<List<Edge>> graph)
        {
            var edges = new List<(int a, int b)>();

            for (int i = 0; i < graph.Count; i++)
            {
                foreach (var edge in graph[i])
                {
                    if (edge.Neighbor != -1 && edge.Neighbor > i)
                        edges.Add((i, edge.Neighbor));
                }
            }

            return edges;
        }
    }
}
