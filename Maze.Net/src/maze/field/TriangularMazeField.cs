using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 三角形朝向
    /// </summary>
    public enum TriangleOrientation
    {
        /// <summary>
        /// 朝上
        /// </summary>
        Upward = 1,
        /// <summary>
        /// 朝下
        /// </summary>
        Downward = 2,
    }

    /// <summary>
    /// 三角形迷宫场地（邻接表方案）
    /// 等边三角形内部由小等边三角形格子填充
    /// 第 i 行有 2i+1 个小三角形（朝上、朝下交替）
    /// 总格子数 = N²
    /// </summary>
    public class TriangularMazeField : MazeField
    {
        public int Order { get; }

        public TriangleOrientation Orientation { get; }

        private static readonly double Sqrt3Over2 = Math.Sqrt(3) / 2;

        internal TriangularMazeField(int order, TriangleOrientation orientation = TriangleOrientation.Upward)
        {
            Shape = EMazeShape.Triangular;
            Order = Math.Max(1, order);
            Orientation = orientation;
            VertexCount = Order * Order;
            Graph = BuildGraph();
        }

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(VertexCount);
            for (int i = 0; i < VertexCount; i++) g.Add(new List<Adjacency>());

            for (int row = 0; row < Order; row++)
            {
                var colsInRow = 2 * row + 1;
                for (int col = 0; col < colsInRow; col++)
                {
                    var node = VertexIndex(row, col);

                    if (col % 2 == 0) 
                        AddUpwardEdges(g, row, col, node);
                    else  
                        AddDownwardEdges(g, row, col, node);
                }
            }

            return g;
        }

        private int VertexIndex(int row, int col)
        {
            return row * row + col;
        }

        private void AddUpwardEdges(List<List<Adjacency>> g, int row, int col, int node)
        {
            if (col > 0)
            {
                var neighbor = VertexIndex(row, col - 1);
                g[node].Add(new Adjacency(neighbor, MakeLeftEdgeOfUpward(row, col)));
            }
            else
            {
                g[node].Add(new Adjacency(-1, MakeLeftEdgeOfUpward(row, col)));
            }

            if (col < 2 * row)
            {
                var neighbor = VertexIndex(row, col + 1);
                g[node].Add(new Adjacency(neighbor, MakeRightEdgeOfUpward(row, col)));
            }
            else
            {
                g[node].Add(new Adjacency(-1, MakeRightEdgeOfUpward(row, col)));
            }

            if (row < Order - 1)
            {
                var neighbor = VertexIndex(row + 1, col + 1);
                g[node].Add(new Adjacency(neighbor, MakeBottomEdgeOfUpward(row, col)));
            }
            else
            {
                g[node].Add(new Adjacency(-1, MakeBottomEdgeOfUpward(row, col)));
            }
        }

        private void AddDownwardEdges(List<List<Adjacency>> g, int row, int col, int node)
        {
            {
                var neighbor = VertexIndex(row, col - 1);
                g[node].Add(new Adjacency(neighbor, MakeLeftEdgeOfDownward(row, col)));
            }

            {
                var neighbor = VertexIndex(row, col + 1);
                g[node].Add(new Adjacency(neighbor, MakeRightEdgeOfDownward(row, col)));
            }

            {
                var neighbor = VertexIndex(row - 1, col - 1);
                g[node].Add(new Adjacency(neighbor, MakeTopEdgeOfDownward(row, col)));
            }
        }

        private LineBorder MakeLineBorder(double x1, double y1, double x2, double y2)
        {
            if (Orientation == TriangleOrientation.Downward)
            {
                var maxY = Order * Sqrt3Over2;
                return new LineBorder(x1, maxY - y1, x2, maxY - y2);
            }
            return new LineBorder(x1, y1, x2, y2);
        }

        private LineBorder MakeLeftEdgeOfUpward(int row, int col)
        {
            var topX = (Order - row) / 2.0 + col / 2.0;
            var topY = row * Sqrt3Over2;
            var blX = (Order - row - 1) / 2.0 + col / 2.0;
            var blY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(topX, topY, blX, blY);
        }

        private LineBorder MakeRightEdgeOfUpward(int row, int col)
        {
            var topX = (Order - row) / 2.0 + col / 2.0;
            var topY = row * Sqrt3Over2;
            var brX = (Order - row - 1) / 2.0 + col / 2.0 + 1;
            var brY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(topX, topY, brX, brY);
        }

        private LineBorder MakeBottomEdgeOfUpward(int row, int col)
        {
            var blX = (Order - row - 1) / 2.0 + col / 2.0;
            var blY = (row + 1) * Sqrt3Over2;
            var brX = blX + 1;
            var brY = blY;
            return MakeLineBorder(blX, blY, brX, brY);
        }

        private LineBorder MakeLeftEdgeOfDownward(int row, int col)
        {
            var k = (col - 1) / 2;
            var tlX = (Order - row) / 2.0 + k;
            var tlY = row * Sqrt3Over2;
            var bX = (Order - row - 1) / 2.0 + k + 1;
            var bY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(tlX, tlY, bX, bY);
        }

        private LineBorder MakeRightEdgeOfDownward(int row, int col)
        {
            var k = (col - 1) / 2;
            var trX = (Order - row) / 2.0 + k + 1;
            var trY = row * Sqrt3Over2;
            var bX = (Order - row - 1) / 2.0 + k + 1;
            var bY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(trX, trY, bX, bY);
        }

        private LineBorder MakeTopEdgeOfDownward(int row, int col)
        {
            var k = (col - 1) / 2;
            var tlX = (Order - row) / 2.0 + k;
            var tlY = row * Sqrt3Over2;
            var trX = tlX + 1;
            var trY = tlY;
            return MakeLineBorder(tlX, tlY, trX, trY);
        }
    }
}
