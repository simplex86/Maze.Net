namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IRectangleMazeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        RectangleMazeAlgorithm algorithm { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        RectangleField Create(int width, int height);
    }

    /// <summary>
    /// 方向
    /// </summary>
    internal enum Dir : byte
    {
        None  = 0, // 无
        Up    = 1, // 上
        Down  = 2, // 下
        Left  = 4, // 左
        Right = 8, // 右
    }
}
