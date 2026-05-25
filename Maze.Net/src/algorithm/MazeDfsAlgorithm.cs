using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 基于DFS的迷宫生成算法
    /// </summary>
    internal class MazeDfsAlgorithm : IMazeAlgorithm
    {
        private Random random = new Random();

        /// <summary>
        /// 算法
        /// </summary>
        public MazeAlgorithm algorithm => MazeAlgorithm.DFS;

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

            var stack = new Stack<Tile>();
            stack.Push(startTile);

            while (stack.Count > 0)
            {
                var current = stack.Peek();
                var unvisited = GetUnvisitedNeighbors(field, visited, current);

                if (unvisited.Count > 0)
                {
                    var next = unvisited[random.Next(unvisited.Count)];
                    field.RemoveWallBetween(current, next);
                    visited[field.GetTileIndex(next)] = true;
                    stack.Push(next);
                }
                else
                {
                    stack.Pop();
                }
            }

            return field;
        }

        /// <summary>
        /// 获取为访问的邻居
        /// </summary>
        /// <param name="field"></param>
        /// <param name="visited"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        private List<Tile> GetUnvisitedNeighbors(IMazeField field, bool[] visited, Tile tile)
        {
            var unvisited = new List<Tile>();
            foreach (var neighbor in field.GetNeighbors(tile))
            {
                if (!visited[field.GetTileIndex(neighbor)]) 
                    unvisited.Add(neighbor);
            }
            return unvisited;
        }
    }
}
