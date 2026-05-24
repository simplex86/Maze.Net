using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于BFS算法生成随机迷宫
    /// </summary>
    public class CircularMazeBfsProvider : ICircularMazeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        private Random random = new Random();
        private IMazeAlgorithm bfs = new MazeBfsAlgorithm();
        /// <summary>
        /// 
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.BFS;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rings"></param>
        /// <param name="sectors"></param>
        /// <returns></returns>
        public CircularMazeField Create(int rings, int sectors)
        {
            return Create(rings, sectors, SectorStrategy.Arc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rings"></param>
        /// <param name="sectors"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public CircularMazeField Create(int rings, int sectors, SectorStrategy strategy)
        {
            var field = new CircularMazeField(rings, sectors, strategy);
            return (CircularMazeField)bfs.Create(field);
        }
    }
}
