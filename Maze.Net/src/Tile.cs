using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 格子
    /// </summary>
    public struct Tile : IEquatable<Tile>
    {
        /// <summary>
        /// 
        /// </summary>
        public int lateral = 0;
        /// <summary>
        /// 
        /// </summary>
        public int radial = 0;
        /// <summary>
        /// 
        /// </summary>
        internal int dir = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lateral"></param>
        /// <param name="radial"></param>
        public Tile(int lateral, int radial)
            : this(lateral, radial, 0)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="d"></param>
        public Tile(int lateral, int radial, int dir)
        {
            this.lateral = lateral;
            this.radial = radial;
            this.dir = dir;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Tile other)
        {
            return lateral == other.lateral && 
                   radial  == other.radial  && 
                   dir     == other.dir;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Tile other && Equals(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (lateral << 16) ^ radial;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Tile left, Tile right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Tile left, Tile right)
        {
            return !left.Equals(right);
        }
    }
}
