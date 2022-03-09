namespace SimpleX.Maze
{
    public class RectangleMazeField
    {
        // 迷宫场地的数据
        private int[] field = null;

        // 宽度
        public int width  { get; private set; } = 9;
        // 高度
        public int height { get; private set; } = 9;

        public RectangleMazeField(int w, int h)
        {
            width  = Odd(w);
            height = Odd(h);
            
            field = new int[width * height];
            for (int i=0; i<field.Length; i++)
            {
                field[i] = TileType.Wall;
            }
        }

        public int this[int x, int y]
        {
            get { return field[y * width + x]; }
            set { field[y * width + x] = value; }
        }

        // 求奇数
        private int Odd(int value)
        {
            return (value / 2) * 2 + 1;
        }
    }
}