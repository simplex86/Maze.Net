using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class HexagonalMazeField : MazeField
    {
        public int size { get; }

        /// <summary>
        /// Y轴朝上
        /// </summary>
        public override bool FlipY => true;

        /// <summary>
        /// 出入口在对边：6个扇区，对边扇区相差3
        /// </summary>
        public override MazeGate GenerateOppositeEdgeGate(Random random)
        {
            var entranceSector = random.Next(6);
            var exitSector = (entranceSector + 3) % 6;

            // 各扇区外边缘顶点：VertexIndex(sector, 0, size-1, i)
            var entranceCandidates = new List<int>();
            var exitCandidates = new List<int>();
            for (int i = 0; i < size; i++)
            {
                entranceCandidates.Add(VertexIndex(entranceSector, 0, size - 1, i));
                exitCandidates.Add(VertexIndex(exitSector, 0, size - 1, i));
            }

            var entrance = entranceCandidates[random.Next(entranceCandidates.Count)];
            var exit = exitCandidates[random.Next(exitCandidates.Count)];

            return new MazeGate(entrance, exit);
        }

        /// <summary>
        /// 获取顶点所在三角形的三个顶点坐标
        /// </summary>
        public Triangle GetVertexTriangle(int vertex)
        {
            var sectorSize = size * size;
            var sector = vertex / sectorSize;
            var remaining = vertex % sectorSize;
            var updownSize = size * (size + 1) / 2;
            var updown = remaining < updownSize ? 0 : 1;
            var idx = updown == 0 ? remaining : remaining - updownSize;

            var row = 0;
            while ((row + 1) * (row + 2) / 2 <= idx)
                row++;
            var column = idx - row * (row + 1) / 2;

            var x1 = 0;
            var y1 = 0;
            var x2 = -size / 2.0;
            var y2 = Math.Sqrt(3) * x2;
            var x3 = -x2;
            var y3 = y2;
            var dx12 = (x2 - x1) / size;
            var dy12 = (y2 - y1) / size;
            var dx23 = (x3 - x2) / size;
            var dy23 = (y3 - y2) / size;

            Vertex pa, pb, pc;

            if (updown == 0)
            {
                var topX = dx12 * row + dx23 * column;
                var topY = dy12 * row + dy23 * column;
                var blX = dx12 * (row + 1) + dx23 * column;
                var blY = dy12 * (row + 1) + dy23 * column;
                var brX = dx12 * (row + 1) + dx23 * (column + 1);
                var brY = dy12 * (row + 1) + dy23 * (column + 1);
                pa = new Vertex(topX, topY);
                pb = new Vertex(blX, blY);
                pc = new Vertex(brX, brY);
            }
            else
            {
                var tlX = dx12 * (row + 1) + dx23 * column;
                var tlY = dy12 * (row + 1) + dy23 * column;
                var trX = dx12 * (row + 1) + dx23 * (column + 1);
                var trY = dy12 * (row + 1) + dy23 * (column + 1);
                var bX = dx12 * (row + 2) + dx23 * (column + 1);
                var bY = dy12 * (row + 2) + dy23 * (column + 1);
                pa = new Vertex(tlX, tlY);
                pb = new Vertex(trX, trY);
                pc = new Vertex(bX, bY);
            }

            var theta = sector * Math.PI / 3;
            var cosTheta = Math.Cos(theta);
            var sinTheta = Math.Sin(theta);

            return new Triangle(new Vertex(pa.x * cosTheta - pa.y * sinTheta, pa.x * sinTheta + pa.y * cosTheta),
                                new Vertex(pb.x * cosTheta - pb.y * sinTheta, pb.x * sinTheta + pb.y * cosTheta),
                                new Vertex(pc.x * cosTheta - pc.y * sinTheta, pc.x * sinTheta + pc.y * cosTheta));
        }

        public HexagonalMazeField(int size)
        {
            this.size = Math.Max(1, size);
            Count = 6 * this.size * this.size;
            Graph = BuildGraph();
        }

        protected int VertexIndex(int sector, int updown, int row, int column)
        {
            var index = sector * size * size;
            if (updown == 1) index += size * (size + 1) / 2;
            index += row * (row + 1) / 2 + column;

            return index;
        }

        protected virtual IMazeBorder GetEdge(int sector, int row, int column, int edge)
        {
            var x1 = 0;
            var y1 = 0;
            var x2 = -size / 2.0;
            var y2 = Math.Sqrt(3) * x2;
            var x3 = -x2;
            var y3 = y2;
            var dx12 = (x2 - x1) / size;
            var dy12 = (y2 - y1) / size;
            var dx23 = (x3 - x2) / size;
            var dy23 = (y3 - y2) / size;

            var ex1 = 0.0;
            var ey1 = 0.0;
            var ex2 = 0.0;
            var ey2 = 0.0;

            if (edge == 0)
            {
                ex1 = x1 + dx12 * (row + 1) + dx23 * column;
                ey1 = y1 + dy12 * (row + 1) + dy23 * column;
                ex2 = ex1 + dx23;
                ey2 = ey1 + dy23;
            }
            else if (edge == 1)
            {
                ex1 = x1 + dx12 * row + dx23 * column;
                ey1 = y1 + dy12 * row + dy23 * column;
                ex2 = ex1 + dx12 + dx23;
                ey2 = ey1 + dy12 + dy23;
            }
            else
            {
                ex1 = x1 + dx12 * row + dx23 * column;
                ey1 = y1 + dy12 * row + dy23 * column;
                ex2 = ex1 + dx12;
                ey2 = ey1 + dy12;
            }

            var theta = sector * Math.PI / 3;
            var sinTheta = Math.Sin(theta);
            var cosTheta = Math.Cos(theta);

            return new LineBorder(ex1 * cosTheta - ey1 * sinTheta,
                                  ex1 * sinTheta + ey1 * cosTheta,
                                  ex2 * cosTheta - ey2 * sinTheta,
                                  ex2 * sinTheta + ey2 * cosTheta);
        }

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(Count);
            for (int i = 0; i < Count; i++)
                g.Add(new List<Adjacency>());

            for (int sector = 0; sector < 6; sector++)
            {
                for (int i = 0; i < size; i++)
                {
                    var border = GetEdge(sector, size - 1, i, 0);
                    g[VertexIndex(sector, 0, size - 1, i)].Add(new Adjacency(-1, border));
                }

                for (int i = 0; i < size; i++)
                {
                    var border = GetEdge(sector, i, i, 1);
                    var v1 = VertexIndex(sector, 0, i, i);
                    var v2 = VertexIndex((sector + 1) % 6, 0, i, 0);
                    g[v1].Add(new Adjacency(v2, border));
                    g[v2].Add(new Adjacency(v1, border));
                }

                for (int i = 0; i < size - 1; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        var border = GetEdge(sector, i, j, 0);
                        var v1 = VertexIndex(sector, 0, i, j);
                        var v2 = VertexIndex(sector, 1, i, j);
                        g[v1].Add(new Adjacency(v2, border));
                        g[v2].Add(new Adjacency(v1, border));
                    }
                }

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        var border = GetEdge(sector, i, j, 1);
                        var v1 = VertexIndex(sector, 0, i, j);
                        var v2 = VertexIndex(sector, 1, i - 1, j);
                        g[v1].Add(new Adjacency(v2, border));
                        g[v2].Add(new Adjacency(v1, border));
                    }
                }

                for (int i = 0; i < size; i++)
                {
                    for (int j = 1; j <= i; j++)
                    {
                        var border = GetEdge(sector, i, j, 2);
                        var v1 = VertexIndex(sector, 0, i, j);
                        var v2 = VertexIndex(sector, 1, i - 1, j - 1);
                        g[v1].Add(new Adjacency(v2, border));
                        g[v2].Add(new Adjacency(v1, border));
                    }
                }
            }

            return g;
        }
    }
}
