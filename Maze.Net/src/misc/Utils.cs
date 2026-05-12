namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// 奇数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Odd(int value)
        {
            return (value / 2) * 2 + 1;
        }

        /// <summary>
        /// 是否为墙
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool IsWall(RectangleField field, int x, int y)
        {
            return field[x, y] == TileType.Wall;
        }

        /// <summary>
        /// 是否为迷宫的边界
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool IsBorder(RectangleField field, int x, int y)
        {
            return (x <= 0 || x >= field.width - 1 || y <= 0 || y >= field.height - 1);
        }
    }
}
