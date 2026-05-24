using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于Aldous-Broder算法生成随机迷宫：最简单的随机迷宫算法，使用纯粹的随机游走
    /// </summary>
    public class CircularMazeAldousBroderProvider : ICircularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        private IMazeAlgorithm broder = new MazeAldousBroderAlgorithm();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.AldousBroder;

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
            return (CircularMazeField)broder.Create(field);
        }
    }
}
