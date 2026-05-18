namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形格子
    /// </summary>
    internal struct RectangularTile
    {
        public int x = 0;
        public int y = 0;
        public int d = 0;

        public RectangularTile(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.d = 0;
        }

        public RectangularTile(int x, int y, int d)
        {
            this.x = x;
            this.y = y;
            this.d = d;
        }
    }
}
