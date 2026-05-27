using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class HoneycombMazeGenerator : MazeGenerator<HoneycombMazeField>
    {
        public HoneycombMazeGenerator() 
        {
        
        }

        public HoneycombMazeGenerator(Random random) 
            : base(random) 
        { 
        
        }

        public HoneycombMazeField Generate(int size, EMazeAlgorithm algorithm = EMazeAlgorithm.Prim)
        {
            var field = new HoneycombMazeField(size);
            return Generate(field, algorithm);
        }

        public async Task<HoneycombMazeField> GenerateAsync(int size, EMazeAlgorithm algorithm = EMazeAlgorithm.Prim)
        {
            return await Task.Run(() => Generate(size, algorithm));
        }
    }
}
