using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫出入口
    /// </summary>
    public abstract class MazeGateGenerator<TField> where TField : IMazeField
    {
        protected Random random = null;

        /// <summary>
        /// 迷宫出入口（使用默认随机数生成器）
        /// </summary>
        public MazeGateGenerator()
            : this(Random.Shared)
        {

        }

        /// <summary>
        /// 迷宫出入口
        /// </summary>
        /// <param name="random">随机数生成器</param>
        public MazeGateGenerator(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// 创建迷宫出入口
        /// </summary>
        /// <param name="field">迷宫场地</param>
        /// <returns></returns>
        public abstract MazeGate Generate(TField field);

        /// <summary>
        /// 创建迷宫出入口（异步）
        /// </summary>
        /// <param name="field">迷宫场地</param>
        public async Task<MazeGate> GenerateAsync(TField field)
        {
            var gate = await Task.Run(() => Generate(field));
            return gate;
        }
    }
}
