using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMazeField
    {
        int count { get; }

        int GetTileIndex(Tile tile);

        Tile GetTileByIndex(int index);

        List<Tile> GetNeighbors(Tile tile);

        bool HasWallBetween(Tile a, Tile b);

        void RemoveWallBetween(Tile a, Tile b);

        int rows { get; }

        int GetRow(Tile tile);

        List<Tile> GetTilesInRow(int row);
    }
}
