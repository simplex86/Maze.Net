using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public abstract class MazeGenerator<TField> where TField : IMazeField
    {
        private Random random = null;
        private IMazeAlgorithm provider = null;

        protected MazeGenerator()
            : this(Random.Shared)
        {
        }

        protected MazeGenerator(Random random)
        {
            this.random = random;
        }

        protected TField Generate(TField field, EMazeAlgorithm algorithm)
        {
            if (!IsAlgorithmSupported(algorithm))
                algorithm = EMazeAlgorithm.DFS;

            if (provider == null || provider.Algorithm != algorithm)
            {
                provider = MazeAlgorithmFactory.CreateAlgorithm(algorithm, random);
            }

            var spanningTree = provider.GenerateSpanningTree(field.VertexCount, field.Graph);
            RemoveAdjacencyBorders(field.Graph, spanningTree);

            return field;
        }

        /// <summary>
        /// 判断当前场地是否支持指定的迷宫生成算法（默认支持所有算法）
        /// </summary>
        protected virtual bool IsAlgorithmSupported(EMazeAlgorithm algorithm) => true;

        /// <summary>
        /// 根据生成树边集移除邻接表的边界
        /// </summary>
        private void RemoveAdjacencyBorders(List<List<Adjacency>> graph, List<SpanningTreeEdge> spanningTree)
        {
            foreach (var edge in spanningTree)
            {
                for (int i = 0; i < graph[edge.U].Count; i++)
                {
                    if (graph[edge.U][i].Neighbor == edge.V)
                    {
                        graph[edge.U][i].IsOpen = true;
                        break;
                    }
                }
                for (int i = 0; i < graph[edge.V].Count; i++)
                {
                    if (graph[edge.V][i].Neighbor == edge.U)
                    {
                        graph[edge.V][i].IsOpen = true;
                        break;
                    }
                }
            }
        }
    }
}
