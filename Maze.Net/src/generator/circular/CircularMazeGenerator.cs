using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// </summary>
    public class CircularMazeGenerator
    {
        private ICircularMazeProvider provider = null;

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
                provider = CreateProvider(algorithm);
            }

            var field = provider == null ? new CircularMazeField(rings, sectors)
                                         : provider.Create(rings, sectors, strategy);

            return field;
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
        private ICircularMazeProvider CreateProvider(MazeAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case MazeAlgorithm.DFS:
                    return new CircularMazeDfsProvider();
                case MazeAlgorithm.BFS:
                    return new CircularMazeBfsProvider();
                case MazeAlgorithm.Prim:
                    return new CircularMazePrimProvider();
                case MazeAlgorithm.Kruskal:
                    return new CircularMazeKruskalProvider();
                case MazeAlgorithm.Wilson:
                    return new CircularMazeWilsonProvider();
                case MazeAlgorithm.Eller:
                    return new CircularMazeEllerProvider();
                case MazeAlgorithm.AldousBroder:
                    return new CircularMazeAldousBroderProvider();
                default:
                    break;
            }

            return new CircularMazeDfsProvider();
        }
    }
}
