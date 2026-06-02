using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫算法
    /// </summary>
    public enum EMazeAlgorithm
    {
        /// <summary>
        /// 深度优先
        /// </summary>
        DFS = 1,
        /// <summary>
        /// 广度优先
        /// </summary>
        BFS = 2,
        /// <summary>
        /// 
        /// </summary>
        Prim = 3,
        /// <summary>
        /// 
        /// </summary>
        Kruskal = 4,
        /// <summary>
        /// 
        /// </summary>
        Wilson = 5,
        /// <summary>
        /// 
        /// </summary>
        Eller = 6,
        /// <summary>
        /// 
        /// </summary>
        AldousBroder = 7,
        /// <summary>
        /// 
        /// </summary>
        HuntAndKill = 8,
    }

    /// <summary>
    /// 迷宫生成算法的接口（生成树返回方式）
    /// </summary>
    internal interface IMazeAlgorithm
    {
        /// <summary>
        /// 算法类型
        /// </summary>
        EMazeAlgorithm Algorithm { get; }

        /// <summary>
        /// 在给定的图上生成随机生成树
        /// </summary>
        /// <param name="vertexCount">顶点数</param>
        /// <param name="graph">邻接表</param>
        /// <returns>生成树边集</returns>
        List<SpanningTreeEdge> GenerateSpanningTree(int vertexCount, List<List<Adjacency>> graph);
    }

    /// <summary>
    /// 
    /// </summary>
    internal abstract class MazeAlgorithm : IMazeAlgorithm
    {
        protected Random random;

        protected MazeAlgorithm(Random random)
        {
            this.random = random;
        }

        public abstract EMazeAlgorithm Algorithm { get; }

        public abstract List<SpanningTreeEdge> GenerateSpanningTree(int vertexCount, List<List<Adjacency>> graph);
    }
}
