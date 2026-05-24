using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public struct CircularMazeField : IMazeField
    {
        private bool[][] innerWalls = null;
        private bool[][] radialWalls = null;
        private int[] sectorsPerRing = null;
        private int cachedCellCount = 0;

        public int rings { get; } = 20;
        public int sectors { get; } = 100;
        public SectorStrategy strategy { get; } = SectorStrategy.Arc;

        public int count => cachedCellCount;

        public CircularMazeField(int rings, int sectors)
            : this(rings, sectors, SectorStrategy.Arc)
        {
        }

        public CircularMazeField(int rings, int maxSectors, SectorStrategy strategy)
        {
            this.rings = Math.Max(1, rings);
            this.sectors = Math.Max(3, maxSectors);
            this.strategy = strategy;

            this.sectorsPerRing = new int[this.rings];

            var normalizedMaxSectors = 3;
            while (normalizedMaxSectors * 2 <= this.sectors)
            {
                normalizedMaxSectors *= 2;
            }

            if (strategy == SectorStrategy.Arc)
            {
                this.sectorsPerRing[0] = 3;
                for (var r = 1; r < this.rings; r++)
                {
                    this.sectorsPerRing[r] = this.sectorsPerRing[r - 1];
                    double arcLength = (2 * Math.PI * (r + 1)) / this.sectorsPerRing[r - 1];
                    if (arcLength > 2.0 && this.sectorsPerRing[r] * 2 <= normalizedMaxSectors)
                    {
                        this.sectorsPerRing[r] *= 2;
                    }
                }
            }
            else
            {
                this.sectorsPerRing[0] = 3;
                for (var r = 1; r < this.rings; r++)
                {
                    this.sectorsPerRing[r] = this.sectorsPerRing[r - 1];
                    double area = (Math.PI * (r + 1) * (r + 1)) / this.sectorsPerRing[r - 1];
                    if (area > 2.0 && this.sectorsPerRing[r] * 2 <= normalizedMaxSectors)
                    {
                        this.sectorsPerRing[r] *= 2;
                    }
                }
            }

            if (this.sectorsPerRing[this.rings - 1] < normalizedMaxSectors)
            {
                this.sectorsPerRing[this.rings - 1] = normalizedMaxSectors;
            }

            innerWalls = new bool[this.rings][];
            radialWalls = new bool[this.rings][];

            for (var r = 0; r < this.rings; r++)
            {
                var sectorsInRing = this.sectorsPerRing[r];
                radialWalls[r] = new bool[sectorsInRing];
                for (var s = 0; s < sectorsInRing; s++)
                {
                    radialWalls[r][s] = true;
                }

                if (r < this.rings - 1)
                {
                    var outerSectorsInRing = this.sectorsPerRing[r + 1];
                    innerWalls[r] = new bool[outerSectorsInRing];
                    for (var s = 0; s < outerSectorsInRing; s++)
                    {
                        innerWalls[r][s] = true;
                    }
                }
                else
                {
                    innerWalls[r] = Array.Empty<bool>();
                }
            }

            cachedCellCount = 0;
            for (var r = 0; r < this.rings; r++)
            {
                cachedCellCount += this.sectorsPerRing[r];
            }
        }

        public int GetSectorsInRing(int ring)
        {
            if (ring < 0 || ring >= rings)
                return 0;
            return sectorsPerRing[ring];
        }

        public int MapSector(int fromRing, int fromSector, int toRing)
        {
            var fromSectors = GetSectorsInRing(fromRing);
            var toSectors = GetSectorsInRing(toRing);
            return (fromSector * toSectors) / fromSectors;
        }

        public int GetNextSector(int ring, int sector)
        {
            var sectorsInRing = GetSectorsInRing(ring);
            return (sector + 1) % sectorsInRing;
        }

        public int GetPrevSector(int ring, int sector)
        {
            var sectorsInRing = GetSectorsInRing(ring);
            return (sector - 1 + sectorsInRing) % sectorsInRing;
        }

        public int GetTotalCells()
        {
            return cachedCellCount;
        }

        public bool GetInnerWall(int ring, int sector)
        {
            if (ring < 0 || ring >= rings - 1 || sector < 0 || sector >= GetSectorsInRing(ring + 1))
                return true;
            return innerWalls[ring][sector];
        }

        public void SetInnerWall(int ring, int sector, bool exists)
        {
            if (ring < 0 || ring >= rings - 1 || sector < 0 || sector >= GetSectorsInRing(ring + 1))
                return;
            innerWalls[ring][sector] = exists;
        }

        public bool GetRadialWall(int ring, int sector)
        {
            if (ring < 0 || ring >= rings || sector < 0 || sector >= GetSectorsInRing(ring))
                return true;
            return radialWalls[ring][sector];
        }

        public void SetRadialWall(int ring, int sector, bool exists)
        {
            if (ring < 0 || ring >= rings || sector < 0 || sector >= GetSectorsInRing(ring))
                return;
            radialWalls[ring][sector] = exists;
        }

        public int GetTileIndex(int ring, int sector)
        {
            if (ring < 0 || ring >= rings || sector < 0 || sector >= GetSectorsInRing(ring))
                throw new ArgumentOutOfRangeException();

            var index = 0;
            for (var r = 0; r < ring; r++)
            {
                index += sectorsPerRing[r];
            }
            return index + sector;
        }

        public int GetTileIndex(Tile tile)
        {
            return GetTileIndex(tile.lateral, tile.radial);
        }

        public Tile GetTileByIndex(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index));

            int remaining = index;
            for (int r = 0; r < rings; r++)
            {
                if (remaining < sectorsPerRing[r])
                    return new Tile(r, remaining);
                remaining -= sectorsPerRing[r];
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        public List<Tile> GetNeighbors(Tile tile)
        {
            int ring = tile.lateral;
            int sector = tile.radial;
            var neighbors = new List<Tile>(6);

            if (ring > 0)
            {
                int innerSector = MapSector(ring, sector, ring - 1);
                neighbors.Add(new Tile(ring - 1, innerSector));
            }

            if (ring < rings - 1)
            {
                int outerRing = ring + 1;
                int innerSectors = GetSectorsInRing(ring);
                int outerSectors = GetSectorsInRing(outerRing);
                int firstOuter = (sector * outerSectors) / innerSectors;
                int lastOuter = ((sector + 1) * outerSectors) / innerSectors;
                for (int os = firstOuter; os < lastOuter; os++)
                {
                    neighbors.Add(new Tile(outerRing, os));
                }
            }

            int leftSector = GetPrevSector(ring, sector);
            neighbors.Add(new Tile(ring, leftSector));

            int rightSector = GetNextSector(ring, sector);
            neighbors.Add(new Tile(ring, rightSector));

            return neighbors;
        }

        bool IMazeField.HasWallBetween(Tile a, Tile b)
        {
            int r1 = a.lateral, s1 = a.radial;
            int r2 = b.lateral, s2 = b.radial;

            if (r1 == r2)
            {
                int sectorsInRing = GetSectorsInRing(r1);
                if (Math.Abs(s1 - s2) == 1 || (s1 == 0 && s2 == sectorsInRing - 1) || (s2 == 0 && s1 == sectorsInRing - 1))
                {
                    int wallSector = Math.Min(s1, s2);
                    if (Math.Abs(s1 - s2) > 1)
                        wallSector = Math.Max(s1, s2);
                    return GetRadialWall(r1, wallSector);
                }
                return true;
            }

            int ringDiff = Math.Abs(r1 - r2);
            if (ringDiff == 1)
            {
                int innerRing = Math.Min(r1, r2);
                int outerSector = r1 > r2 ? s1 : s2;
                int innerSector = r1 > r2 ? s2 : s1;
                int mappedSector = MapSector(innerRing + 1, outerSector, innerRing);
                if (mappedSector == innerSector)
                    return GetInnerWall(innerRing, outerSector);
            }

            return true;
        }

        void IMazeField.RemoveWallBetween(Tile a, Tile b)
        {
            int r1 = a.lateral, s1 = a.radial;
            int r2 = b.lateral, s2 = b.radial;

            if (r1 == r2)
            {
                int sectorsInRing = GetSectorsInRing(r1);
                int wallSector = Math.Min(s1, s2);
                if (Math.Abs(s1 - s2) > 1)
                    wallSector = Math.Max(s1, s2);
                SetRadialWall(r1, wallSector, false);
            }
            else
            {
                int innerRing = Math.Min(r1, r2);
                int outerSector = r1 > r2 ? s1 : s2;
                SetInnerWall(innerRing, outerSector, false);
            }
        }

        public int rows => rings;

        public int GetRow(Tile tile)
        {
            return tile.lateral;
        }

        public List<Tile> GetTilesInRow(int row)
        {
            if (row < 0 || row >= rings)
                throw new ArgumentOutOfRangeException(nameof(row));
            var sectorsInRing = sectorsPerRing[row];
            var tiles = new List<Tile>(sectorsInRing);
            for (int s = 0; s < sectorsInRing; s++)
                tiles.Add(new Tile(row, s));
            return tiles;
        }
    }
}
