using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫场地（邻接表方案）
    /// </summary>
    public class CircularMazeField : IMazeField
    {
        private readonly List<List<Edge>> _graph;
        private readonly int[] _sectorsPerRing;
        private readonly int _cachedCellCount;

        public int rings { get; }
        public int sectors { get; }
        public SectorStrategy strategy { get; }
        public int count => _cachedCellCount;
        public List<List<Edge>> graph => _graph;
        public int rows => rings;

        public CircularMazeField(int rings, int sectors)
            : this(rings, sectors, SectorStrategy.Arc)
        {
        }

        public CircularMazeField(int rings, int maxSectors, SectorStrategy strategy)
        {
            this.rings = Math.Max(1, rings);
            this.sectors = Math.Max(3, maxSectors);
            this.strategy = strategy;

            _sectorsPerRing = new int[this.rings];

            var normalizedMaxSectors = 3;
            while (normalizedMaxSectors * 2 <= this.sectors)
                normalizedMaxSectors *= 2;

            if (strategy == SectorStrategy.Arc)
            {
                _sectorsPerRing[0] = 3;
                for (var r = 1; r < this.rings; r++)
                {
                    _sectorsPerRing[r] = _sectorsPerRing[r - 1];
                    double arcLength = (2 * Math.PI * (r + 1)) / _sectorsPerRing[r - 1];
                    if (arcLength > 2.0 && _sectorsPerRing[r] * 2 <= normalizedMaxSectors)
                        _sectorsPerRing[r] *= 2;
                }
            }
            else
            {
                _sectorsPerRing[0] = 3;
                for (var r = 1; r < this.rings; r++)
                {
                    _sectorsPerRing[r] = _sectorsPerRing[r - 1];
                    double area = (Math.PI * (r + 1) * (r + 1)) / _sectorsPerRing[r - 1];
                    if (area > 2.0 && _sectorsPerRing[r] * 2 <= normalizedMaxSectors)
                        _sectorsPerRing[r] *= 2;
                }
            }

            if (_sectorsPerRing[this.rings - 1] < normalizedMaxSectors)
                _sectorsPerRing[this.rings - 1] = normalizedMaxSectors;

            _cachedCellCount = 0;
            for (var r = 0; r < this.rings; r++)
                _cachedCellCount += _sectorsPerRing[r];

            _graph = BuildGraph();
        }

        public int GetSectorsInRing(int ring)
        {
            if (ring < 0 || ring >= rings)
                return 0;
            return _sectorsPerRing[ring];
        }

        private int VertexIndex(int ring, int sector)
        {
            var index = 0;
            for (var r = 0; r < ring; r++)
                index += _sectorsPerRing[r];
            return index + sector;
        }

        private List<List<Edge>> BuildGraph()
        {
            var g = new List<List<Edge>>(_cachedCellCount);

            for (int r = 0; r < rings; r++)
            {
                int n = _sectorsPerRing[r];
                double angleStep = 2 * Math.PI / n;

                for (int s = 0; s < n; s++)
                {
                    var edges = new List<Edge>();

                    // 右邻居（同一环，下一个扇区）
                    int rightSector = (s + 1) % n;
                    int rightNeighbor = VertexIndex(r, rightSector);
                    double rightAngle = (s + 1) * angleStep - Math.PI / 2;
                    edges.Add(new Edge(rightNeighbor, new LineBorder(
                        r * Math.Cos(rightAngle), r * Math.Sin(rightAngle),
                        (r + 1) * Math.Cos(rightAngle), (r + 1) * Math.Sin(rightAngle))));

                    // 左邻居（同一环，上一个扇区）
                    int leftSector = (s - 1 + n) % n;
                    int leftNeighbor = VertexIndex(r, leftSector);
                    double leftAngle = s * angleStep - Math.PI / 2;
                    edges.Add(new Edge(leftNeighbor, new LineBorder(
                        r * Math.Cos(leftAngle), r * Math.Sin(leftAngle),
                        (r + 1) * Math.Cos(leftAngle), (r + 1) * Math.Sin(leftAngle))));

                    // 内邻居
                    if (r > 0)
                    {
                        int nInner = _sectorsPerRing[r - 1];
                        int innerSector = (s * nInner) / n;
                        int innerNeighbor = VertexIndex(r - 1, innerSector);
                        double arcStart = s * angleStep - Math.PI / 2;
                        edges.Add(new Edge(innerNeighbor, new ArcBorder(0, 0, r, arcStart, angleStep)));
                    }

                    // 外邻居
                    if (r < rings - 1)
                    {
                        int nOuter = _sectorsPerRing[r + 1];
                        int firstOuter = (s * nOuter) / n;
                        int lastOuter = ((s + 1) * nOuter) / n;
                        double outerAngleStep = 2 * Math.PI / nOuter;
                        for (int os = firstOuter; os < lastOuter; os++)
                        {
                            int outerNeighbor = VertexIndex(r + 1, os);
                            double arcStart = os * outerAngleStep - Math.PI / 2;
                            edges.Add(new Edge(outerNeighbor, new ArcBorder(0, 0, r + 1, arcStart, outerAngleStep)));
                        }
                    }

                    // 边界：最外环的外墙
                    if (r == rings - 1)
                    {
                        double arcStart = s * angleStep - Math.PI / 2;
                        edges.Add(new Edge(-1, new ArcBorder(0, 0, rings, arcStart, angleStep)));
                    }

                    g.Add(edges);
                }
            }

            return g;
        }

        public void RemoveBorders(List<(int, int)> spanningTree)
        {
            foreach (var (u, v) in spanningTree)
            {
                for (int i = 0; i < _graph[u].Count; i++)
                {
                    if (_graph[u][i].Neighbor == v)
                    {
                        _graph[u][i].Border = null;
                        break;
                    }
                }
                for (int i = 0; i < _graph[v].Count; i++)
                {
                    if (_graph[v][i].Neighbor == u)
                    {
                        _graph[v][i].Border = null;
                        break;
                    }
                }
            }
        }

        public int GetTileIndex(int ring, int sector)
        {
            if (ring < 0 || ring >= rings || sector < 0 || sector >= GetSectorsInRing(ring))
                throw new ArgumentOutOfRangeException();
            return VertexIndex(ring, sector);
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
                if (remaining < _sectorsPerRing[r])
                    return new Tile(r, remaining);
                remaining -= _sectorsPerRing[r];
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        public int GetRow(Tile tile)
        {
            return tile.lateral;
        }

        public List<Tile> GetTilesInRow(int row)
        {
            if (row < 0 || row >= rings)
                throw new ArgumentOutOfRangeException(nameof(row));
            var sectorsInRing = _sectorsPerRing[row];
            var tiles = new List<Tile>(sectorsInRing);
            for (int s = 0; s < sectorsInRing; s++)
                tiles.Add(new Tile(row, s));
            return tiles;
        }
    }
}
