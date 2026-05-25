using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 三角形迷宫生成器
    /// </summary>
    public class TriangularMazeGenerator
    {
        private Random random = null;
        private IMazeAlgorithm provider = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TriangularMazeGenerator()
            : this(Random.Shared)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="random"></param>
        public TriangularMazeGenerator(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// 创建三角形迷宫
        /// </summary>
        /// <param name="order">阶数</param>
        /// <param name="orientation">朝向</param>
        /// <param name="algorithm">生成算法</param>
        /// <returns>生成的迷宫场地</returns>
        public TriangularMazeField Create(int order,
                                          TriangleOrientation orientation = TriangleOrientation.Upward,
                                          MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = Utils.CreateAlgorithm(algorithm, random);
            }

            var field = new TriangularMazeField(order, orientation);
            var spanningTree = provider.GenerateSpanningTree(field.count, field.graph);
            field.RemoveBorders(spanningTree);
            return field;
        }

        /// <summary>
        /// 异步创建三角形迷宫
        /// </summary>
        /// <param name="order">阶数</param>
        /// <param name="orientation">朝向</param>
        /// <param name="algorithm">生成算法</param>
        /// <returns>生成的迷宫场地</returns>
        public async Task<TriangularMazeField> CreateAsync(int order,
                                                           TriangleOrientation orientation = TriangleOrientation.Upward,
                                                           MazeAlgorithm algorithm = MazeAlgorithm.Prim)
        {
            return await Task.Run(() => Create(order, orientation, algorithm));
        }
    }
}
