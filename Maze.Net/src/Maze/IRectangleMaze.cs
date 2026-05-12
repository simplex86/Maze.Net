namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class IRectangleMaze
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public abstract RectangleMazeField Create();

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public abstract Task<RectangleMazeField> CreateAsync();

        /// <summary>
        /// 奇数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected int Odd(int value)
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
        protected bool IsWall(RectangleMazeField field, int x, int y)
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
        protected bool IsBorder(RectangleMazeField field, int x, int y)
        {
            return (x <= 0 || x >= field.width - 1 || y <= 0 || y >= field.height - 1);
        }
    }
}
