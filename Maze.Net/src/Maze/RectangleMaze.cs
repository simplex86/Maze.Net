namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫
    /// </summary>
    public class RectangleMaze
    {
        private IRectangleMazeProvider provider = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        public RectangleMaze()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public RectangleField Create(int width, int height, RectangleMazeAlgorithm algorithm = RectangleMazeAlgorithm.Prim)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = CreateProvider(algorithm);
            }
            
            return provider == null ? new RectangleField(width, height) 
                                    : provider.Create(width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public async Task<RectangleField> CreateAsync(int width, int height, RectangleMazeAlgorithm algorithm = RectangleMazeAlgorithm.Prim)
        {
            return await Task.Run(() => Create(width, height, algorithm));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        private IRectangleMazeProvider CreateProvider(RectangleMazeAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case RectangleMazeAlgorithm.Prim:
                    return new RectangleMazePrimProvider();
                default:
                    break;
            }

            return null;
        }
    }
}