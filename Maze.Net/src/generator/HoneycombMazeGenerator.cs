using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class HoneycombMazeGenerator
    {
        private Random random = null;
        private IMazeAlgorithm provider = null;

        public HoneycombMazeGenerator()
            : this(Random.Shared)
        {
        }

        public HoneycombMazeGenerator(Random random)
        {
            this.random = random;
        }

        public HoneycombMazeField Create(int size, MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = Utils.CreateAlgorithm(algorithm, random);
            }

            var field = new HoneycombMazeField(size);
            var spanningTree = provider.GenerateSpanningTree(field.count, field.graph);
            field.RemoveBorders(spanningTree);
            return field;
        }

        public async Task<HoneycombMazeField> CreateAsync(int size, MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            return await Task.Run(() => Create(size, algorithm));
        }
    }
}
