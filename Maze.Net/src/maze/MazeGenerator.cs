using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    public class MazeGenerator
    {
        private Random random = null;
        private IMazeAlgorithm provider = null;

        /// <summary>
        /// 
        /// </summary>
        public MazeGenerator()
            : this(Random.Shared)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="random"></param>
        public MazeGenerator(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public MazeField Generate(MazeField field, EMazeAlgorithm algorithm)
        {
            algorithm = CheckSupportedAlgorithm(field.Shape, algorithm);

            if (provider == null || provider.Algorithm != algorithm)
            {
                provider = MazeAlgorithmFactory.CreateAlgorithm(algorithm, random);
            }

            var spanningTree = provider.GenerateSpanningTree(field.VertexCount, field.Graph);
            RemoveAdjacencyBorders(field.Graph, spanningTree);

            return field;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public async Task<MazeField> GenerateAsync(MazeField field, EMazeAlgorithm algorithm)
        {
            return await Task.Run(() => Generate(field, algorithm));
        }

        /// <summary>
        /// 矫正当前场地不支持的迷宫生成算法
        /// </summary>
        private EMazeAlgorithm CheckSupportedAlgorithm(EMazeShape shape, EMazeAlgorithm algorithm)
        {
            if (shape == EMazeShape.Circular        || 
                shape == EMazeShape.CircularHexagon ||
                shape == EMazeShape.Customized)
            {
                if (algorithm == EMazeAlgorithm.Eller) algorithm = EMazeAlgorithm.DFS;
            }

            return algorithm;
        }

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
