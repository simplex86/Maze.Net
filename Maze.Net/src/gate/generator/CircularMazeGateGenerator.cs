using System;

namespace SimplexLab.Maze
{
    public class CircularMazeGateGenerator : MazeGateGenerator<CircularMazeField>
    {
        public CircularMazeGateGenerator() 
        { 
        
        }

        public CircularMazeGateGenerator(Random random) 
            : base(random) 
        {
        
        }

        public override MazeGate Generate(CircularMazeField field)
        {
            var n = field.SectorsPerRing[field.Rings - 1];
            var entranceSector = random.Next(n);
            var exitSector = (entranceSector + n / 2) % n;

            var entrance = VertexIndex(field, field.Rings - 1, entranceSector);
            var exit = VertexIndex(field, field.Rings - 1, exitSector);

            return new MazeGate(entrance, exit)
            {
                EntranceBorder = PickOuterBorder(field, entrance),
                ExitBorder = PickOuterBorder(field, exit)
            };
        }

        private static int VertexIndex(CircularMazeField field, int ring, int sector)
        {
            var index = 0;
            for (var r = 0; r < ring; r++)
                index += field.SectorsPerRing[r];

            return index + sector;
        }
    }
}
