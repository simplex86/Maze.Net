using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫场地（邻接表方案）
    /// </summary>
    public class CircularMazeField : MazeField
    {
        public int[] SectorsPerRing { get; }

        public int Rings { get; }
        public int Sectors { get; }

        public CircularMazeField(int rings, int maxSectors)
        {
            Shape = EMazeShape.Circular;

            Rings = Math.Max(1, rings);
            Sectors = Math.Max(3, maxSectors);

            SectorsPerRing = new int[this.Rings];

            var normalizedMaxSectors = 3;
            while (normalizedMaxSectors * 2 <= this.Sectors)
                normalizedMaxSectors *= 2;

            SectorsPerRing[0] = 3;
            for (var r = 1; r < this.Rings; r++)
            {
                SectorsPerRing[r] = SectorsPerRing[r - 1];
                var arcLength = (2 * Math.PI * (r + 1)) / SectorsPerRing[r - 1];
                if (arcLength > 2.0 && SectorsPerRing[r] * 2 <= normalizedMaxSectors)
                    SectorsPerRing[r] *= 2;
            }

            if (SectorsPerRing[this.Rings - 1] < normalizedMaxSectors)
                SectorsPerRing[this.Rings - 1] = normalizedMaxSectors;

            VertexCount = 0;
            for (var r = 0; r < this.Rings; r++)
                VertexCount += SectorsPerRing[r];

            Graph = BuildGraph();
        }

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(VertexCount);

            for (int r = 0; r < Rings; r++)
            {
                var n = SectorsPerRing[r];
                var angleStep = 2 * Math.PI / n;

                for (int s = 0; s < n; s++)
                {
                    var edges = new List<Adjacency>();

                    // 右邻居（同一环，下一个扇区）
                    var rightSector = (s + 1) % n;
                    var rightNeighbor = VertexIndex(r, rightSector);
                    var rightAngle = (s + 1) * angleStep - Math.PI / 2;
                    edges.Add(new Adjacency(rightNeighbor, new LineBorder(r * Math.Cos(rightAngle), 
                                                                     r * Math.Sin(rightAngle),
                                                                     (r + 1) * Math.Cos(rightAngle), 
                                                                     (r + 1) * Math.Sin(rightAngle))));

                    // 左邻居（同一环，上一个扇区）
                    var leftSector = (s - 1 + n) % n;
                    var leftNeighbor = VertexIndex(r, leftSector);
                    var leftAngle = s * angleStep - Math.PI / 2;
                    edges.Add(new Adjacency(leftNeighbor, new LineBorder(r * Math.Cos(leftAngle), 
                                                                    r * Math.Sin(leftAngle),
                                                                    (r + 1) * Math.Cos(leftAngle), 
                                                                    (r + 1) * Math.Sin(leftAngle))));

                    // 内邻居
                    if (r > 0)
                    {
                        var nInner = SectorsPerRing[r - 1];
                        var innerSector = (s * nInner) / n;
                        var innerNeighbor = VertexIndex(r - 1, innerSector);
                        var arcStart = s * angleStep - Math.PI / 2;
                        edges.Add(new Adjacency(innerNeighbor, new ArcBorder(0, 0, r, arcStart, angleStep)));
                    }

                    // 外邻居
                    if (r < Rings - 1)
                    {
                        var nOuter = SectorsPerRing[r + 1];
                        var firstOuter = (s * nOuter) / n;
                        var lastOuter = ((s + 1) * nOuter) / n;
                        var outerAngleStep = 2 * Math.PI / nOuter;
                        for (var os = firstOuter; os < lastOuter; os++)
                        {
                            var outerNeighbor = VertexIndex(r + 1, os);
                            var arcStart = os * outerAngleStep - Math.PI / 2;
                            edges.Add(new Adjacency(outerNeighbor, new ArcBorder(0, 0, r + 1, arcStart, outerAngleStep)));
                        }
                    }

                    // 边界：最外环的外墙
                    if (r == Rings - 1)
                    {
                        var arcStart = s * angleStep - Math.PI / 2;
                        edges.Add(new Adjacency(-1, new ArcBorder(0, 0, Rings, arcStart, angleStep)));
                    }

                    g.Add(edges);
                }
            }

            return g;
        }

        private int VertexIndex(int ring, int sector)
        {
            var index = 0;
            for (var r = 0; r < ring; r++)
                index += SectorsPerRing[r];

            return index + sector;
        }
    }
}
