namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形地牢
    /// </summary>
    internal class RectangleDungeonOverlaprProvider : IRectangleDungeonProvider
    {
        // 随机数
        private Random random = null;
        // 
        private int currentRegion = -1;
        // 
        private int[,] regions = null;

        /// <summary>
        /// 邻居的方位
        /// </summary>
        private static readonly Vector[] CARDINAL_DIR = new Vector[] {
            new Vector( 0, -1), //上
            new Vector( 0,  1), //下
            new Vector(-1,  0), //左
            new Vector( 1,  0), //右
        };

        //
        private int minRoomWidth = 3;
        //
        private int maxRoomWidth = 7;
        //
        private int minRoomHeight = 3;
        //
        private int maxRoomHeight = 7;
        //
        private int maxRoomCount = 5;
        // 
        private int mulConnector = 5;
        // 曲折度
        private int tortuosity = 50;

        /// <summary>
        /// 
        /// </summary>
        public RectangleDungeonAlgorithm algorithm { get; } = RectangleDungeonAlgorithm.Nystroms;

        /// <summary>
        /// 创建地牢
        /// </summary>
        /// <returns></returns>
        public RectangleField Create(int width, 
                                     int height, 
                                     int minRoomWidth, 
                                     int maxRoomWidth, 
                                     int minRoomHeight, 
                                     int maxRoomHeight, 
                                     int maxRoomCount, 
                                     int mulConnector,
                                     int tortuosity)
        {
            width = Utils.Odd(width);
            height = Utils.Odd(height);

            this.minRoomWidth = Utils.Odd(minRoomWidth);
            this.maxRoomWidth = Utils.Odd(maxRoomWidth);
            this.minRoomHeight = Utils.Odd(minRoomHeight);
            this.maxRoomHeight = Utils.Odd(maxRoomHeight);
            this.maxRoomCount = maxRoomCount;
            this.mulConnector = mulConnector;
            this.tortuosity = Math.Clamp(tortuosity, 0, 100);
            this.random = new Random();

            currentRegion = -1;

            regions = new int[width, height];
            for (var y=0; y<height; y++)
            {
                for (var x=0; x<width; x++)
                {
                    regions[x, y] = -1;
                }
            }

            var field = new RectangleField(width, height);

            CreateRooms(field);
            CreateMaze(field);
            ConnectRegions(field);
            RemoveDeadEnds(field);

            return field;
        }

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="field"></param>
        private void CreateRooms(RectangleField field)
        {
            for (var i = 0; i < maxRoomCount; i++)
            {
                CreateRoom(field);
                StartRegion();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rooms"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        private void CreateRoom(RectangleField field)
        {
            var w = Utils.Odd(random.Next(minRoomWidth, maxRoomWidth + 1));
            var h = Utils.Odd(random.Next(minRoomHeight, maxRoomHeight + 1));
            var x = Utils.Odd(random.Next(1, field.width - w));
            var y = Utils.Odd(random.Next(1, field.height - h));

            for (var ry = y; ry < y + h; ry++)
            {
                for (var rx = x; rx < x + w; rx++)
                {
                    Carve(field, rx, ry);
                }
            }
        }

        /// <summary>
        /// 创建空地上迷宫
        /// </summary>
        /// <param name="field"></param>
        private void CreateMaze(RectangleField field)
        {
            for (var y = 1; y < field.height; y += 2)
            {
                for (var x = 1; x < field.width; x += 2)
                {
                    if (!Utils.IsWall(field, x, y)) continue;
                    GrowMaze(field, x, y);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void GrowMaze(RectangleField field, int x, int y)
        {
            var tiles = new List<Tile>();
            var prevdir = new Vector();

            StartRegion();
            Carve(field, x, y);

            tiles.Add(new Tile(x, y));

            while (tiles.Count > 0)
            {
                var tile = tiles[tiles.Count - 1];

                var uncarves = new List<Vector>();
                foreach (var dir in CARDINAL_DIR)
                {
                    if (CanCarve(field, tile, dir)) uncarves.Add(dir);
                }

                if (uncarves.Count > 0)
                {
                    var pct = random.Next(0, 100);
                    var dir = uncarves.Contains(prevdir) && pct >= tortuosity ? prevdir 
                                                                              : uncarves[random.Next(0, uncarves.Count)];

                    var a = Find(tile, dir, 1);
                    Carve(field, a.x, a.y);
                    var b = Find(tile, dir, 2);
                    Carve(field, b.x, b.y);

                    tiles.Add(b);
                    prevdir = dir;
                }
                else
                {
                    tiles.RemoveAt(tiles.Count - 1);
                    prevdir = new Vector();
                }
            }
        }

        /// <summary>
        /// 雕刻
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void Carve(RectangleField field, int x, int y)
        {
            field[x, y] = TileType.Path;
            regions[x, y] = currentRegion;
        }

        /// <summary>
        /// 判断是否可雕刻
        /// </summary>
        /// <param name="field"></param>
        /// <param name="tile"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private bool CanCarve(in RectangleField field, Tile tile, Vector dir)
        {
            var a = Find(tile, dir, 3);
            if (a.x < 0 || a.x >= field.width ||
                a.y < 0 || a.y >= field.height)
            {
                return false;
            }

            var b = Find(tile, dir, 2);
            return Utils.IsWall(field, b.x, b.y);
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartRegion()
        {
            currentRegion++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="dir"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private Tile Find(Tile tile, Vector dir, int length)
        {
            var x = tile.x + dir.x * length;
            var y = tile.y + dir.y * length;

            return new Tile(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        private void ConnectRegions(RectangleField field)
        {
            var connectorRegions = new Dictionary<Tile, HashSet<int>>();

            for (var y = 1; y < field.height - 1; y++)
            {
                for (var x = 1; x < field.width - 1; x++)
                {
                    var pos = new Tile(x, y);
                    if (!Utils.IsWall(field, x, y)) continue;

                    var sets = new HashSet<int>();
                    foreach (var dir in CARDINAL_DIR)
                    {
                        var region = regions[x + dir.x, y + dir.y];
                        if (region != -1)
                        {
                            sets.Add(region);
                        }
                    }

                    if (sets.Count < 2) continue;
                    connectorRegions[pos] = sets;
                }
            }
            
            var connectors = connectorRegions.Keys.ToList();

            var merged = new Dictionary<int, int>();
            var opened = new HashSet<int>();

            for (var i = 0; i <= currentRegion; i++)
            {
                merged[i] = i;
                opened.Add(i);
            }

            while (opened.Count > 1)
            {
                var connector = connectors[random.Next(0, connectors.Count)];
                AddJunction(field, connector.x, connector.y);

                var list = connectorRegions[connector].Select((region) => merged[region]);
                var dest = list.First();
                var sources = list.Skip(1).ToList();

                for (int i = 0; i <= currentRegion; i++)
                {
                    if (sources.Contains(merged[i]))
                    {
                        merged[i] = dest;
                    }
                }

                opened.RemoveWhere((region) => sources.Contains(region));

                connectors.RemoveAll((pos) =>
                {
                    // 在原始算法中有这个判断，但是这里会造成connectors的数量锐减，导致最终索引越界
                    // 所以这里先注释掉了
                    // if (connector.x - pos.x < 2 || connector.y - pos.y < 2) return true;

                    var sets = new HashSet<int>(connectorRegions[pos].Select((region) => merged[region]));
                    if (sets.Count > 1) return false;

                    if (random.Next(0, 1000) < mulConnector) // 偶尔连接一下，避免地牢成为单一连通结构
                    {
                        AddJunction(field, pos.x, pos.y);
                    }

                    return true;
                });

                if (connectors.Count == 0) break;
            }
        }

        /// <summary>
        /// 添加连接点
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AddJunction(RectangleField field, int x, int y)
        {
            field[x, y] = TileType.Path;
        }

        /// <summary>
        /// 删除死胡同
        /// </summary>
        /// <param name="field"></param>
        private void RemoveDeadEnds(RectangleField field)
        {
            var done = false;

            while (!done)
            {
                done = true;

                for (var y = 1; y < field.height-1; y++)
                {
                    for (var x = 1; x <field.width-1; x++)
                    {
                        Tile pos = new Tile(x, y);
                        if (Utils.IsWall(field, x, y)) continue;

                        var exits = 0;
                        foreach (var dir in CARDINAL_DIR)
                        {
                            var t = Find(pos, dir, 1);
                            if (!Utils.IsWall(field, t.x, t.y)) exits++;
                        }

                        if (exits != 1) continue;

                        done = false;
                        field[pos.x, pos.y] = TileType.Wall;
                        regions[x, y] = -1;
                    }
                }
            }
        }
    }
}