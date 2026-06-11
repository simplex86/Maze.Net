namespace SimplexLab.Maze
{
    /// <summary>
    /// 带弧形边的三角形
    /// </summary>
    internal struct CurvedTriangle
    {
        public bool Upward;
        public double InnerRadius;
        public double InnerAngle;
        public double ArcRadius;
        public double ArcStartAngle;
        public double ArcSweepAngle;
        public double OuterRadius;
        public double OuterAngle;

        public CurvedTriangle(bool upward, double innerRadius, double innerAngle, double arcRadius, double arcStartAngle, double arcSweepAngle, double outerRadius, double outerAngle)
        {
            Upward = upward;
            InnerRadius = innerRadius;
            InnerAngle = innerAngle;
            ArcRadius = arcRadius;
            ArcStartAngle = arcStartAngle;
            ArcSweepAngle = arcSweepAngle;
            OuterRadius = outerRadius;
            OuterAngle = outerAngle;
        }
    }
}
