using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫
    /// </summary>
    public class RectangularMazeGenerator
    {
        private IRectangularMazeProvider provider = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        public RectangularMazeGenerator()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public RectangleField Create(int width, int height, RectangularMazeAlgorithm algorithm = RectangularMazeAlgorithm.Prim)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = CreateProvider(algorithm);
            }
            
            var field = provider == null ? new RectangleField(width, height) 
                                         : provider.Create(width, height);

            return field;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public async Task<RectangleField> CreateAsync(int width, int height, RectangularMazeAlgorithm algorithm = RectangularMazeAlgorithm.Prim)
        {
            return await Task.Run(() => Create(width, height, algorithm));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        private IRectangularMazeProvider CreateProvider(RectangularMazeAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case RectangularMazeAlgorithm.DFS:
                    return new RectangularMazeDfsProvider();
                case RectangularMazeAlgorithm.Prim:
                    return new RectangularMazePrimProvider();
                case RectangularMazeAlgorithm.Kruskal:
                    return new RectangularMazeKruskalProvider();
                default:
                    break;
            }

            return null;
        }
    }
}