using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class CircularMazeBfsProvider : ICircularMazeProvider
    {
        private Random random = new Random();

        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.BFS;

        public CircularMazeField Create(int rings, int sectors)
        {
            return Create(rings, sectors, SectorStrategy.Each);
        }

        public CircularMazeField Create(int rings, int sectors, SectorStrategy strategy)
        {
            var field = new CircularMazeField(rings, sectors, strategy);

            var visited = new bool[field.rings][];
            for (int r = 0; r < field.rings; r++)
            {
                visited[r] = new bool[field.GetSectorsInRing(r)];
            }

            int startRing = random.Next(field.rings);
            int startSector = random.Next(field.GetSectorsInRing(startRing));
            visited[startRing][startSector] = true;

            var queue = new List<Tile>();
            queue.Add(new Tile(startRing, startSector));

            while (queue.Count > 0)
            {
                int index = random.Next(queue.Count);
                var tile = queue[index];
                queue.RemoveAt(index);

                var neighbors = GetNeighborPositions(field, tile.lateral, tile.radial);
                Shuffle(neighbors);

                foreach (var (nr, ns) in neighbors)
                {
                    if (!visited[nr][ns])
                    {
                        visited[nr][ns] = true;
                        RemoveWall(field, tile.lateral, tile.radial, nr, ns);
                        queue.Add(new Tile(nr, ns));
                    }
                }
            }

            return field;
        }

        private List<(int ring, int sector)> GetNeighborPositions(CircularMazeField field, int ring, int sector)
        {
            var neighbors = new List<(int, int)>();

            if (ring > 0)
            {
                int innerRing = ring - 1;
                int innerSector = field.MapSector(ring, sector, innerRing);
                neighbors.Add((innerRing, innerSector));
            }

            if (ring < field.rings - 1)
            {
                int outerRing = ring + 1;
                int innerSectors = field.GetSectorsInRing(ring);
                int outerSectors = field.GetSectorsInRing(outerRing);
                int firstOuter = (sector * outerSectors) / innerSectors;
                int lastOuter = ((sector + 1) * outerSectors) / innerSectors;
                for (int os = firstOuter; os < lastOuter; os++)
                {
                    neighbors.Add((outerRing, os));
                }
            }

            int leftSector = field.GetPrevSector(ring, sector);
            neighbors.Add((ring, leftSector));

            int rightSector = field.GetNextSector(ring, sector);
            neighbors.Add((ring, rightSector));

            return neighbors;
        }

        private void RemoveWall(CircularMazeField field, int ring1, int sector1, int ring2, int sector2)
        {
            if (ring1 == ring2)
            {
                int ring = ring1;
                int wallSector = Math.Min(sector1, sector2);
                if (Math.Abs(sector1 - sector2) > 1)
                {
                    wallSector = Math.Max(sector1, sector2);
                }
                field.SetRadialWall(ring, wallSector, false);
            }
            else
            {
                int innerRing = Math.Min(ring1, ring2);
                int outerSector = (ring1 > ring2) ? sector1 : sector2;
                field.SetInnerWall(innerRing, outerSector, false);
            }
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
