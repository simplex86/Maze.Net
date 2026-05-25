using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class HexagonalMazeGenerator
    {
        private Random random = null;
        private IMazeAlgorithm provider = null;

        public HexagonalMazeGenerator()
            : this(Random.Shared)
        {
        }

        public HexagonalMazeGenerator(Random random)
        {
            this.random = random;
        }

        public HexagonalMazeField Create(int size,
                                         MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = Utils.CreateAlgorithm(algorithm, random);
            }

            var field = new HexagonalMazeField(size);
            var spanningTree = provider.GenerateSpanningTree(field.count, field.graph);
            field.RemoveBorders(spanningTree);
            return field;
        }

        public async Task<HexagonalMazeField> CreateAsync(int size,
                                                           MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            return await Task.Run(() => Create(size, algorithm));
        }
    }
}
