using System;

namespace SimplexLab.Maze
{
    public class StairwayMazeGateGenerator : MazeGateGenerator<StairwayMazeField>
    {
        public StairwayMazeGateGenerator()
        {

        }

        public StairwayMazeGateGenerator(Random random)
            : base(random)
        {

        }

        public override MazeGate Generate(StairwayMazeField field)
        {
            // 入口：最底行最左边的格子
            var entrance = field.VertexIndex(field.Steps - 1, 0);
            // 出口：最顶端的格子
            var exit = field.VertexIndex(0, 0);

            return new MazeGate(entrance, exit)
            {
                EntranceBorder = PickOuterBorder(field, entrance),
                ExitBorder = PickOuterBorder(field, exit)
            };
        }
    }
}
