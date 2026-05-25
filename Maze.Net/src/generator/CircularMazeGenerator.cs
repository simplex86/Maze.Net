using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// </summary>
    public class CircularMazeGenerator
    {
        private IMazeAlgorithm provider = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CircularMazeGenerator()
        {
        }

        /// <summary>
        /// 创建圆形迷宫
        /// </summary>
        /// <param name="rings">环数</param>
        /// <param name="sectors">最大扇区数</param>
        /// <param name="algorithm">生成算法</param>
        /// <param name="strategy">分割策略</param>
        /// <returns>生成的迷宫场地</returns>
        public CircularMazeField Create(int rings,
                                        int sectors,
                                        MazeAlgorithm algorithm = MazeAlgorithm.DFS,
                                        SectorStrategy strategy = SectorStrategy.Arc)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = Utils.CreateAlgorithm(algorithm);
            }

            var field = new CircularMazeField(rings, sectors, strategy);
            var spanningTree = provider.GenerateSpanningTree(field.count, field.graph);
            field.RemoveBorders(spanningTree);
            return field;
        }

        /// <summary>
        /// 异步创建圆形迷宫
        /// </summary>
        /// <param name="rings">环数</param>
        /// <param name="sectors">最大扇区数</param>
        /// <param name="algorithm">生成算法</param>
        /// <param name="strategy">分割策略</param>
        /// <returns>生成的迷宫场地</returns>
        public async Task<CircularMazeField> CreateAsync(int rings,
                                                         int sectors,
                                                         MazeAlgorithm algorithm = MazeAlgorithm.DFS,
                                                         SectorStrategy strategy = SectorStrategy.Arc)
        {
            return await Task.Run(() => Create(rings, sectors, algorithm, strategy));
        }
    }
}
