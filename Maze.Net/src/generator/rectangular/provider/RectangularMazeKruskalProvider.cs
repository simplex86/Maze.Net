using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于Kruskal最小生成树算法生成随机迷宫
    /// </summary>
    public class RectangularMazeKruskalProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        private IMazeAlgorithm kruskal = new MazeKruskalAlgorithm();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Kruskal;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);
            return (RectangularMazeField)kruskal.Create(field);
        }
    }
}