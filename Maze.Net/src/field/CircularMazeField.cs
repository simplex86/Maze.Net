using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫场地（邻接表方案）
    /// </summary>
    public class CircularMazeField : MazeField
    {
        private readonly int[] _sectorsPerRing;

        public int rings { get; }
        public int sectors { get; }
        public SectorStrategy strategy { get; }

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

            count = 0;
            for (var r = 0; r < this.rings; r++)
                count += _sectorsPerRing[r];

            graph = BuildGraph();
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
            var g = new List<List<Edge>>(count);

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
    }
}
