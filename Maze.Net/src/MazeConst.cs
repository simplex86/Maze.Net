namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫形状
    /// </summary>
    public enum MazeShape
    {
        /// <summary>
        /// 矩形
        /// </summary>
        Rectangle = 0,
    }

    /// <summary>
    /// 类型
    /// </summary>
    public static class TileType
    {
        public const int Wall = 0; //墙
        public const int Path = 1; //通路
    }
}