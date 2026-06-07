using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class CircularMazeGenerator : MazeGenerator<CircularMazeField>
    {
        public CircularMazeGenerator() 
        { 
        
        }

        public CircularMazeGenerator(Random random) 
            : base(random) 
        { 
        
        }

        public CircularMazeField Generate(int rings,
                                          int sectors,
                                          EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            var field = new CircularMazeField(rings, sectors);
            return Generate(field, algorithm);
        }

        public async Task<CircularMazeField> GenerateAsync(int rings,
                                                           int sectors,
                                                           EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            return await Task.Run(() => Generate(rings, sectors, algorithm));
        }
    }
}
