using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于Wilson算法生成随机迷宫
    /// Wilson算法特点：使用随机游走，生成的迷宫具有均匀的随机性
    /// </summary>
    public class RectangularMazeWilsonProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        private IMazeAlgorithm wilson = new MazeWilsonAlgorithm();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Wilson;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);
            return (RectangularMazeField)wilson.Create(field);
        }
    }
}
