using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleX.Maze
{
    public class RectangleDungeon : IRectangleMaze
    {
        // 格子
        private class Tile
        {
            public int x = 0;
            public int y = 0;

            public Tile(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        // 房间
        private class Room
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
        // 向量
        private class Vector
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
        private int currentRegion = -1;
        private int[,] regions = null;

        // 邻居的方位
        private static readonly Vector[] CARDINAL_DIR = new Vector[] {
            new Vector( 0, -1), //上
            new Vector( 0,  1), //下
            new Vector(-1,  0), //左
            new Vector( 1,  0), //右
        };

        public int width { get; set; } = 25;
        public int height { get; set; } = 25;
        public int minRoomWidth { get; set; } = 3;
        public int maxRoomWidth { get; set; } = 7;
        public int minRoomHeight { get; set; } = 3;
        public int maxRoomHeight { get; set; } = 7;
        public int maxRoomCount { get; set; } = 5;


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

        // 创建迷宫
        public override RectangleMazeField Create()
        {
            currentRegion = -1;

            regions = new int[width, height];
            for (int y=0; y<height; y++)
            {
                for (int x=0; x<width; x++)
                {
                    regions[x, y] = -1;
                }
            }

            RectangleMazeField field = new RectangleMazeField(width, height);

            CreateRooms(field);
            CreateMaze(field);
            ConnectRegions(field);
            RemoveDeadEnds(field);

            return field;
        }

        // 创建房间
        private void CreateRooms(RectangleMazeField field)
        {
            List<Room> rooms = new List<Room>();

            for (int i = 0; i < maxRoomCount; i++)
            {
                int w = Odd(random.Next(minRoomWidth,  maxRoomWidth + 1));
                int h = Odd(random.Next(minRoomHeight, maxRoomHeight + 1));
                int x = Odd(random.Next(1, field.width  - w));
                int y = Odd(random.Next(1, field.height - h));

                Room room = new Room(x, y, w, h);

                bool overlaps = false;
                foreach (var other in rooms)
                {
                    if (room.IsOverlapsWith(other))
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (overlaps) continue;
                rooms.Add(room);

                StartRegion();
                for (int ry = room.y; ry < room.y + room.h; ry++)
                {
                    for (int rx = room.x; rx < room.x + room.w; rx++)
                    {
                        Carve(field, rx, ry);
                    }
                }
            }
        }

        // 创建空地上迷宫
        private void CreateMaze(RectangleMazeField field)
        {
            for (int y = 1; y < field.height; y += 2)
            {
                for (int x = 1; x < field.width; x += 2)
                {
                    if (!IsWall(field, x, y)) continue;
                    GrowMaze(field, x, y);
                }
            }
        }

        // 
        private void GrowMaze(RectangleMazeField field, int x, int y)
        {
            List<Tile> tiles = new List<Tile>();
            Vector lastDir = new Vector();

            StartRegion();
            Carve(field, x, y);

            tiles.Add(new Tile(x, y));

            while (tiles.Count > 0)
            {
                var tile = tiles[tiles.Count - 1];

                List<Vector> uncarves = new List<Vector>();
                foreach (var dir in CARDINAL_DIR)
                {
                    if (CanCarve(field, tile, dir)) uncarves.Add(dir);
                }

                if (uncarves.Count > 0)
                {
                    var dir = uncarves.Contains(lastDir) ? lastDir : uncarves[random.Next(0, uncarves.Count)];

                    var a = Find(tile, dir, 1);
                    Carve(field, a.x, a.y);
                    var b = Find(tile, dir, 2);
                    Carve(field, b.x, b.y);

                    tiles.Add(b);
                    lastDir = dir;
                }
                else
                {
                    tiles.RemoveAt(tiles.Count - 1);
                    lastDir = new Vector();
                }
            }
        }

        // 雕刻
        private void Carve(RectangleMazeField field, int x, int y)
        {
            field[x, y] = TileType.Path;
            regions[x, y] = currentRegion;
        }

        // 判断是否可雕刻
        private bool CanCarve(RectangleMazeField field, Tile tile, Vector vector)
        {
            var a = Find(tile, vector, 3);
            if (a.x < 0 || a.x >= field.width || a.y < 0 || a.y >= field.height) return false;

            var b = Find(tile, vector, 2);
            return IsWall(field, b.x, b.y);
        }

        //
        private void StartRegion()
        {
            currentRegion++;
        }

        private Tile Find(Tile tile, Vector vector, int length)
        {
            int x = tile.x + vector.x * length;
            int y = tile.y + vector.y * length;

            return new Tile(x, y);
        }

        private void ConnectRegions(RectangleMazeField field)
        {
            Dictionary<Tile, HashSet<int>> connectorRegions = new Dictionary<Tile, HashSet<int>>();

            for (int y = 1; y < field.height - 1; y++)
            {
                for (int x = 1; x < field.width - 1; x++)
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

            for (int i = 0; i <= currentRegion; i++)
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

        // 添加连接点
        private void AddJunction(RectangleMazeField field, int x, int y)
        {
            field[x, y] = TileType.Path;
        }

        // 删除死胡同
        private void RemoveDeadEnds(RectangleMazeField field)
        {
            bool done = false;

            while (!done)
            {
                done = true;

                for (int y = 1; y < field.height-1; y++)
                {
                    for (int x = 1; x <field.width-1; x++)
                    {
                        Tile pos = new Tile(x, y);
                        if (IsWall(field, x, y)) continue;

                        int exits = 0;
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