using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于广度优先搜索算法生成随机迷宫
    /// </summary>
    public class RectangularMazeBfsProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        private IMazeAlgorithm bfs = new MazeBfsAlgorithm();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.BFS;

        // 用于临时存储邻居信息的结构体
        private struct NeighborInfo
        {
            public int x;
            public int y;
            public int parentX;
            public int parentY;

            public NeighborInfo(int x, int y, int parentX, int parentY)
            {
                this.x = x;
                this.y = y;
                this.parentX = parentX;
                this.parentY = parentY;
            }
        }

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularMazeField Create(int width, int height)
        {
            var field = new RectangularMazeField(width, height);
            return (RectangularMazeField)bfs.Create(field);
        }
    }
}
