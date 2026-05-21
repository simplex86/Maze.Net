using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IRectangularDungeonProvider
    {
        /// <summary>
        /// 
        /// </summary>
        DungeonAlgorithm algorithm { get; }

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
        /// <param name="mulConnector"></param>
        /// <param name="tortuosity"></param>
        /// <returns></returns>
        RectangularDungeonField Create(int width,
                              int height,
                              int minRoomWidth,
                              int maxRoomWidth,
                              int minRoomHeight,
                              int maxRoomHeight,
                              int maxRoomCount,
                              int mulConnector,
                              int tortuosity);
    }

    /// <summary>
    /// 房间
    /// </summary>
    internal struct Room
    {
        public int x = 0;
        public int y = 0;
        public int w = 0;
        public int h = 0;

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
    /// 
    /// </summary>
    internal struct GrotesqueRoom
    {
        public int minx = int.MaxValue;
        public int maxx = int.MinValue;
        public int miny = int.MaxValue;
        public int maxy = int.MaxValue;

        public List<Room> rooms = new List<Room>();        

        public GrotesqueRoom()
        {

        }

        public void Add(Room room)
        {
            rooms.Add(room);
        }

        public bool IsOverlapsWith(GrotesqueRoom other)
        {
            foreach (var room in other.rooms)
            {
                if (IsOverlapsWith(room)) return true;
            }

            return false;
        }

        public bool IsOverlapsWith(Room other)
        {
            foreach (var room in rooms)
            {
                if (other.IsOverlapsWith(room)) return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 向量
    /// </summary>
    internal struct Vector
    {
        public int x = 0;
        public int y = 0;

        public Vector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
