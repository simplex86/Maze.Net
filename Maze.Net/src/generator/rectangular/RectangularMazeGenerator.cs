using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// </summary>
    public class RectangularMazeGenerator
    {
        private IMazeAlgorithm provider = null;

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

            var field = new RectangularMazeField(width, height);
            return (RectangularMazeField)provider.Create(field);
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
        private IMazeAlgorithm CreateProvider(MazeAlgorithm algorithm)
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