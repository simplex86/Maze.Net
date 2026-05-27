using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class CircularHexagonMazeGenerator : MazeGenerator<CircularHexagonMazeField>
    {
        public CircularHexagonMazeGenerator() 
        { 
        
        }

        public CircularHexagonMazeGenerator(Random random) 
            : base(random) 
        {
        
        }

        public CircularHexagonMazeField Generate(int size, EMazeAlgorithm algorithm = EMazeAlgorithm.Prim)
        {
            var field = new CircularHexagonMazeField(size);
            return Generate(field, algorithm);
        }

        public async Task<CircularHexagonMazeField> GenerateAsync(int size, EMazeAlgorithm algorithm = EMazeAlgorithm.Prim)
        {
            return await Task.Run(() => Generate(size, algorithm));
        }
    }
}
