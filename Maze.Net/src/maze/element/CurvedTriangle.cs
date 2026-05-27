namespace SimplexLab.Maze
{
    /// <summary>
    /// 带弧形边的三角形
    /// </summary>
    public struct CurvedTriangle
    {
        public bool upward;
        public double innerRadius;
        public double innerAngle;
        public double arcRadius;
        public double arcStartAngle;
        public double arcSweepAngle;
        public double outerRadius;
        public double outerAngle;

        public CurvedTriangle(bool upward, double innerRadius, double innerAngle, double arcRadius, double arcStartAngle, double arcSweepAngle, double outerRadius, double outerAngle)
        {
            this.upward = upward;
            this.innerRadius = innerRadius;
            this.innerAngle = innerAngle;
            this.arcRadius = arcRadius;
            this.arcStartAngle = arcStartAngle;
            this.arcSweepAngle = arcSweepAngle;
            this.outerRadius = outerRadius;
            this.outerAngle = outerAngle;
        }
    }
}
