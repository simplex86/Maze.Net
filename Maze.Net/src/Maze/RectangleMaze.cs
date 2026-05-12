namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫
    /// </summary>
    public class RectangleMaze : IRectangleMaze
    {
        // 格子
        private struct Tile
        {
            public int x = 0;
            public int y = 0;
            public Dir d = 0;

            public Tile(int x, int y, Dir d)
            {
                this.x = x;
                this.y = y;
                this.d = d;
            }
        }
        // 方向
        private enum Dir
        {
            Up    = 1, //上
            Down  = 2, //下
            Left  = 4, //左
            Right = 8, //右
        }

        // 随机数
        private Random random = new Random();
        // 开链表
        private List<Tile> openlist = new List<Tile>();
        // 
        private int width = 25;
        // 
        private int height = 25;

        public RectangleMaze(int width, int height)
        {
            this.width  = Odd(width);
            this.height = Odd(height);
        }

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <returns></returns>
        public override RectangleMazeField Create()
        {
            var field = new RectangleMazeField(width, height);

            // 随机起点
            var x = random.Next(1, field.width - 1);
            var y = random.Next(1, field.height - 1);
            field[x, y] = TileType.Path;

            // 从起点开始探索
            SearchNeighbours(field, x, y);
            while (openlist.Count > 0)
            {
                var idx = random.Next(0, openlist.Count);
                var cur = openlist[idx];

                x = cur.x;
                y = cur.y;

                switch (cur.d)
                {
                    case Dir.Up:
                        y = y - 1;
                        break;
                    case Dir.Down:
                        y = y + 1;
                        break;
                    case Dir.Left:
                        x = x - 1;
                        break;
                    case Dir.Right:
                        x = x + 1;
                        break;
                }

                if (IsWall(field, x, y))
                {
                    field[cur.x, cur.y] = TileType.Path;
                    if (!IsBorder(field, x, y))
                    {
                        field[x, y] = TileType.Path;
                        SearchNeighbours(field, x, y);
                    }
                }
                openlist.RemoveAt(idx);
            }

            return field;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task<RectangleMazeField> CreateAsync()
        {
            return await Task.Run(Create);
        }

        /// <summary>
        /// 获取邻居
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SearchNeighbours(RectangleMazeField field, int x, int y)
        {
            // 上
            if (!IsBorder(field, x, y - 1) && IsWall(field, x, y - 1)) openlist.Add(new Tile(x, y - 1, Dir.Up));
            // 下
            if (!IsBorder(field, x, y + 1) && IsWall(field, x, y + 1)) openlist.Add(new Tile(x, y + 1, Dir.Down));
            // 左
            if (!IsBorder(field, x - 1, y) && IsWall(field, x - 1, y)) openlist.Add(new Tile(x - 1, y, Dir.Left));
            // 右
            if (!IsBorder(field, x + 1, y) && IsWall(field, x + 1, y)) openlist.Add(new Tile(x + 1, y, Dir.Right));
        }
    }
}