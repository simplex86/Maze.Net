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
        private static readonly double Sqrt3Over2 = Math.Sqrt(3) / 2;

        /// <summary>
        /// 阶数（行数，也等于边长的小三角形数）
        /// </summary>
        public int order { get; }

        /// <summary>
        /// 朝向
        /// </summary>
        public TriangleOrientation orientation { get; }

        /// <summary>
        /// 入口在底边，出口在顶点
        /// </summary>
        public override MazeGate GenerateOppositeEdgeGate(Random random)
        {
            // 底边顶点：最后一行 row=order-1
            var baseVertices = new List<int>();
            for (int col = 0; col < 2 * order - 1; col++)
                baseVertices.Add(VertexIndex(order - 1, col));

            // 顶点：row=0, col=0
            var apex = VertexIndex(0, 0);
            var entrance = baseVertices[random.Next(baseVertices.Count)];

            return new MazeGate(entrance, apex);
        }

        /// <summary>
        /// 获取顶点所在三角形的三个顶点坐标
        /// </summary>
        public Triangle GetVertexTriangle(int vertex)
        {
            var row = 0;
            while ((row + 1) * (row + 1) <= vertex)
                row++;

            var col = vertex - row * row;
            var upward = (col % 2 == 0);

            Vertex p1, p2, p3;

            if (upward)
            {
                var topX = (order - row) / 2.0 + col / 2.0;
                var topY = row * Sqrt3Over2;
                var blX = (order - row - 1) / 2.0 + col / 2.0;
                var blY = (row + 1) * Sqrt3Over2;
                var brX = blX + 1;
                var brY = blY;
                p1 = new Vertex(topX, topY);
                p2 = new Vertex(blX, blY);
                p3 = new Vertex(brX, brY);
            }
            else
            {
                var k = (col - 1) / 2;
                var tlX = (order - row) / 2.0 + k;
                var tlY = row * Sqrt3Over2;
                var trX = tlX + 1;
                var trY = tlY;
                var bX = (order - row - 1) / 2.0 + k + 1;
                var bY = (row + 1) * Sqrt3Over2;
                p1 = new Vertex(tlX, tlY);
                p2 = new Vertex(trX, trY);
                p3 = new Vertex(bX, bY);
            }

            if (orientation == TriangleOrientation.Downward)
            {
                var maxY = order * Sqrt3Over2;
                p1 = new Vertex(p1.x, maxY - p1.y);
                p2 = new Vertex(p2.x, maxY - p2.y);
                p3 = new Vertex(p3.x, maxY - p3.y);
            }

            return new Triangle(p1, p2, p3);
        }

        /// <summary>
        /// 构造三角形迷宫场地
        /// </summary>
        /// <param name="order">阶数</param>
        /// <param name="orientation">朝向</param>
        public TriangularMazeField(int order, TriangleOrientation orientation = TriangleOrientation.Upward)
        {
            this.order = Math.Max(1, order);
            this.orientation = orientation;
            Count = this.order * this.order;
            Graph = BuildGraph();
        }

        private int VertexIndex(int row, int col)
        {
            return row * row + col;
        }

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(Count);
            for (int i = 0; i < Count; i++) g.Add(new List<Adjacency>());

            for (int row = 0; row < order; row++)
            {
                var colsInRow = 2 * row + 1;
                for (int col = 0; col < colsInRow; col++)
                {
                    int node = VertexIndex(row, col);
                    bool upward = (col % 2 == 0);

                    if (upward) AddUpwardEdges(g, row, col, node);
                    else        AddDownwardEdges(g, row, col, node);
                }
            }

            return g;
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

            if (row < order - 1)
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
            if (orientation == TriangleOrientation.Downward)
            {
                var maxY = order * Sqrt3Over2;
                return new LineBorder(x1, maxY - y1, x2, maxY - y2);
            }
            return new LineBorder(x1, y1, x2, y2);
        }

        private LineBorder MakeLeftEdgeOfUpward(int row, int col)
        {
            var topX = (order - row) / 2.0 + col / 2.0;
            var topY = row * Sqrt3Over2;
            var blX = (order - row - 1) / 2.0 + col / 2.0;
            var blY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(topX, topY, blX, blY);
        }

        private LineBorder MakeRightEdgeOfUpward(int row, int col)
        {
            var topX = (order - row) / 2.0 + col / 2.0;
            var topY = row * Sqrt3Over2;
            var brX = (order - row - 1) / 2.0 + col / 2.0 + 1;
            var brY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(topX, topY, brX, brY);
        }

        private LineBorder MakeBottomEdgeOfUpward(int row, int col)
        {
            var blX = (order - row - 1) / 2.0 + col / 2.0;
            var blY = (row + 1) * Sqrt3Over2;
            var brX = blX + 1;
            var brY = blY;
            return MakeLineBorder(blX, blY, brX, brY);
        }

        private LineBorder MakeLeftEdgeOfDownward(int row, int col)
        {
            var k = (col - 1) / 2;
            var tlX = (order - row) / 2.0 + k;
            var tlY = row * Sqrt3Over2;
            var bX = (order - row - 1) / 2.0 + k + 1;
            var bY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(tlX, tlY, bX, bY);
        }

        private LineBorder MakeRightEdgeOfDownward(int row, int col)
        {
            var k = (col - 1) / 2;
            var trX = (order - row) / 2.0 + k + 1;
            var trY = row * Sqrt3Over2;
            var bX = (order - row - 1) / 2.0 + k + 1;
            var bY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(trX, trY, bX, bY);
        }

        private LineBorder MakeTopEdgeOfDownward(int row, int col)
        {
            var k = (col - 1) / 2;
            var tlX = (order - row) / 2.0 + k;
            var tlY = row * Sqrt3Over2;
            var trX = tlX + 1;
            var trY = tlY;
            return MakeLineBorder(tlX, tlY, trX, trY);
        }
    }
}
