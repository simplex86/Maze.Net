namespace SimplexLab.Maze
{
    /// <summary>
    /// 坐标范围，描述迷宫所有边界的几何包围盒
    /// </summary>
    internal struct CoordinateBounds
    {
        public double MinX { get; }
        public double MinY { get; }
        public double MaxX { get; }
        public double MaxY { get; }

        public CoordinateBounds(double minX, double minY, double maxX, double maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public double Width => MaxX - MinX;

        /// <summary>
        /// 高度
        /// </summary>
        public double Height => MaxY - MinY;
    }
}
