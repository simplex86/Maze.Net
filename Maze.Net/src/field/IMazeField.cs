using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫场地接口（邻接表方案）
    /// </summary>
    public interface IMazeField
    {
        /// <summary>
        /// 顶点总数
        /// </summary>
        int count { get; }

        /// <summary>
        /// 邻接表（图）
        /// </summary>
        List<List<Edge>> graph { get; }

        /// <summary>
        /// 根据生成树边集移除边界
        /// </summary>
        void RemoveBorders(List<(int, int)> spanningTree);
    }

    /// <summary>
    /// 
    /// </summary>
    public class MazeField : IMazeField
    {
        /// <summary>
        /// 顶点总数
        /// </summary>
        public int count { get; protected set; }

        /// <summary>
        /// 邻接表（图）
        /// </summary>
        public List<List<Edge>> graph { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spanningTree"></param>
        public void RemoveBorders(List<(int, int)> spanningTree)
        {
            foreach (var (u, v) in spanningTree)
            {
                for (int i = 0; i < graph[u].Count; i++)
                {
                    if (graph[u][i].Neighbor == v)
                    {
                        graph[u][i].Border = null;
                        break;
                    }
                }
                for (int i = 0; i < graph[v].Count; i++)
                {
                    if (graph[v][i].Neighbor == u)
                    {
                        graph[v][i].Border = null;
                        break;
                    }
                }
            }
        }
    }
}
