namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IRectangularMazeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        MazeAlgorithm algorithm { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        RectangularMazeField Create(int width, int height);
    }
}
