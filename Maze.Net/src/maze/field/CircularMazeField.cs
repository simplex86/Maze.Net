using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫场地（邻接表方案）
    /// </summary>
    public class CircularMazeField : MazeField
    {
        private readonly int[] sectorsPerRing;

        public int rings { get; }
        public int sectors { get; }
        public ESectorStrategy strategy { get; }

        /// <summary>
        /// 出入口在直径上：外环对径扇区
        /// </summary>
        public override MazeGate GenerateOppositeEdgeGate(Random random)
        {
            var n = sectorsPerRing[rings - 1];
            var entranceSector = random.Next(n);
            var exitSector = (entranceSector + n / 2) % n;

            var entrance = VertexIndex(rings - 1, entranceSector);
            var exit = VertexIndex(rings - 1, exitSector);
            return new MazeGate(entrance, exit);
        }

        /// <summary>
        /// 获取顶点所在扇区的几何参数（内半径、外半径、起始角、扫过角）
        /// </summary>
        public AnnularSector GetVertexSector(int vertex)
        {
            var remaining = vertex;
            for (var r = 0; r < rings; r++)
            {
                if (remaining < sectorsPerRing[r])
                {
                    var n = sectorsPerRing[r];
                    var angleStep = 2 * Math.PI / n;
                    var startAngle = remaining * angleStep - Math.PI / 2;
                    return new AnnularSector(r, r + 1, startAngle, angleStep);
                }
                remaining -= sectorsPerRing[r];
            }
            return new AnnularSector(0, 0, 0, 0);
        }

        public CircularMazeField(int rings, int maxSectors, ESectorStrategy strategy)
        {
            this.rings = Math.Max(1, rings);
            this.sectors = Math.Max(3, maxSectors);
            this.strategy = strategy;

            sectorsPerRing = new int[this.rings];

            var normalizedMaxSectors = 3;
            while (normalizedMaxSectors * 2 <= this.sectors)
                normalizedMaxSectors *= 2;

            if (strategy == ESectorStrategy.Arc)
            {
                sectorsPerRing[0] = 3;
                for (var r = 1; r < this.rings; r++)
                {
                    sectorsPerRing[r] = sectorsPerRing[r - 1];
                    var arcLength = (2 * Math.PI * (r + 1)) / sectorsPerRing[r - 1];
                    if (arcLength > 2.0 && sectorsPerRing[r] * 2 <= normalizedMaxSectors)
                        sectorsPerRing[r] *= 2;
                }
            }
            else
            {
                sectorsPerRing[0] = 3;
                for (var r = 1; r < this.rings; r++)
                {
                    sectorsPerRing[r] = sectorsPerRing[r - 1];
                    var area = (Math.PI * (r + 1) * (r + 1)) / sectorsPerRing[r - 1];
                    if (area > 2.0 && sectorsPerRing[r] * 2 <= normalizedMaxSectors)
                        sectorsPerRing[r] *= 2;
                }
            }

            if (sectorsPerRing[this.rings - 1] < normalizedMaxSectors)
                sectorsPerRing[this.rings - 1] = normalizedMaxSectors;

            Count = 0;
            for (var r = 0; r < this.rings; r++)
                Count += sectorsPerRing[r];

            Graph = BuildGraph();
        }

        private int VertexIndex(int ring, int sector)
        {
            var index = 0;
            for (var r = 0; r < ring; r++)
                index += sectorsPerRing[r];

            return index + sector;
        }

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(Count);

            for (int r = 0; r < rings; r++)
            {
                var n = sectorsPerRing[r];
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
                        var nInner = sectorsPerRing[r - 1];
                        var innerSector = (s * nInner) / n;
                        var innerNeighbor = VertexIndex(r - 1, innerSector);
                        var arcStart = s * angleStep - Math.PI / 2;
                        edges.Add(new Adjacency(innerNeighbor, new ArcBorder(0, 0, r, arcStart, angleStep)));
                    }

                    // 外邻居
                    if (r < rings - 1)
                    {
                        var nOuter = sectorsPerRing[r + 1];
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
                    if (r == rings - 1)
                    {
                        var arcStart = s * angleStep - Math.PI / 2;
                        edges.Add(new Adjacency(-1, new ArcBorder(0, 0, rings, arcStart, angleStep)));
                    }

                    g.Add(edges);
                }
            }

            return g;
        }
    }
}
