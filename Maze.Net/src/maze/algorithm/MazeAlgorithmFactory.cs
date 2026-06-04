using System.Collections.Generic;
using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫生成算法工厂类
    /// </summary>
    internal static class MazeAlgorithmFactory
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
                case EMazeAlgorithm.HuntAndKill:
                    return new MazeHuntAndKillAlgorithm(random);
                default:
                    break;
            }

            return new MazeDfsAlgorithm(random);
        }
    }
}
