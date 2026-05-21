using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// </summary>
    public class RectangularMazeGenerator
    {
        private IRectangularMazeProvider provider = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RectangularMazeGenerator()
        {
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
                provider = CreateProvider(algorithm);
            }
            
            var field = provider == null ? new RectangularMazeField(width, height) 
                                         : provider.Create(width, height);

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

        /// <summary>
        /// 创建算法提供者
        /// </summary>
        /// <param name="algorithm">算法类型</param>
        /// <returns>算法提供者</returns>
        private IRectangularMazeProvider CreateProvider(MazeAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case MazeAlgorithm.DFS:
                    return new RectangularMazeDfsProvider();
                case MazeAlgorithm.Prim:
                    return new RectangularMazePrimProvider();
                case MazeAlgorithm.Kruskal:
                    return new RectangularMazeKruskalProvider();
                case MazeAlgorithm.Wilson:
                    return new RectangularMazeWilsonProvider();
                case MazeAlgorithm.Eller:
                    return new RectangularMazeEllerProvider();
                case MazeAlgorithm.AldousBroder:
                    return new RectangularMazeAldousBroderProvider();
                case MazeAlgorithm.BFS:
                    return new RectangularMazeBfsProvider();
                default:
                    break;
            }

            return null;
        }
    }
}