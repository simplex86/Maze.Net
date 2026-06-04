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

        protected TField Generate(TField field, EMazeAlgorithm algorithm)
        {
            if (!IsAlgorithmSupported(algorithm))
                algorithm = EMazeAlgorithm.DFS;

            if (provider == null || provider.Algorithm != algorithm)
            {
                provider = Utils.CreateAlgorithm(algorithm, random);
            }

            var spanningTree = provider.GenerateSpanningTree(field.VertexCount, field.Graph);
            Utils.RemoveAdjacencyBorders(field.Graph, spanningTree);

            return field;
        }

        /// <summary>
        /// 判断当前场地是否支持指定的迷宫生成算法（默认支持所有算法）
        /// </summary>
        protected virtual bool IsAlgorithmSupported(EMazeAlgorithm algorithm) => true;
    }
}
