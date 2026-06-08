using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class HexagonalMazeGateGenerator : MazeGateGenerator<HexagonalMazeField>
    {
        public HexagonalMazeGateGenerator() 
        { 
        
        }

        public HexagonalMazeGateGenerator(Random random) 
            : base(random) 
        {
        
        }

        public override MazeGate Generate(HexagonalMazeField field)
        {
            var entranceSector = random.Next(6);
            var exitSector = (entranceSector + 3) % 6;

            var entranceCandidates = new List<int>();
            var exitCandidates = new List<int>();

            var colStart = field.Size >= 3 ? 1 : 0;
            var colEnd = field.Size >= 3 ? field.Size - 2 : field.Size - 1;

            for (int i = colStart; i <= colEnd; i++)
            {
                entranceCandidates.Add(VertexIndex(field, entranceSector, 0, field.Size - 1, i));
                exitCandidates.Add(VertexIndex(field, exitSector, 0, field.Size - 1, i));
            }

            var entrance = entranceCandidates[random.Next(entranceCandidates.Count)];
            var exit = exitCandidates[random.Next(exitCandidates.Count)];

            return new MazeGate(entrance, exit)
            {
                EntranceBorder = PickOuterBorder(field, entrance),
                ExitBorder = PickOuterBorder(field, exit)
            };
        }

        private static int VertexIndex(HexagonalMazeField field, int sector, int updown, int row, int column)
        {
            var index = sector * field.Size * field.Size;
            if (updown == 1) index += field.Size * (field.Size + 1) / 2;
            index += row * (row + 1) / 2 + column;
            return index;
        }
    }
}
