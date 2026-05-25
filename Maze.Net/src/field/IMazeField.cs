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
}
