using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于Prim算法生成随机迷宫
    /// </summary>
    public class CircularMazePrimProvider : ICircularMazeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        private Random random = new Random();

        private IMazeAlgorithm prim = new MazePrimAlgorithm();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Prim;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        public CircularMazeField Create(int rings, int sectors)
        {
            return Create(rings, sectors, SectorStrategy.Arc);
        }

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="rings">圈数</param>
        /// <param name="sectors">最大扇形数（最外圈）</param>
        /// <param name="strategy">扇形分割策略（可选）</param>
        public CircularMazeField Create(int rings, int sectors, SectorStrategy strategy)
        {
            var field = new CircularMazeField(rings, sectors, strategy);
            return (CircularMazeField)prim.Create(field);
        }
    }
}
