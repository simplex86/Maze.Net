namespace SimplexLab.Maze
{
    /// <summary>
    /// 格子
    /// </summary>
    internal struct RectangleTile
    {
        public int x = 0;
        public int y = 0;
        public int d = 0;

        public RectangleTile(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.d = 0;
        }

        public RectangleTile(int x, int y, int d)
        {
            this.x = x;
            this.y = y;
            this.d = d;
        }
    }
}
