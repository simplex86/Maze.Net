namespace SimplexLab.Maze
{
    /// <summary>
    /// 三角形
    /// </summary>
    public struct Triangle
    {
        public Vertex A;
        public Vertex B;
        public Vertex C;

        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
