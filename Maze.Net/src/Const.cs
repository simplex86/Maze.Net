namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫的形状
    /// </summary>
    public enum EMazeShape
    {
        /// <summary>
        /// 矩形
        /// </summary>
        Rectangular,
        /// <summary>
        /// 圆形
        /// </summary>
        Circular,
        /// <summary>
        /// 蜂窝状
        /// </summary>
        Honeycomb,
        /// <summary>
        /// 三角形
        /// </summary>
        Triangular,
        /// <summary>
        /// 六边形
        /// </summary>
        Hexagonal,
        /// <summary>
        /// 圆三角格
        /// </summary>
        CircularHexagon,
        /// <summary>
        /// 阶梯形
        /// </summary>
        Stairway,
        /// <summary>
        /// 自定义
        /// </summary>
        Customized,
    }

    /// <summary>
    /// 三角形朝向
    /// </summary>
    public enum ETriangleOrientation
    {
        /// <summary>
        /// 朝上
        /// </summary>
        Upward = 1,
        /// <summary>
        /// 朝下
        /// </summary>
        Downward = 2,
    }
}
