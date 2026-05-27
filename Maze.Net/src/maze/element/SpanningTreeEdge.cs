namespace SimplexLab.Maze
{
    /// <summary>
    /// 生成树的边
    /// </summary>
    public struct SpanningTreeEdge
    {
        public int U;
        public int V;

        public SpanningTreeEdge(int u, int v)
        {
            U = u;
            V = v;
        }
    }
}
