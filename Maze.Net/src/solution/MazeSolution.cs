using System;
using System.Collections;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public struct MazeSolution : IEnumerable<int>
    {
        private List<int> solution;

        public int Count => solution.Count;

        public MazeSolution()
        {
            solution = new List<int>();
        }

        internal void Add(int vertex)
        {
            solution.Add(vertex);
        }

        public readonly int this[int index] => solution[index];

        public readonly IEnumerator<int> GetEnumerator()
        {
            return solution.GetEnumerator();
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return solution.GetEnumerator();
        }
    }
}
