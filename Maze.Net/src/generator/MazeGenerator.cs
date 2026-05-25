using System;

namespace SimplexLab.Maze
{
    public abstract class MazeGenerator<TField> where TField : IMazeField
    {
        private Random random = null;
        private IMazeAlgorithm provider = null;

        protected MazeGenerator()
            : this(Random.Shared)
        {
        }

        protected MazeGenerator(Random random)
        {
            this.random = random;
        }

        protected TField Generate(TField field, MazeAlgorithm algorithm)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = Utils.CreateAlgorithm(algorithm, random);
            }

            var spanningTree = provider.GenerateSpanningTree(field.count, field.graph);
            field.RemoveBorders(spanningTree);
            return field;
        }
    }
}
