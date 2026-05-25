using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 基于 Kruskal 的迷宫生成算法
    /// </summary>
    internal class MazeKruskalAlgorithm : IMazeAlgorithm
    {
        private Random random = new Random();

        /// <summary>
        /// 算法
        /// </summary>
        public MazeAlgorithm algorithm => MazeAlgorithm.Kruskal;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IMazeField Create(IMazeField field)
        {
            var edges = CollectEdges(field);
            edges.Shuffle(random);

            var dsu = new DisjointSet(field.count);

            foreach (var (a, b) in edges)
            {
                int idxA = field.GetTileIndex(a);
                int idxB = field.GetTileIndex(b);

                if (dsu.Union(idxA, idxB))
                {
                    field.RemoveWallBetween(a, b);
                    if (dsu.Count == 1) break;
                }
            }

            return field;
        }

        /// <summary>
        /// 收集边
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private List<(Tile a, Tile b)> CollectEdges(IMazeField field)
        {
            var edges = new List<(Tile a, Tile b)>();

            for (int i = 0; i < field.count; i++)
            {
                var tile = field.GetTileByIndex(i);
                foreach (var neighbor in field.GetNeighbors(tile))
                {
                    int j = field.GetTileIndex(neighbor);
                    if (j > i)
                        edges.Add((tile, neighbor));
                }
            }

            return edges;
        }
    }
}
