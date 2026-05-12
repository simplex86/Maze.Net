namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫算法
    /// </summary>
    public enum RectangleMazeAlgorithm
    {
        Prim = 1,
    }

    /// <summary>
    /// 矩形地牢算法
    /// </summary>
    public enum RectangleDungeonAlgorithm
    {
        Nystroms = 1,
        OverlapR = 2,
    }

    /// <summary>
    /// 地块类型
    /// </summary>
    public static class TileType
    {
        public const int Wall  = 0;         // 墙
        public const int Path  = 1;         // 通路
        public const int Entry = 10 + Path; // 入口
        public const int Exit  = 20 + Path; // 出口
    }
}