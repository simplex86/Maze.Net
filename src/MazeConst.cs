namespace SimpleX.Maze
{
    // 迷宫形状
    public enum MazeShape
    {
        // 矩形
        Rectangle = 0,
    }

    // 类型
    public static class TileType
    {
        public const int Wall = 0; //墙
        public const int Path = 1; //通路
    }
}