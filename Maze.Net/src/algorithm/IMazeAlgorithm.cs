namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫生成算法的接口
    /// </summary>
    internal interface IMazeAlgorithm
    {
        /// <summary>
        /// 算法
        /// </summary>
        MazeAlgorithm algorithm { get; }

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IMazeField Create(IMazeField field);
    }
}
