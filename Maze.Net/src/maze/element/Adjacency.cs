namespace SimplexLab.Maze
{
    /// <summary>
    /// 顶点与邻居的邻接关系
    /// </summary>
    public class Adjacency
    {
        /// <summary>
        /// 邻居顶点索引，-1 表示边界
        /// </summary>
        public int Neighbor { get; }

        /// <summary>
        /// 边界几何信息，null 表示边界已被移除（通道）
        /// </summary>
        public IMazeBorder? Border { get; internal set; }

        public Adjacency(int neighbor, IMazeBorder? border)
        {
            Neighbor = neighbor;
            Border = border;
        }
    }
}
