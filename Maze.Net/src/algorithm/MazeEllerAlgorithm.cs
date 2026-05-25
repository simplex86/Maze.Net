using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 基于 Eller 的迷宫生成算法
    /// </summary>
    internal class MazeEllerAlgorithm : IMazeAlgorithm
    {
        private Random random = new Random();

        /// <summary>
        /// 算法
        /// </summary>
        public MazeAlgorithm algorithm => MazeAlgorithm.Eller;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IMazeField Create(IMazeField field)
        {
            var setOf = new int[field.count];
            int nextSetId = 1;

            for (int row = 0; row < field.rows; row++)
            {
                var tiles = field.GetTilesInRow(row);
                var isLastRow = (row == field.rows - 1);

                for (int i = 0; i < tiles.Count; i++)
                {
                    int idx = field.GetTileIndex(tiles[i]);
                    if (setOf[idx] == 0)
                        setOf[idx] = nextSetId++;
                }

                for (int i = 0; i < tiles.Count; i++)
                {
                    int nextI = (i + 1) % tiles.Count;
                    if (nextI == 0 && !isLastRow)
                        continue;

                    var tileA = tiles[i];
                    var tileB = tiles[nextI];

                    bool adjacent = false;
                    foreach (var n in field.GetNeighbors(tileA))
                    {
                        if (n.Equals(tileB)) { adjacent = true; break; }
                    }
                    if (!adjacent) continue;

                    int idxA = field.GetTileIndex(tileA);
                    int idxB = field.GetTileIndex(tileB);

                    if (setOf[idxA] != setOf[idxB] && (isLastRow || random.Next(2) == 0))
                    {
                        int oldSet = setOf[idxB];
                        int newSet = setOf[idxA];
                        for (int k = 0; k < setOf.Length; k++)
                        {
                            if (setOf[k] == oldSet)
                                setOf[k] = newSet;
                        }
                        field.RemoveWallBetween(tileA, tileB);
                    }
                }

                if (!isLastRow)
                {
                    var sets = new Dictionary<int, List<int>>();
                    for (int i = 0; i < tiles.Count; i++)
                    {
                        int idx = field.GetTileIndex(tiles[i]);
                        int root = setOf[idx];
                        if (!sets.ContainsKey(root))
                            sets[root] = new List<int>();
                        sets[root].Add(i);
                    }

                    var hasVertical = new bool[tiles.Count];
                    foreach (var members in sets.Values)
                    {
                        hasVertical[members[random.Next(members.Count)]] = true;
                    }

                    for (int i = 0; i < tiles.Count; i++)
                    {
                        if (!hasVertical[i] && random.Next(2) == 0)
                            hasVertical[i] = true;
                    }

                    for (int i = 0; i < tiles.Count; i++)
                    {
                        if (!hasVertical[i]) continue;

                        var tile = tiles[i];
                        foreach (var neighbor in field.GetNeighbors(tile))
                        {
                            if (field.GetRow(neighbor) == row + 1)
                            {
                                field.RemoveWallBetween(tile, neighbor);
                                int nIdx = field.GetTileIndex(neighbor);
                                setOf[nIdx] = setOf[field.GetTileIndex(tile)];
                                break;
                            }
                        }
                    }
                }
            }

            return field;
        }
    }
}
