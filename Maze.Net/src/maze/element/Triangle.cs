namespace SimplexLab.Maze
{
    /// <summary>
    /// 三角形
    /// </summary>
    public struct Triangle
    {
        public Vertex a;
        public Vertex b;
        public Vertex c;

        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }
}
