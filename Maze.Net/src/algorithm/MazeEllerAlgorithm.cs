using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    internal class MazeEllerAlgorithm : MazeAlgorithm
    {
        public override EMazeAlgorithm algorithm => EMazeAlgorithm.Eller;

        public MazeEllerAlgorithm(Random random) : base(random) { }

        public override List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph)
        {
            var spanningTree = new List<(int, int)>();
            var dsu = new DisjointSet(vertexCount);

            var rows = new List<List<int>>();
            int remaining = vertexCount;
            int idx = 0;
            while (remaining > 0)
            {
                var row = new List<int>();
                row.Add(idx);
                remaining--;
                idx++;

                while (remaining > 0 && IsNeighbor(graph, idx - 1, idx))
                {
                    row.Add(idx);
                    remaining--;
                    idx++;
                }

                rows.Add(row);
            }

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                var row = rows[rowIdx];
                var isLastRow = (rowIdx == rows.Count - 1);

                for (int i = 0; i < row.Count; i++)
                {
                    int nextI = (i + 1) % row.Count;

                    int vA = row[i];
                    int vB = row[nextI];

                    if (!IsNeighbor(graph, vA, vB))
                        continue;

                    if (!dsu.IsConnected(vA, vB) && (isLastRow || random.Next(2) == 0))
                    {
                        dsu.Union(vA, vB);
                        spanningTree.Add((vA, vB));
                    }
                }

                if (!isLastRow)
                {
                    var nextRowSet = new HashSet<int>(rows[rowIdx + 1]);

                    var sets = new Dictionary<int, List<int>>();
                    for (int i = 0; i < row.Count; i++)
                    {
                        int root = dsu.Find(row[i]);
                        if (!sets.ContainsKey(root))
                            sets[root] = new List<int>();
                        sets[root].Add(i);
                    }

                    var hasVertical = new bool[row.Count];
                    foreach (var members in sets.Values)
                    {
                        hasVertical[members[random.Next(members.Count)]] = true;
                    }

                    for (int i = 0; i < row.Count; i++)
                    {
                        if (!hasVertical[i] && random.Next(2) == 0)
                            hasVertical[i] = true;
                    }

                    for (int i = 0; i < row.Count; i++)
                    {
                        if (!hasVertical[i]) continue;

                        int v = row[i];
                        foreach (var edge in graph[v])
                        {
                            if (edge.Neighbor != -1 && nextRowSet.Contains(edge.Neighbor))
                            {
                                dsu.Union(v, edge.Neighbor);
                                spanningTree.Add((v, edge.Neighbor));
                                break;
                            }
                        }
                    }
                }
            }

            return spanningTree;
        }

        private bool IsNeighbor(List<List<Edge>> graph, int a, int b)
        {
            foreach (var edge in graph[a])
            {
                if (edge.Neighbor == b)
                    return true;
            }
            return false;
        }
    }
}
