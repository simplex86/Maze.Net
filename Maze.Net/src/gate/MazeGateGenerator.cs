using System;
using System.Collections.Generic;

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
    public class MazeGateGenerator
    {
        private Random random = null;

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
        /// 创建迷宫出入口（在边缘位置）
        /// </summary>
        /// <param name="field">迷宫场地</param>
        /// <param name="entrance">入口顶点索引</param>
        /// <param name="exit">出口顶点索引</param>
        public MazeGate Generate(MazeField field)
        {
            return Generate(field, EGatePosition.Edge, EGatePosition.Edge);
        }

        /// <summary>
        /// 创建迷宫出入口
        /// </summary>
        /// <param name="field">迷宫场地</param>
        /// <param name="entrancePosition">入口位置选项</param>
        /// <param name="exitPosition">出口位置选项</param>
        /// <param name="entrance">入口顶点索引</param>
        /// <param name="exit">出口顶点索引</param>
        public MazeGate Generate(MazeField field, EGatePosition entrancePosition, EGatePosition exitPosition)
        {
            // 出入口均在边缘时，使用各Field类型的对边约束逻辑
            if (entrancePosition == EGatePosition.Edge && exitPosition == EGatePosition.Edge)
            {
                return field.GenerateOppositeEdgeGate(random);
            }

            var vertices = FindEdgeVertices(field);

            var entranceCandidates = GetCandidates(field, vertices, entrancePosition);
            var exitCandidates = GetCandidates(field, vertices, exitPosition);

            var entrance = entranceCandidates[random.Next(entranceCandidates.Count)];
            if (exitCandidates.Count > 1) exitCandidates.Remove(entrance);
            var exit = exitCandidates[random.Next(exitCandidates.Count)];

            return new MazeGate(entrance, exit);
        }

        /// <summary>
        /// 查找所有边缘顶点（至少有一条边界边的顶点）
        /// </summary>
        private static List<int> FindEdgeVertices(MazeField field)
        {
            var list = new List<int>();
            for (int v = 0; v < field.Count; v++)
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

            var list = new List<int>(field.Count);
            for (int i = 0; i < field.Count; i++)
                list.Add(i);

            return list;
        }
    }
}
