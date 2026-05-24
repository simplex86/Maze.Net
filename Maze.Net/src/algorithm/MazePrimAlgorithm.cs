using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 基于 PRIM 的迷宫生成算法
    /// </summary>
    internal class MazePrimAlgorithm : IMazeAlgorithm
    {
        private Random random = new Random();

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IMazeField Create(IMazeField field)
        {
            var visited = new bool[field.count];
            var frontier = new List<(Tile from, Tile to)>();

            var startTile = field.GetTileByIndex(random.Next(field.count));
            visited[field.GetTileIndex(startTile)] = true;
            AddFrontier(field, visited, frontier, startTile);

            while (frontier.Count > 0)
            {
                var idx = random.Next(frontier.Count);
                var (from, to) = frontier[idx];
                frontier.RemoveAt(idx);

                if (visited[field.GetTileIndex(to)])
                    continue;

                field.RemoveWallBetween(from, to);
                visited[field.GetTileIndex(to)] = true;
                AddFrontier(field, visited, frontier, to);
            }

            return field;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="visited"></param>
        /// <param name="frontier"></param>
        /// <param name="tile"></param>
        private void AddFrontier(IMazeField field, bool[] visited, List<(Tile from, Tile to)> frontier, Tile tile)
        {
            foreach (var neighbor in field.GetNeighbors(tile))
            {
                if (!visited[field.GetTileIndex(neighbor)])
                    frontier.Add((tile, neighbor));
            }
        }
    }
}
