using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class HexagonalMazeGenerator : MazeGenerator<HexagonalMazeField>
    {
        public HexagonalMazeGenerator() 
        {
        
        }

        public HexagonalMazeGenerator(Random random) 
            : base(random) 
        { 
        
        }

        public HexagonalMazeField Generate(int size, MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            var field = new HexagonalMazeField(size);
            return Generate(field, algorithm);
        }

        public async Task<HexagonalMazeField> GenerateAsync(int size, MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            return await Task.Run(() => Generate(size, algorithm));
        }
    }
}
