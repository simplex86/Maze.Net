using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 基于 Aldous-Broder 的迷宫生成算法
    /// </summary>
    internal class MazeAldousBroderAlgorithm : IMazeAlgorithm
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

            var current = field.GetTileByIndex(random.Next(field.count));
            visited[field.GetTileIndex(current)] = true;
            int visitedCount = 1;

            while (visitedCount < field.count)
            {
                var neighbors = field.GetNeighbors(current);
                var next = neighbors[random.Next(neighbors.Count)];

                if (!visited[field.GetTileIndex(next)])
                {
                    field.RemoveWallBetween(current, next);
                    visited[field.GetTileIndex(next)] = true;
                    visitedCount++;
                }

                current = next;
            }

            return field;
        }
    }
}
