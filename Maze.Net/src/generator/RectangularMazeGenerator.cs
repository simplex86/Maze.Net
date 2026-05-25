using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// </summary>
    public class RectangularMazeGenerator
    {
        private Random random = null;
        private IMazeAlgorithm provider = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RectangularMazeGenerator()
            : this(Random.Shared)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="random"></param>
        public RectangularMazeGenerator(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// 创建矩形迷宫
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="algorithm">生成算法</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height, MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = Utils.CreateAlgorithm(algorithm, random);
            }

            var field = new RectangularMazeField(width, height);
            var spanningTree = provider.GenerateSpanningTree(field.count, field.graph);
            field.RemoveBorders(spanningTree);
            return field;
        }

        /// <summary>
        /// 异步创建矩形迷宫
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="algorithm">生成算法</param>
        /// <returns>生成的迷宫场地</returns>
        public async Task<RectangularMazeField> CreateAsync(int width, int height, MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            return await Task.Run(() => Create(width, height, algorithm));
        }
    }
}
