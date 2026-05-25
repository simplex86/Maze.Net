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
        public static IMazeAlgorithm CreateAlgorithm(MazeAlgorithm algorithm, Random random)
        {
            switch (algorithm)
            {
                case MazeAlgorithm.DFS:
                    return new MazeDfsAlgorithm(random);
                case MazeAlgorithm.BFS:
                    return new MazeBfsAlgorithm(random);
                case MazeAlgorithm.Prim:
                    return new MazePrimAlgorithm(random);
                case MazeAlgorithm.Kruskal:
                    return new MazeKruskalAlgorithm(random);
                case MazeAlgorithm.Wilson:
                    return new MazeWilsonAlgorithm(random);
                case MazeAlgorithm.Eller:
                    return new MazeEllerAlgorithm(random);
                case MazeAlgorithm.AldousBroder:
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
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
