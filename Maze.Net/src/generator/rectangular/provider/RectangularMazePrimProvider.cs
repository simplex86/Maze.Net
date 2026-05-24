using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于PRIM算法
    /// </summary>
    public class RectangularMazePrimProvider : IRectangularMazeProvider
    {
        // 随机数
        private Random random = new Random();

        private IMazeAlgorithm prim = new MazePrimAlgorithm();

        /// <summary>
        /// 
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Prim;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <returns></returns>
        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);
            return (RectangularMazeField)prim.Create(field);
        }
    }
}