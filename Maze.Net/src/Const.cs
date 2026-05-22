namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫算法
    /// </summary>
    public enum MazeAlgorithm
    {
        /// <summary>
        /// 深度优先
        /// </summary>
        DFS = 1,
        /// <summary>
        /// 广度优先
        /// </summary>
        BFS = 2,
        /// <summary>
        /// 
        /// </summary>
        Prim = 3,
        /// <summary>
        /// 
        /// </summary>
        Kruskal = 4,
        /// <summary>
        /// 
        /// </summary>
        Wilson = 5,
        /// <summary>
        /// 
        /// </summary>
        Eller = 6,
        /// <summary>
        /// 
        /// </summary>
        AldousBroder = 7,
        
    }

    /// <summary>
    /// 扇形分割策略
    /// </summary>
    public enum SectorStrategy
    {
        /// <summary>
        /// 每圈扇形数相同
        /// </summary>
        Each = 1,
        /// <summary>
        /// 弧长相同
        /// </summary>
        Arc = 2,
        /// <summary>
        /// 面积相同
        /// </summary>
        Area = 3,
    }
}