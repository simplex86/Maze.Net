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
        /// 创建矩形迷宫
        /// </summary>
        /// <param name="rings">宽度</param>
        /// <param name="sectors">高度</param>
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
                provider = CreateAlgorithm(algorithm);
            }

            var field = new CircularMazeField(rings, sectors, strategy);
            return (CircularMazeField)provider.Create(field);
        }

        /// <summary>
        /// 异步创建矩形迷宫
        /// </summary>
        /// <param name="rings">宽度</param>
        /// <param name="sectors">高度</param>
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

        /// <summary>
        /// 创建算法提供者
        /// </summary>
        /// <param name="algorithm">算法类型</param>
        /// <returns>算法提供者</returns>
        private IMazeAlgorithm CreateAlgorithm(MazeAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case MazeAlgorithm.DFS:
                    return new MazeDfsAlgorithm();
                case MazeAlgorithm.BFS:
                    return new MazeBfsAlgorithm();
                case MazeAlgorithm.Prim:
                    return new MazePrimAlgorithm();
                case MazeAlgorithm.Kruskal:
                    return new MazeKruskalAlgorithm();
                case MazeAlgorithm.Wilson:
                    return new MazeWilsonAlgorithm();
                case MazeAlgorithm.Eller:
                    return new MazeEllerAlgorithm();
                case MazeAlgorithm.AldousBroder:
                    return new MazeAldousBroderAlgorithm();
                default:
                    break;
            }

            return new MazeDfsAlgorithm();
        }
    }
}
