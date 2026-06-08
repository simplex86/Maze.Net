using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 从指定顶点的朝外墙壁中随机选择一个边框
        /// </summary>
        protected IMazeBorder? PickOuterBorder(TField field, int vertex)
        {
            var candidates = new List<IMazeBorder>();
            foreach (var edge in field.Graph[vertex])
            {
                if (edge.Neighbor == -1 && edge.Border != null)
                    candidates.Add(edge.Border);
            }
            return candidates.Count > 0 ? candidates[random.Next(candidates.Count)] : null;
        }
    }
}
