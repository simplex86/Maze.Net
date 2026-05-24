using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 基于 BFS 的迷宫生成算法
    /// </summary>
    internal class MazeBfsAlgorithm : IMazeAlgorithm
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

            var startTile = field.GetTileByIndex(random.Next(field.count));
            visited[field.GetTileIndex(startTile)] = true;

            var currentLevel = new List<Tile> { startTile };

            while (currentLevel.Count > 0)
            {
                var nextLevel = new List<(Tile parent, Tile child)>();

                foreach (var tile in currentLevel)
                {
                    foreach (var neighbor in field.GetNeighbors(tile))
                    {
                        if (!visited[field.GetTileIndex(neighbor)])
                        {
                            visited[field.GetTileIndex(neighbor)] = true;
                            nextLevel.Add((tile, neighbor));
                        }
                    }
                }

                nextLevel.Shuffle(random);

                foreach (var (parent, child) in nextLevel)
                {
                    field.RemoveWallBetween(parent, child);
                }

                currentLevel = new List<Tile>(nextLevel.Count);
                foreach (var (_, child) in nextLevel)
                {
                    currentLevel.Add(child);
                }
            }

            return field;
        }
    }
}
