namespace SimplexLab.Maze
{
    /// <summary>
    /// 弧线段边界
    /// </summary>
    internal struct ArcBorder : IMazeBorder
    {
        public double CenterX { get; }
        public double CenterY { get; }
        public double Radius { get; }
        public double StartAngle { get; }
        public double SweepAngle { get; }

        public ArcBorder(double centerX, double centerY, double radius, double startAngle, double sweepAngle)
        {
            CenterX = centerX;
            CenterY = centerY;
            Radius = radius;
            StartAngle = startAngle;
            SweepAngle = sweepAngle;
        }
    }
}
