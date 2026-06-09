using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫出入口生成器。
    /// 使用 BFS 最远点法：收集所有边界顶点（有 Neighbor == -1 外墙的顶点），
    /// 随机选入口，BFS 找图论距离最远的边界顶点作为出口。
    /// </summary>
    public class MazeGateGenerator
    {
        private Random random;

        /// <summary>
        /// 迷宫出入口生成器（使用默认随机数生成器）
        /// </summary>
        public MazeGateGenerator()
            : this(Random.Shared)
        {
        }

        /// <summary>
        /// 迷宫出入口生成器
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
        public MazeGate Generate(MazeField field)
        {
            var borderVertices = CollectBorderVertices(field);

            if (borderVertices.Count == 0)
                return new MazeGate();

            if (borderVertices.Count == 1)
            {
                var v = borderVertices[0];
                return new MazeGate(v, v)
                {
                    EntranceBorder = PickOuterBorder(field, v),
                    ExitBorder = PickOuterBorder(field, v)
                };
            }

            var entrance = borderVertices[random.Next(borderVertices.Count)];
            var exit = FindFarthestBorderVertex(field, entrance, borderVertices);

            return new MazeGate(entrance, exit)
            {
                EntranceBorder = PickOuterBorder(field, entrance),
                ExitBorder = PickOuterBorder(field, exit)
            };
        }

        /// <summary>
        /// 创建迷宫出入口（异步）
        /// </summary>
        /// <param name="field">迷宫场地</param>
        public async Task<MazeGate> GenerateAsync(MazeField field)
        {
            var gate = await Task.Run(() => Generate(field));
            return gate;
        }

        /// <summary>
        /// 收集所有边界顶点（至少有一面 Neighbor == -1 外墙的顶点）
        /// </summary>
        private static List<int> CollectBorderVertices(MazeField field)
        {
            var borderVertices = new List<int>();

            for (int v = 0; v < field.VertexCount; v++)
            {
                foreach (var edge in field.Graph[v])
                {
                    if (edge.Neighbor == -1)
                    {
                        borderVertices.Add(v);
                        break;
                    }
                }
            }

            return borderVertices;
        }

        /// <summary>
        /// 从指定顶点做 BFS，在边界顶点中找到图论距离最远的一个
        /// </summary>
        private static int FindFarthestBorderVertex(MazeField field, int start, List<int> borderVertices)
        {
            var borderSet = new HashSet<int>(borderVertices);
            var distance = new int[field.VertexCount];
            for (int i = 0; i < distance.Length; i++)
                distance[i] = -1;

            distance[start] = 0;
            var queue = new Queue<int>();
            queue.Enqueue(start);

            int farthestVertex = start;
            int maxDistance = 0;

            while (queue.Count > 0)
            {
                var v = queue.Dequeue();

                foreach (var edge in field.Graph[v])
                {
                    if (edge.Neighbor >= 0 && distance[edge.Neighbor] < 0)
                    {
                        distance[edge.Neighbor] = distance[v] + 1;
                        queue.Enqueue(edge.Neighbor);

                        if (borderSet.Contains(edge.Neighbor) && distance[edge.Neighbor] > maxDistance)
                        {
                            maxDistance = distance[edge.Neighbor];
                            farthestVertex = edge.Neighbor;
                        }
                    }
                }
            }

            return farthestVertex;
        }

        /// <summary>
        /// 从指定顶点的朝外墙壁中随机选择一个边框
        /// </summary>
        private IMazeBorder? PickOuterBorder(MazeField field, int vertex)
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
