namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    public struct RectangleField
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

        public RectangleField(int w, int h)
        {
            width  = Odd(w);
            height = Odd(h);
            
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

        /// <summary>
        /// 求奇数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int Odd(int value)
        {
            return (value / 2) * 2 + 1;
        }
    }
}