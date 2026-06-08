using System;

namespace SimplexLab.Maze
{
    public class CircularHexagonMazeGateGenerator : MazeGateGenerator<CircularHexagonMazeField>
    {
        public CircularHexagonMazeGateGenerator() 
        {
        
        }

        public CircularHexagonMazeGateGenerator(Random random) 
            : base(random) 
        { 
        
        }

        public override MazeGate Generate(CircularHexagonMazeField field)
        {
            var entranceSector = random.Next(6);
            var exitSector = (entranceSector + 3) % 6;

            var col = random.Next(field.Size);
            var entrance = VertexIndex(field, entranceSector, 0, field.Size - 1, col);
            var exit = VertexIndex(field, exitSector, 0, field.Size - 1, col);

            return (random.Next(2) == 0) ? new MazeGate(exit, entrance)
                                         {
                                             EntranceBorder = PickOuterBorder(field, exit),
                                             ExitBorder = PickOuterBorder(field, entrance)
                                         }
                                         : new MazeGate(entrance, exit)
                                         {
                                             EntranceBorder = PickOuterBorder(field, entrance),
                                             ExitBorder = PickOuterBorder(field, exit)
                                         };
        }

        private static int VertexIndex(CircularHexagonMazeField field, int sector, int updown, int row, int column)
        {
            var index = sector * field.Size * field.Size;
            if (updown == 1) index += field.Size * (field.Size + 1) / 2;
            index += row * (row + 1) / 2 + column;
            return index;
        }
    }
}
