using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 极坐标格子
    /// </summary>
    public struct CircularTile : IEquatable<CircularTile>
    {
        /// <summary>
        /// 第几圈（从内到外）
        /// </summary>
        public int ring;
        /// <summary>
        /// 第几个扇形
        /// </summary>
        public int sector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="sector"></param>
        public CircularTile(int ring, int sector)
        {
            this.ring = ring;
            this.sector = sector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CircularTile other)
        {
            return ring == other.ring && sector == other.sector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is CircularTile other && Equals(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (ring << 16) ^ sector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CircularTile left, CircularTile right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CircularTile left, CircularTile right)
        {
            return !left.Equals(right);
        }
    }
}
