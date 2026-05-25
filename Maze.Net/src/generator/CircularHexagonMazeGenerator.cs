using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class CircularHexagonMazeGenerator
    {
        private Random random = null;
        private IMazeAlgorithm provider = null;

        public CircularHexagonMazeGenerator()
            : this(Random.Shared)
        {
        }

        public CircularHexagonMazeGenerator(Random random)
        {
            this.random = random;
        }

        public CircularHexagonMazeField Create(int size,
                                               MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = Utils.CreateAlgorithm(algorithm, random);
            }

            var field = new CircularHexagonMazeField(size);
            var spanningTree = provider.GenerateSpanningTree(field.count, field.graph);
            field.RemoveBorders(spanningTree);
            return field;
        }

        public async Task<CircularHexagonMazeField> CreateAsync(int size,
                                                                 MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            return await Task.Run(() => Create(size, algorithm));
        }
    }
}
