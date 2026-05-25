using System.Collections.Generic;
using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// 创建算法提供者
        /// </summary>
        /// <param name="algorithm">算法类型</param>
        /// <returns>算法提供者</returns>
        public static IMazeAlgorithm CreateAlgorithm(MazeAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case MazeAlgorithm.DFS:
                    return new MazeDfsAlgorithm();
                case MazeAlgorithm.BFS:
                    return new MazeBfsAlgorithm();
                case MazeAlgorithm.Prim:
                    return new MazePrimAlgorithm();
                case MazeAlgorithm.Kruskal:
                    return new MazeKruskalAlgorithm();
                case MazeAlgorithm.Wilson:
                    return new MazeWilsonAlgorithm();
                case MazeAlgorithm.Eller:
                    return new MazeEllerAlgorithm();
                case MazeAlgorithm.AldousBroder:
                    return new MazeAldousBroderAlgorithm();
                default:
                    break;
            }

            return new MazeDfsAlgorithm();
        }

        /// <summary>
        /// Fisher-Yates 洗牌算法
        /// </summary>
        public static void Shuffle<T>(this List<T> list, Random random)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
