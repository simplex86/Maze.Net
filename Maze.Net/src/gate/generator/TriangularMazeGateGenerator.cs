using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class TriangularMazeGateGenerator : MazeGateGenerator<TriangularMazeField>
    {
        public TriangularMazeGateGenerator() 
        {
        
        }

        public TriangularMazeGateGenerator(Random random) 
            : base(random) 
        {
        
        }

        public override MazeGate Generate(TriangularMazeField field)
        {
            var baseVertices = new List<int>();
            for (int col = 0; col < 2 * field.Order - 1; col += 2)
                baseVertices.Add(VertexIndex(field.Order - 1, col));

            var apex = VertexIndex(0, 0);
            var entrance = baseVertices[random.Next(baseVertices.Count)];

            return new MazeGate(entrance, apex)
            {
                EntranceBorder = PickOuterBorder(field, entrance),
                ExitBorder = PickOuterBorder(field, apex)
            };
        }

        private static int VertexIndex(int row, int col)
        {
            return row * row + col;
        }
    }
}
