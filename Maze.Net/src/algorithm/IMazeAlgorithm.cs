using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫生成算法的接口（生成树返回方式）
    /// </summary>
    internal interface IMazeAlgorithm
    {
        /// <summary>
        /// 算法类型
        /// </summary>
        MazeAlgorithm algorithm { get; }

        /// <summary>
        /// 在给定的图上生成随机生成树
        /// </summary>
        /// <param name="vertexCount">顶点数</param>
        /// <param name="graph">邻接表</param>
        /// <returns>生成树边集</returns>
        List<(int, int)> GenerateSpanningTree(int vertexCount, List<List<Edge>> graph);
    }
}
