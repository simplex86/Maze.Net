using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class RectangularMazeGenerator : MazeGenerator<RectangularMazeField>
    {
        public RectangularMazeGenerator() 
        {
        
        }

        public RectangularMazeGenerator(Random random) 
            : base(random) 
        {

        }

        public RectangularMazeField Generate(int width, int height, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            var field = new RectangularMazeField(width, height);
            return Generate(field, algorithm);
        }

        public async Task<RectangularMazeField> GenerateAsync(int width, int height, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            return await Task.Run(() => Generate(width, height, algorithm));
        }
    }
}
