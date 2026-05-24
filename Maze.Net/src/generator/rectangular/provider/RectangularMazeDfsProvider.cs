using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于深度优先搜索算法生成随机迷宫
    /// </summary>
    public class RectangularMazeDfsProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        private IMazeAlgorithm dfs = new MazeDfsAlgorithm();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.DFS;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度（格子数量）</param>
        /// <param name="height">迷宫高度（格子数量）</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);
            return (RectangularMazeField)dfs.Create(field);
        }
    }
}
