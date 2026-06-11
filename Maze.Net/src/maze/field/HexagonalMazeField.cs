using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class HexagonalMazeField : MazeField
    {
        public int Size { get; internal protected set; }

        /// <summary>
        /// Y轴朝上
        /// </summary>
        public override bool FlipY => true;

        public HexagonalMazeField(int size)
        {
            Shape = EMazeShape.Hexagonal;
            Size = Math.Max(1, size);
            VertexCount = 6 * Size * Size;
            Graph = BuildGraph();
        }

        internal HexagonalMazeField() { }

        internal List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(VertexCount);
            for (int i = 0; i < VertexCount; i++)
                g.Add(new List<Adjacency>());

            for (int sector = 0; sector < 6; sector++)
            {
                for (int i = 0; i < Size; i++)
                {
                    var border = GetEdge(sector, Size - 1, i, 0);
                    g[VertexIndex(sector, 0, Size - 1, i)].Add(new Adjacency(-1, border));
                }

                for (int i = 0; i < Size; i++)
                {
                    var border = GetEdge(sector, i, i, 1);
                    var v1 = VertexIndex(sector, 0, i, i);
                    var v2 = VertexIndex((sector + 1) % 6, 0, i, 0);
                    g[v1].Add(new Adjacency(v2, border));
                    g[v2].Add(new Adjacency(v1, border));
                }

                for (int i = 0; i < Size - 1; i++)
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

                for (int i = 0; i < Size; i++)
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

                for (int i = 0; i < Size; i++)
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

        internal virtual IMazeBorder GetEdge(int sector, int row, int column, int edge)
        {
            var x1 = 0;
            var y1 = 0;
            var x2 = -Size / 2.0;
            var y2 = Math.Sqrt(3) * x2;
            var x3 = -x2;
            var y3 = y2;
            var dx12 = (x2 - x1) / Size;
            var dy12 = (y2 - y1) / Size;
            var dx23 = (x3 - x2) / Size;
            var dy23 = (y3 - y2) / Size;

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

        private int VertexIndex(int sector, int updown, int row, int column)
        {
            var index = sector * Size * Size;
            if (updown == 1) index += Size * (Size + 1) / 2;
            index += row * (row + 1) / 2 + column;

            return index;
        }

        internal override CellShape GetCellShape(int vertex)
        {
            var sectorSize = Size * Size;
            var sector = vertex / sectorSize;
            var remaining = vertex % sectorSize;
            var updownSize = Size * (Size + 1) / 2;
            var updown = remaining < updownSize ? 0 : 1;
            var idx = updown == 0 ? remaining : remaining - updownSize;

            var row = 0;
            while ((row + 1) * (row + 2) / 2 <= idx)
                row++;
            var column = idx - row * (row + 1) / 2;

            var x1 = 0;
            var y1 = 0;
            var x2 = -Size / 2.0;
            var y2 = Math.Sqrt(3) * x2;
            var x3 = -x2;
            var y3 = y2;
            var dx12 = (x2 - x1) / Size;
            var dy12 = (y2 - y1) / Size;
            var dx23 = (x3 - x2) / Size;
            var dy23 = (y3 - y2) / Size;

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

            return CellShape.Polygon(new Vertex[]
            {
                new Vertex(pa.X * cosTheta - pa.Y * sinTheta, pa.X * sinTheta + pa.Y * cosTheta),
                new Vertex(pb.X * cosTheta - pb.Y * sinTheta, pb.X * sinTheta + pb.Y * cosTheta),
                new Vertex(pc.X * cosTheta - pc.Y * sinTheta, pc.X * sinTheta + pc.Y * cosTheta),
            });
        }
    }
}
