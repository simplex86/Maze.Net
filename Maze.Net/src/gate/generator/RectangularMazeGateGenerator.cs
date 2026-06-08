using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class RectangularMazeGateGenerator : MazeGateGenerator<RectangularMazeField>
    {
        public RectangularMazeGateGenerator()
        {
        
        }

        public RectangularMazeGateGenerator(Random random) 
            : base(random) 
        {
        
        }

        public override MazeGate Generate(RectangularMazeField field)
        {
            var width = field.Width;
            var height = field.Height;

            var sides = new List<int>[4];
            for (int i = 0; i < 4; i++) sides[i] = new List<int>();

            for (int x = 0; x < width; x++)
            {
                sides[0].Add(x);
                sides[1].Add((height - 1) * width + x);
            }
            for (int y = 0; y < height; y++)
            {
                sides[2].Add(y * width);
                sides[3].Add(y * width + width - 1);
            }

            var pair = random.Next(2);
            var entranceSide = pair * 2;
            var exitSide = pair * 2 + 1;

            if (random.Next(2) == 0)
                (entranceSide, exitSide) = (exitSide, entranceSide);

            var entrance = sides[entranceSide][random.Next(sides[entranceSide].Count)];
            var exit = sides[exitSide][random.Next(sides[exitSide].Count)];

            return new MazeGate(entrance, exit)
            {
                EntranceBorder = PickOuterBorder(field, entrance),
                ExitBorder = PickOuterBorder(field, exit)
            };
        }
    }
}
