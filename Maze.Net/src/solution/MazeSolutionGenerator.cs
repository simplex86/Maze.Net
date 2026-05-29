using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class MazeSolutionGenerator
    {
        public MazeSolution Generate(MazeField field, MazeGate gate)
        {
            var solution = new MazeSolution();

            if (gate.Entrance < 0 || gate.Exit < 0)
                return solution;

            var graph = field.Graph;
            var visited = new bool[graph.Count];
            var parent = new int[graph.Count];
            for (int i = 0; i < parent.Length; i++)
                parent[i] = -1;

            var queue = new Queue<int>();
            queue.Enqueue(gate.Entrance);
            visited[gate.Entrance] = true;

            while (queue.Count > 0)
            {
                var v = queue.Dequeue();

                if (v == gate.Exit)
                    break;

                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor < 0)
                        continue;

                    if (!edge.IsOpen)
                        continue;

                    if (visited[edge.Neighbor])
                        continue;

                    visited[edge.Neighbor] = true;
                    parent[edge.Neighbor] = v;
                    queue.Enqueue(edge.Neighbor);
                }
            }

            if (!visited[gate.Exit])
                return solution;

            var path = new List<int>();
            var current = gate.Exit;
            while (current != -1)
            {
                path.Add(current);
                current = parent[current];
            }

            path.Reverse();

            foreach (var vertex in path)
                solution.Add(vertex);

            return solution;
        }

        public async Task<MazeSolution> GenerateAsync(MazeField field, MazeGate gate)
        {
            return await Task.Run(() => Generate(field, gate));
        }
    }
}
