using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 出入口位置
    /// </summary>
    public enum EGatePosition
    {
        /// <summary>
        /// 任意位置
        /// </summary>
        Any = 1,
        /// <summary>
        /// 边缘上
        /// </summary>
        Edge = 2,
    }

    /// <summary>
    /// 迷宫出入口
    /// </summary>
    public abstract class GateGenerator<TField> where TField : IMazeField
    {
        protected Random random = null;

        /// <summary>
        /// 迷宫出入口（使用默认随机数生成器）
        /// </summary>
        public GateGenerator()
            : this(Random.Shared)
        {

        }

        /// <summary>
        /// 迷宫出入口
        /// </summary>
        /// <param name="random">随机数生成器</param>
        public GateGenerator(Random random)
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
        /// 查找所有边缘顶点（至少有一条边界边的顶点）
        /// </summary>
        private static List<int> FindEdgeVertices(MazeField field)
        {
            var list = new List<int>();
            for (int v = 0; v < field.VertexCount; v++)
            {
                foreach (var edge in field.Graph[v])
                {
                    if (edge.Neighbor == -1)
                    {
                        list.Add(v);
                        break;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 根据位置选项获取候选顶点列表
        /// </summary>
        private static List<int> GetCandidates(MazeField field, List<int> edgeVertices, EGatePosition position)
        {
            if (position == EGatePosition.Edge)
                return new List<int>(edgeVertices);

            var list = new List<int>(field.VertexCount);
            for (int i = 0; i < field.VertexCount; i++)
                list.Add(i);

            return list;
        }
    }
}
