namespace SimplexLab.Maze
{
    /// <summary>
    /// 环形扇区
    /// </summary>
    public struct AnnularSector
    {
        public double InnerRadius;
        public double OuterRadius;
        public double StartAngle;
        public double SweepAngle;

        public AnnularSector(double innerRadius, double outerRadius, double startAngle, double sweepAngle)
        {
            InnerRadius = innerRadius;
            OuterRadius = outerRadius;
            StartAngle = startAngle;
            SweepAngle = sweepAngle;
        }
    }
}
