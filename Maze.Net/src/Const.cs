namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫算法
    /// </summary>
    public enum RectangularMazeAlgorithm
    {
        /// <summary>
        /// 深度优先
        /// </summary>
        DFS     = 1,
        /// <summary>
        /// 
        /// </summary>
        Prim    = 2,
        /// <summary>
        /// 
        /// </summary>
        Kruskal = 3,
    }

    /// <summary>
    /// 矩形地牢算法
    /// </summary>
    public enum RectangularDungeonAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        Nystroms = 1,
        /// <summary>
        /// 由 Nystroms 改进的算法，生成异形房间
        /// </summary>
        OverlapR = 2,
    }

    /// <summary>
    /// 地块类型
    /// </summary>
    public static class TileType
    {
        /// <summary>
        /// 墙
        /// </summary>
        public const int Wall  = 0;        
        /// <summary>
        /// 通路
        /// </summary>
        public const int Path  = 1;        
        /// <summary>
        /// 入口
        /// </summary>
        public const int Entry = 10 + Path;
        /// <summary>
        /// 出口
        /// </summary>
        public const int Exit  = 20 + Path;
    }
}