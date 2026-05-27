namespace SimplexLab.Maze
{
    /// <summary>
    /// 生成树的边
    /// </summary>
    public struct SpanningTreeEdge
    {
        public int u;
        public int v;

        public SpanningTreeEdge(int u, int v)
        {
            this.u = u;
            this.v = v;
        }
    }
}
