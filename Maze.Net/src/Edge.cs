namespace SimplexLab.Maze
{
    /// <summary>
    /// 邻接表中的边
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// 邻居顶点索引，-1 表示边界
        /// </summary>
        public int Neighbor { get; set; }

        /// <summary>
        /// 边界几何信息，null 表示边界已被移除（通道）
        /// </summary>
        public IMazeBorder? Border { get; set; }

        public Edge(int neighbor, IMazeBorder? border)
        {
            Neighbor = neighbor;
            Border = border;
        }
    }
}
