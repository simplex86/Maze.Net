namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    public struct RectangularField
    {
        /// <summary>
        /// 迷宫场地的数据
        /// </summary>
        private int[] field = null;

        /// <summary>
        /// 宽度
        /// </summary>
        public int width  { get; private set; } = 9;
        /// <summary>
        /// 高度
        /// </summary>
        public int height { get; private set; } = 9;

        public RectangularField(int w, int h)
        {
            width  = Utils.Odd(w);
            height = Utils.Odd(h);
            
            field = new int[width * height];
            for (int i=0; i<field.Length; i++)
            {
                field[i] = TileType.Wall;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int this[int x, int y]
        {
            get { return field[y * width + x]; }
            internal set { field[y * width + x] = value; }
        }
    }
}