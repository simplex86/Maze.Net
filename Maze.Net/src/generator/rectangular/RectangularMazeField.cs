using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public struct RectangularMazeField : IMazeField
    {
        private bool[][] horizontalWalls = null;
        private bool[][] verticalWalls = null;

        public int width { get; private set; } = 10;
        public int height { get; private set; } = 10;

        public int count => width * height;

        public RectangularMazeField(int width, int height)
        {
            this.width = Math.Max(1, width);
            this.height = Math.Max(1, height);

            horizontalWalls = new bool[this.height + 1][];
            verticalWalls = new bool[this.height][];

            for (int y = 0; y <= this.height; y++)
            {
                if (y < this.height)
                {
                    horizontalWalls[y] = new bool[this.width];
                    verticalWalls[y] = new bool[this.width + 1];
                    for (int x = 0; x <= this.width; x++)
                    {
                        if (x < this.width)
                        {
                            horizontalWalls[y][x] = true;
                        }
                        verticalWalls[y][x] = true;
                    }
                }
                else
                {
                    horizontalWalls[y] = new bool[this.width];
                    for (int x = 0; x < this.width; x++)
                    {
                        horizontalWalls[y][x] = true;
                    }
                }
            }
        }

        public bool GetHorizontalWall(int x, int y)
        {
            if (y == 0 || y == height)
                return true;
            if (x < 0 || x >= width)
                return true;
            return horizontalWalls[y][x];
        }

        public void SetHorizontalWall(int x, int y, bool exists)
        {
            if (y == 0 || y == height)
                return;
            if (x < 0 || x >= width)
                return;
            horizontalWalls[y][x] = exists;
        }

        public bool GetVerticalWall(int x, int y)
        {
            if (x == 0 || x == width)
                return true;
            if (y < 0 || y >= height)
                return true;
            return verticalWalls[y][x];
        }

        public void SetVerticalWall(int x, int y, bool exists)
        {
            if (x == 0 || x == width)
                return;
            if (y < 0 || y >= height)
                return;
            verticalWalls[y][x] = exists;
        }

        public bool HasWallBetween(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2)
            {
                int y = Math.Max(y1, y2);
                return GetHorizontalWall(x1, y);
            }
            else if (y1 == y2)
            {
                int x = Math.Max(x1, x2);
                return GetVerticalWall(x, y1);
            }
            return true;
        }

        public void RemoveWallBetween(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2)
            {
                int y = Math.Max(y1, y2);
                SetHorizontalWall(x1, y, false);
            }
            else if (y1 == y2)
            {
                int x = Math.Max(x1, x2);
                SetVerticalWall(x, y1, false);
            }
        }

        public int GetTileIndex(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                throw new System.ArgumentOutOfRangeException();
            return y * width + x;
        }

        public int GetTileIndex(Tile tile)
        {
            return GetTileIndex(tile.lateral, tile.radial);
        }

        public Tile GetTileByIndex(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new Tile(index % width, index / width);
        }

        public List<Tile> GetNeighbors(Tile tile)
        {
            int x = tile.lateral;
            int y = tile.radial;
            var neighbors = new List<Tile>(4);

            if (y > 0)
                neighbors.Add(new Tile(x, y - 1));
            if (y < height - 1)
                neighbors.Add(new Tile(x, y + 1));
            if (x > 0)
                neighbors.Add(new Tile(x - 1, y));
            if (x < width - 1)
                neighbors.Add(new Tile(x + 1, y));

            return neighbors;
        }

        bool IMazeField.HasWallBetween(Tile a, Tile b)
        {
            return HasWallBetween(a.lateral, a.radial, b.lateral, b.radial);
        }

        void IMazeField.RemoveWallBetween(Tile a, Tile b)
        {
            RemoveWallBetween(a.lateral, a.radial, b.lateral, b.radial);
        }

        public int rows => height;

        public int GetRow(Tile tile)
        {
            return tile.radial;
        }

        public List<Tile> GetTilesInRow(int row)
        {
            if (row < 0 || row >= height)
                throw new ArgumentOutOfRangeException(nameof(row));
            var tiles = new List<Tile>(width);
            for (int x = 0; x < width; x++)
                tiles.Add(new Tile(x, row));
            return tiles;
        }
    }
}
