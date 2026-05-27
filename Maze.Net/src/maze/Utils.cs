using System.Collections.Generic;
using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 工具类
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// 创建算法提供者
        /// </summary>
        /// <param name="algorithm">算法类型</param>
        /// <returns>算法提供者</returns>
        public static IMazeAlgorithm CreateAlgorithm(EMazeAlgorithm algorithm, Random random)
        {
            switch (algorithm)
            {
                case EMazeAlgorithm.DFS:
                    return new MazeDfsAlgorithm(random);
                case EMazeAlgorithm.BFS:
                    return new MazeBfsAlgorithm(random);
                case EMazeAlgorithm.Prim:
                    return new MazePrimAlgorithm(random);
                case EMazeAlgorithm.Kruskal:
                    return new MazeKruskalAlgorithm(random);
                case EMazeAlgorithm.Wilson:
                    return new MazeWilsonAlgorithm(random);
                case EMazeAlgorithm.Eller:
                    return new MazeEllerAlgorithm(random);
                case EMazeAlgorithm.AldousBroder:
                    return new MazeAldousBroderAlgorithm(random);
                default:
                    break;
            }

            return new MazeDfsAlgorithm(random);
        }

        /// <summary>
        /// Fisher-Yates 洗牌算法
        /// </summary>
        public static void Shuffle<T>(this List<T> list, Random random)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

        /// <summary>
        /// 根据生成树边集移除邻接表的边界
        /// </summary>
        public static void RemoveAdjacencyBorders(List<List<Adjacency>> graph, List<SpanningTreeEdge> spanningTree)
        {
            foreach (var edge in spanningTree)
            {
                for (int i = 0; i < graph[edge.u].Count; i++)
                {
                    if (graph[edge.u][i].Neighbor == edge.v)
                    {
                        graph[edge.u][i].Border = null;
                        break;
                    }
                }
                for (int i = 0; i < graph[edge.v].Count; i++)
                {
                    if (graph[edge.v][i].Neighbor == edge.u)
                    {
                        graph[edge.v][i].Border = null;
                        break;
                    }
                }
            }
        }
    }
}
