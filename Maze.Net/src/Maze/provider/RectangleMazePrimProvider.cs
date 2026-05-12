namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫
    /// </summary>
    public class RectangleMazePrimProvider : IRectangleMazeProvider
    {
        /// <summary>
        /// 方向
        /// </summary>
        private enum Dir : byte
        {
            None  = 0, // 无
            Up    = 1, // 上
            Down  = 2, // 下
            Left  = 4, // 左
            Right = 8, // 右
        }

        // 随机数
        private Random random = new Random();
        // 开链表
        private List<Tile> openlist = new List<Tile>();

        /// <summary>
        /// 
        /// </summary>
        public RectangleMazeAlgorithm algorithm { get; } = RectangleMazeAlgorithm.Prim;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <returns></returns>
        public RectangleField Create(int width, int height)
        {
            width = Utils.Odd(width);
            height = Utils.Odd(height);

            var field = new RectangleField(width, height);

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
                    case (int)Dir.Up:
                        y = y - 1;
                        break;
                    case (int)Dir.Down:
                        y = y + 1;
                        break;
                    case (int)Dir.Left:
                        x = x - 1;
                        break;
                    case (int)Dir.Right:
                        x = x + 1;
                        break;
                }

                if (Utils.IsWall(field, x, y))
                {
                    field[cur.x, cur.y] = TileType.Path;
                    if (!Utils.IsBorder(field, x, y))
                    {
                        field[x, y] = TileType.Path;
                        SearchNeighbours(field, x, y);
                    }
                }
                openlist.RemoveAt(idx);
            }

            SetEntryAndExit(field);

            return field;
        }

        /// <summary>
        /// 获取邻居
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SearchNeighbours(RectangleField field, int x, int y)
        {
            // 上
            if (!Utils.IsBorder(field, x, y - 1) && Utils.IsWall(field, x, y - 1)) openlist.Add(new Tile(x, y - 1, (int)Dir.Up));
            // 下
            if (!Utils.IsBorder(field, x, y + 1) && Utils.IsWall(field, x, y + 1)) openlist.Add(new Tile(x, y + 1, (int)Dir.Down));
            // 左
            if (!Utils.IsBorder(field, x - 1, y) && Utils.IsWall(field, x - 1, y)) openlist.Add(new Tile(x - 1, y, (int)Dir.Left));
            // 右
            if (!Utils.IsBorder(field, x + 1, y) && Utils.IsWall(field, x + 1, y)) openlist.Add(new Tile(x + 1, y, (int)Dir.Right));
        }

        /// <summary>
        /// 添加入口和出口
        /// </summary>
        private void SetEntryAndExit(RectangleField field)
        {
            // 收集所有边缘上的路径格子
            var paths = new List<Tile>();

            // 顶部边缘（第一行）
            for (int x = 1; x < field.width - 1; x++)
            {
                if (field[x, 1] == TileType.Path) paths.Add(new Tile(x, 1));
            }
            // 底部边缘（最后一行）
            for (int x = 1; x < field.width - 1; x++)
            {
                if (field[x, field.height - 2] == TileType.Path) paths.Add(new Tile(x, field.height - 2));
            }
            // 左侧边缘（第一列，排除角落）
            for (int y = 2; y < field.height - 2; y++)
            {
                if (field[1, y] == TileType.Path) paths.Add(new Tile(1, y));
            }
            // 右侧边缘（最后一列，排除角落）
            for (int y = 2; y < field.height - 2; y++)
            {
                if (field[field.width - 2, y] == TileType.Path) paths.Add(new Tile(field.width - 2, y));
            }

            if (paths.Count >= 2)
            {
                // 随机选择入口位置（优先选择顶部边缘）
                var tops = paths.Where(p => p.y == 1).ToList();
                var entry = tops.Count > 0 ? tops[random.Next(tops.Count)] : paths[random.Next(paths.Count)];

                // 移除入口位置，从剩余中选择出口（优先选择底部边缘）
                paths.Remove(entry);
                var bottoms = paths.Where(p => p.y == field.height - 2).ToList();
                var exit = bottoms.Count > 0 ? bottoms[random.Next(bottoms.Count)] : paths[random.Next(paths.Count)];

                // 标记入口和出口
                field[entry.x, entry.y] = TileType.Entry;
                field[exit.x, exit.y] = TileType.Exit;
            }
        }
    }
}