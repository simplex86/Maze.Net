using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public enum SectorStrategy
    {
        Arc = 1,
        Area = 2,
    }

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
                                          MazeAlgorithm algorithm = MazeAlgorithm.DFS,
                                          SectorStrategy strategy = SectorStrategy.Arc)
        {
            var field = new CircularMazeField(rings, sectors, strategy);
            return Generate(field, algorithm);
        }

        public async Task<CircularMazeField> GenerateAsync(int rings,
                                                           int sectors,
                                                           MazeAlgorithm algorithm = MazeAlgorithm.DFS,
                                                           SectorStrategy strategy = SectorStrategy.Arc)
        {
            return await Task.Run(() => Generate(rings, sectors, algorithm, strategy));
        }
    }
}
