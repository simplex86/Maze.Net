namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    internal interface ICircularMazeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        MazeAlgorithm algorithm { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rings"></param>
        /// <param name="sectors"></param>
        /// <returns></returns>
        CircularMazeField Create(int rings, int sectors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rings"></param>
        /// <param name="sectors"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        CircularMazeField Create(int rings, int sectors, SectorStrategy strategy);
    }
}
