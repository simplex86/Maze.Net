using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 基于 Wilson 的迷宫生成算法
    /// </summary>
    internal class MazeWilsonAlgorithm : IMazeAlgorithm
    {
        private Random random = new Random();

        /// <summary>
        /// 算法
        /// </summary>
        public MazeAlgorithm algorithm => MazeAlgorithm.Wilson;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IMazeField Create(IMazeField field)
        {
            var visited = new bool[field.count];

            var startTile = field.GetTileByIndex(random.Next(field.count));
            visited[field.GetTileIndex(startTile)] = true;
            int visitedCount = 1;

            while (visitedCount < field.count)
            {
                var walkStart = PickUnvisited(field, visited);
                var path = RandomWalkToVisited(field, visited, walkStart);

                for (int i = 0; i < path.Count - 1; i++)
                {
                    field.RemoveWallBetween(path[i], path[i + 1]);
                    if (!visited[field.GetTileIndex(path[i])])
                    {
                        visited[field.GetTileIndex(path[i])] = true;
                        visitedCount++;
                    }
                }

                var last = path[path.Count - 1];
                if (!visited[field.GetTileIndex(last)])
                {
                    visited[field.GetTileIndex(last)] = true;
                    visitedCount++;
                }
            }

            return field;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="visited"></param>
        /// <returns></returns>
        private Tile PickUnvisited(IMazeField field, bool[] visited)
        {
            Tile tile;
            do
            {
                tile = field.GetTileByIndex(random.Next(field.count));
            } while (visited[field.GetTileIndex(tile)]);

            return tile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="visited"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        private List<Tile> RandomWalkToVisited(IMazeField field, bool[] visited, Tile start)
        {
            var direction = new Dictionary<int, Tile>();
            var current = start;

            while (!visited[field.GetTileIndex(current)])
            {
                var neighbors = field.GetNeighbors(current);
                var next = neighbors[random.Next(neighbors.Count)];
                direction[field.GetTileIndex(current)] = next;
                current = next;
            }

            var path = new List<Tile>();
            var trace = start;
            path.Add(trace);
            while (!visited[field.GetTileIndex(trace)])
            {
                trace = direction[field.GetTileIndex(trace)];
                path.Add(trace);
            }

            return path;
        }
    }
}
