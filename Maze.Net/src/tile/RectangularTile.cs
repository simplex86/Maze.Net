using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形格子
    /// </summary>
    public struct RectangularTile : IEquatable<RectangularTile>
    {
        /// <summary>
        /// 
        /// </summary>
        public int x = 0;
        /// <summary>
        /// 
        /// </summary>
        public int y = 0;
        /// <summary>
        /// 
        /// </summary>
        public int d = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public RectangularTile(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.d = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="d"></param>
        public RectangularTile(int x, int y, int d)
        {
            this.x = x;
            this.y = y;
            this.d = d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RectangularTile other)
        {
            return x == other.x && y == other.y && d == other.d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is RectangularTile other && Equals(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (x << 16) ^ y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(RectangularTile left, RectangularTile right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RectangularTile left, RectangularTile right)
        {
            return !left.Equals(right);
        }
    }
}
