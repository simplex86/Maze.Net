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
    /// 矩形地牢算法
    /// </summary>
    public enum DungeonAlgorithm
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
    }

    /// <summary>
    /// 方向
    /// </summary>
    internal enum Dir : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 上
        /// </summary>
        Up = 1,
        /// <summary>
        /// 下
        /// </summary>
        Down = 2,
        /// <summary>
        /// 左
        /// </summary>
        Left = 4,
        /// <summary>
        /// 右
        /// </summary>
        Right = 8,
    }
}