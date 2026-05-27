namespace SimplexLab.Maze
{
    /// <summary>
    /// 环形扇区
    /// </summary>
    public struct AnnularSector
    {
        public double innerRadius;
        public double outerRadius;
        public double startAngle;
        public double sweepAngle;

        public AnnularSector(double innerRadius, double outerRadius, double startAngle, double sweepAngle)
        {
            this.innerRadius = innerRadius;
            this.outerRadius = outerRadius;
            this.startAngle = startAngle;
            this.sweepAngle = sweepAngle;
        }
    }
}
