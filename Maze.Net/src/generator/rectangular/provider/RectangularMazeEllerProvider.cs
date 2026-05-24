using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于Eller算法生成随机迷宫
    /// </summary>
    public class RectangularMazeEllerProvider : IRectangularMazeProvider
    {
        private Random random = new Random();

        private IMazeAlgorithm eller = new MazeEllerAlgorithm();

        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Eller;

        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);
            return (RectangularMazeField)eller.Create(field);
        }
    }
}