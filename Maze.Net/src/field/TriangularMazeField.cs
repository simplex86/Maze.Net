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
        /// 构造三角形迷宫场地
        /// </summary>
        /// <param name="order">阶数</param>
        /// <param name="orientation">朝向</param>
        public TriangularMazeField(int order, TriangleOrientation orientation = TriangleOrientation.Upward)
        {
            this.order = Math.Max(1, order);
            this.orientation = orientation;
            count = this.order * this.order;
            graph = BuildGraph();
        }

        private int VertexIndex(int row, int col)
        {
            return row * row + col;
        }

        private List<List<Edge>> BuildGraph()
        {
            var g = new List<List<Edge>>(count);
            for (int i = 0; i < count; i++)
                g.Add(new List<Edge>());

            for (int row = 0; row < order; row++)
            {
                int colsInRow = 2 * row + 1;
                for (int col = 0; col < colsInRow; col++)
                {
                    int node = VertexIndex(row, col);
                    bool upward = (col % 2 == 0);

                    if (upward)
                        AddUpwardEdges(g, row, col, node);
                    else
                        AddDownwardEdges(g, row, col, node);
                }
            }

            return g;
        }

        private void AddUpwardEdges(List<List<Edge>> g, int row, int col, int node)
        {
            if (col > 0)
            {
                int neighbor = VertexIndex(row, col - 1);
                g[node].Add(new Edge(neighbor, MakeLeftEdgeOfUpward(row, col)));
            }
            else
            {
                g[node].Add(new Edge(-1, MakeLeftEdgeOfUpward(row, col)));
            }

            if (col < 2 * row)
            {
                int neighbor = VertexIndex(row, col + 1);
                g[node].Add(new Edge(neighbor, MakeRightEdgeOfUpward(row, col)));
            }
            else
            {
                g[node].Add(new Edge(-1, MakeRightEdgeOfUpward(row, col)));
            }

            if (row < order - 1)
            {
                int neighbor = VertexIndex(row + 1, col + 1);
                g[node].Add(new Edge(neighbor, MakeBottomEdgeOfUpward(row, col)));
            }
            else
            {
                g[node].Add(new Edge(-1, MakeBottomEdgeOfUpward(row, col)));
            }
        }

        private void AddDownwardEdges(List<List<Edge>> g, int row, int col, int node)
        {
            {
                int neighbor = VertexIndex(row, col - 1);
                g[node].Add(new Edge(neighbor, MakeLeftEdgeOfDownward(row, col)));
            }

            {
                int neighbor = VertexIndex(row, col + 1);
                g[node].Add(new Edge(neighbor, MakeRightEdgeOfDownward(row, col)));
            }

            {
                int neighbor = VertexIndex(row - 1, col - 1);
                g[node].Add(new Edge(neighbor, MakeTopEdgeOfDownward(row, col)));
            }
        }

        private LineBorder MakeLineBorder(double x1, double y1, double x2, double y2)
        {
            if (orientation == TriangleOrientation.Downward)
            {
                double maxY = order * Sqrt3Over2;
                return new LineBorder(x1, maxY - y1, x2, maxY - y2);
            }
            return new LineBorder(x1, y1, x2, y2);
        }

        private LineBorder MakeLeftEdgeOfUpward(int row, int col)
        {
            double topX = (order - row) / 2.0 + col / 2.0;
            double topY = row * Sqrt3Over2;
            double blX = (order - row - 1) / 2.0 + col / 2.0;
            double blY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(topX, topY, blX, blY);
        }

        private LineBorder MakeRightEdgeOfUpward(int row, int col)
        {
            double topX = (order - row) / 2.0 + col / 2.0;
            double topY = row * Sqrt3Over2;
            double brX = (order - row - 1) / 2.0 + col / 2.0 + 1;
            double brY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(topX, topY, brX, brY);
        }

        private LineBorder MakeBottomEdgeOfUpward(int row, int col)
        {
            double blX = (order - row - 1) / 2.0 + col / 2.0;
            double blY = (row + 1) * Sqrt3Over2;
            double brX = blX + 1;
            double brY = blY;
            return MakeLineBorder(blX, blY, brX, brY);
        }

        private LineBorder MakeLeftEdgeOfDownward(int row, int col)
        {
            int k = (col - 1) / 2;
            double tlX = (order - row) / 2.0 + k;
            double tlY = row * Sqrt3Over2;
            double bX = (order - row - 1) / 2.0 + k + 1;
            double bY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(tlX, tlY, bX, bY);
        }

        private LineBorder MakeRightEdgeOfDownward(int row, int col)
        {
            int k = (col - 1) / 2;
            double trX = (order - row) / 2.0 + k + 1;
            double trY = row * Sqrt3Over2;
            double bX = (order - row - 1) / 2.0 + k + 1;
            double bY = (row + 1) * Sqrt3Over2;
            return MakeLineBorder(trX, trY, bX, bY);
        }

        private LineBorder MakeTopEdgeOfDownward(int row, int col)
        {
            int k = (col - 1) / 2;
            double tlX = (order - row) / 2.0 + k;
            double tlY = row * Sqrt3Over2;
            double trX = tlX + 1;
            double trY = tlY;
            return MakeLineBorder(tlX, tlY, trX, trY);
        }
    }
}
