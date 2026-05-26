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

        public HexagonalMazeField(int size)
        {
            this.size = Math.Max(1, size);
            count = 6 * this.size * this.size;
            graph = BuildGraph();
        }

        private int VertexIndex(int sector, int updown, int row, int column)
        {
            int index = sector * size * size;
            if (updown == 1)
                index += size * (size + 1) / 2;
            index += row * (row + 1) / 2 + column;
            return index;
        }

        protected virtual IMazeBorder GetEdge(int sector, int row, int column, int edge)
        {
            double x1 = 0, y1 = 0;
            double x2 = -size / 2.0, y2 = Math.Sqrt(3) * x2;
            double x3 = -x2, y3 = y2;
            double dx12 = (x2 - x1) / size, dy12 = (y2 - y1) / size;
            double dx23 = (x3 - x2) / size, dy23 = (y3 - y2) / size;

            double ex1, ey1, ex2, ey2;
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

            double theta = sector * Math.PI / 3;
            double sinTheta = Math.Sin(theta);
            double cosTheta = Math.Cos(theta);

            return new LineBorder(
                ex1 * cosTheta - ey1 * sinTheta,
                ex1 * sinTheta + ey1 * cosTheta,
                ex2 * cosTheta - ey2 * sinTheta,
                ex2 * sinTheta + ey2 * cosTheta);
        }

        private List<List<Edge>> BuildGraph()
        {
            var g = new List<List<Edge>>(count);
            for (int i = 0; i < count; i++)
                g.Add(new List<Edge>());

            for (int sector = 0; sector < 6; sector++)
            {
                for (int i = 0; i < size; i++)
                {
                    var border = GetEdge(sector, size - 1, i, 0);
                    g[VertexIndex(sector, 0, size - 1, i)].Add(new Edge(-1, border));
                }

                for (int i = 0; i < size; i++)
                {
                    var border = GetEdge(sector, i, i, 1);
                    int v1 = VertexIndex(sector, 0, i, i);
                    int v2 = VertexIndex((sector + 1) % 6, 0, i, 0);
                    g[v1].Add(new Edge(v2, border));
                    g[v2].Add(new Edge(v1, border));
                }

                for (int i = 0; i < size - 1; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        var border = GetEdge(sector, i, j, 0);
                        int v1 = VertexIndex(sector, 0, i, j);
                        int v2 = VertexIndex(sector, 1, i, j);
                        g[v1].Add(new Edge(v2, border));
                        g[v2].Add(new Edge(v1, border));
                    }
                }

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        var border = GetEdge(sector, i, j, 1);
                        int v1 = VertexIndex(sector, 0, i, j);
                        int v2 = VertexIndex(sector, 1, i - 1, j);
                        g[v1].Add(new Edge(v2, border));
                        g[v2].Add(new Edge(v1, border));
                    }
                }

                for (int i = 0; i < size; i++)
                {
                    for (int j = 1; j <= i; j++)
                    {
                        var border = GetEdge(sector, i, j, 2);
                        int v1 = VertexIndex(sector, 0, i, j);
                        int v2 = VertexIndex(sector, 1, i - 1, j - 1);
                        g[v1].Add(new Edge(v2, border));
                        g[v2].Add(new Edge(v1, border));
                    }
                }
            }

            return g;
        }
    }
}
