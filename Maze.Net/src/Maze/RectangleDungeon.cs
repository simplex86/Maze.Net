namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形地牢
    /// </summary>
    public class RectangleDungeon : IRectangleMaze
    {
        /// <summary>
        /// 格子
        /// </summary>
        private struct Tile
        {
            public int x = 0;
            public int y = 0;

            public Tile(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// 房间
        /// </summary>
        private struct Room
        {
            public int x = 0;
            public int y = 0;
            public int w = 0;
            public int h = 0;
            public bool open = false;

            public Room(int x, int y, int w, int h)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
            }

            public bool IsOverlapsWith(Room other)
            {
                return Math.Max(x, other.x) < Math.Min(x + w, other.x + other.w) &&
                       Math.Max(y, other.y) < Math.Min(y + h, other.y + other.h);
            }
        }
        /// <summary>
        /// 向量
        /// </summary>
        private struct Vector
        {
            public int x = 0;
            public int y = 0;

            public Vector()
            {

            }

            public Vector(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        
        // 随机数
        private Random random = new Random();
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
        private int width = 25;
        //
        private int height = 25;
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
        // 曲折度
        private int tortuosity = 50;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="minRoomWidth"></param>
        /// <param name="maxRoomWidth"></param>
        /// <param name="minRoomHeight"></param>
        /// <param name="maxRoomHeight"></param>
        /// <param name="maxRoomCount"></param>
        public RectangleDungeon(int width, int height, int minRoomWidth, int maxRoomWidth, int minRoomHeight, int maxRoomHeight, int maxRoomCount)
        {
            this.width = Odd(width);
            this.height = Odd(height);
            this.minRoomWidth = Odd(minRoomWidth);
            this.maxRoomWidth = Odd(maxRoomWidth);
            this.minRoomHeight = Odd(minRoomHeight);
            this.maxRoomHeight = Odd(maxRoomHeight);
            this.maxRoomCount = maxRoomCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="minRoomWidth"></param>
        /// <param name="maxRoomWidth"></param>
        /// <param name="minRoomHeight"></param>
        /// <param name="maxRoomHeight"></param>
        /// <param name="maxRoomCount"></param>
        /// <param name="tortuosity"></param>
        public RectangleDungeon(int width, int height, int minRoomWidth, int maxRoomWidth, int minRoomHeight, int maxRoomHeight, int maxRoomCount, int tortuosity)
        {
            this.width = Odd(width);
            this.height = Odd(height);
            this.minRoomWidth = Odd(minRoomWidth);
            this.maxRoomWidth = Odd(maxRoomWidth);
            this.minRoomHeight = Odd(minRoomHeight);
            this.maxRoomHeight = Odd(maxRoomHeight);
            this.maxRoomCount = maxRoomCount;
            this.tortuosity = Math.Clamp(tortuosity, 0, 100);
        }

        /// <summary>
        /// 创建地牢
        /// </summary>
        /// <returns></returns>
        public override RectangleMazeField Create()
        {
            currentRegion = -1;

            regions = new int[width, height];
            for (var y=0; y<height; y++)
            {
                for (var x=0; x<width; x++)
                {
                    regions[x, y] = -1;
                }
            }

            var field = new RectangleMazeField(width, height);

            CreateRooms(ref field);
            CreateMaze(ref field);
            ConnectRegions(ref field);
            RemoveDeadEnds(ref field);

            return field;
        }

        /// <summary>
        /// 创建地牢
        /// </summary>
        /// <returns></returns>
        public override async Task<RectangleMazeField> CreateAsync()
        {
            return await Task.Run(Create);
        }

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="field"></param>
        private void CreateRooms(ref RectangleMazeField field)
        {
            var rooms = new List<Room>();

            for (var i = 0; i < maxRoomCount; i++)
            {
                if (TryCreateRoom(field.width, field.height, rooms, out var room))
                {
                    rooms.Add(room);

                    StartRegion();
                    for (var ry = room.y; ry < room.y + room.h; ry++)
                    {
                        for (var rx = room.x; rx < room.x + room.w; rx++)
                        {
                            Carve(ref field, rx, ry);
                        }
                    }
                }
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
        private bool TryCreateRoom(int width, int height, List<Room> rooms, out Room room)
        {
            var times = 5;

            while (times > 0)
            {
                times--;

                var w = Odd(random.Next(minRoomWidth, maxRoomWidth + 1));
                var h = Odd(random.Next(minRoomHeight, maxRoomHeight + 1));
                var x = Odd(random.Next(1, width - w));
                var y = Odd(random.Next(1, height - h));

                room = new Room(x, y, w, h);

                var overlaps = false;
                foreach (var other in rooms)
                {
                    if (room.IsOverlapsWith(other))
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (!overlaps) return true;
            }

            room = new Room();
            return false;
        }

        /// <summary>
        /// 创建空地上迷宫
        /// </summary>
        /// <param name="field"></param>
        private void CreateMaze(ref RectangleMazeField field)
        {
            for (var y = 1; y < field.height; y += 2)
            {
                for (var x = 1; x < field.width; x += 2)
                {
                    if (!IsWall(field, x, y)) continue;
                    GrowMaze(ref field, x, y);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void GrowMaze(ref RectangleMazeField field, int x, int y)
        {
            var tiles = new List<Tile>();
            var prevdir = new Vector();

            StartRegion();
            Carve(ref field, x, y);

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
                    Carve(ref field, a.x, a.y);
                    var b = Find(tile, dir, 2);
                    Carve(ref field, b.x, b.y);

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
        private void Carve(ref RectangleMazeField field, int x, int y)
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
        private bool CanCarve(in RectangleMazeField field, Tile tile, Vector dir)
        {
            var a = Find(tile, dir, 3);
            if (a.x < 0 || a.x >= field.width ||
                a.y < 0 || a.y >= field.height)
            {
                return false;
            }

            var b = Find(tile, dir, 2);
            return IsWall(field, b.x, b.y);
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
        private void ConnectRegions(ref RectangleMazeField field)
        {
            var connectorRegions = new Dictionary<Tile, HashSet<int>>();

            for (var y = 1; y < field.height - 1; y++)
            {
                for (var x = 1; x < field.width - 1; x++)
                {
                    var pos = new Tile(x, y);
                    if (!IsWall(field, x, y)) continue;

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
                AddJunction(ref field, connector.x, connector.y);

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

                connectors.RemoveAll((pos) => {
                    // 在 Hauberk 的源码里面有这个判断，但是这里会造成connectors的数量锐减，导致最终索引越界
                    // 所以这里先注释掉了
                    // if (connector.x - pos.x < 2 && connector.y - pos.y < 2) return true;

                    var sets = new HashSet<int>(connectorRegions[pos].Select((region) => merged[region]));
                    if (sets.Count > 1) return false;

                    return true;
                });
            }
        }

        /// <summary>
        /// 添加连接点
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AddJunction(ref RectangleMazeField field, int x, int y)
        {
            field[x, y] = TileType.Path;
        }

        /// <summary>
        /// 删除死胡同
        /// </summary>
        /// <param name="field"></param>
        private void RemoveDeadEnds(ref RectangleMazeField field)
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
                        if (IsWall(field, x, y)) continue;

                        var exits = 0;
                        foreach (var dir in CARDINAL_DIR)
                        {
                            var t = Find(pos, dir, 1);
                            if (!IsWall(field, t.x, t.y)) exits++;
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