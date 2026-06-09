using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class CustomizedMazeGenerator : MazeGenerator<CustomizedMazeField>
    {
        public CustomizedMazeGenerator()
        {
        }

        public CustomizedMazeGenerator(Random random)
            : base(random)
        {
        }

        public CustomizedMazeField Generate(CustomizedMazeMask mask, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            var field = new CustomizedMazeField(mask);
            return Generate(field, algorithm);
        }

        public async Task<CustomizedMazeField> GenerateAsync(CustomizedMazeMask mask, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            return await Task.Run(() => Generate(mask, algorithm));
        }
    }
}
