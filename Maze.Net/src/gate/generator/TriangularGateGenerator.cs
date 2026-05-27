using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class TriangularGateGenerator : GateGenerator<TriangularMazeField>
    {
        public TriangularGateGenerator() 
        {
        
        }

        public TriangularGateGenerator(Random random) 
            : base(random) 
        {
        
        }

        public override MazeGate Generate(TriangularMazeField field)
        {
            var baseVertices = new List<int>();
            for (int col = 0; col < 2 * field.Order - 1; col++)
                baseVertices.Add(VertexIndex(field.Order - 1, col));

            var apex = VertexIndex(0, 0);
            var entrance = baseVertices[random.Next(baseVertices.Count)];

            return new MazeGate(entrance, apex);
        }

        private static int VertexIndex(int row, int col)
        {
            return row * row + col;
        }
    }
}
