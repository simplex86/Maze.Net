using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// �����Թ�
    /// </summary>
    public class HoneycombMazeField : MazeField
    {
        /// <summary>
        /// Y轴朝上
        /// </summary>
        public override bool FlipY => true;

        private static readonly int[][] Neighbors = new int[][]
        {
            new int[] { -1, 0 },
            new int[] { -1, 1 },
            new int[] { 0, 1 },
            new int[] { 1, 0 },
            new int[] { 1, -1 },
            new int[] { 0, -1 },
        };

        public int length { get; }

        public HoneycombMazeField(int length)
        {
            this.length = Math.Max(1, length);
            count = 3 * this.length * (this.length - 1) + 1;
            graph = BuildGraph();
        }

        private (int min, int max) VExtent(int u)
        {
            return (u < 0) ? (-length - u + 1, length - 1)
                           : (-length + 1,     length - 1 - u);
        }

        private bool IsValidNode(int u, int v)
        {
            if (u <= -length || u >= length) 
                return false;

            var (min, max) = VExtent(u);
            return v >= min && v <= max;
        }

        private int VertexIndex(int u, int v)
        {
            if (u <= 0)
                return ((3 * length + u) * (length + u - 1)) / 2 + v;
            else
                return (3 * length * (length - 1) + (4 * length - u - 1) * u) / 2 + v;
        }

        private LineBorder GetEdge(int u, int v, int edge)
        {
            double dxu = Math.Sqrt(3) / 2, dyu = 1.5, dxv = Math.Sqrt(3), dyv = 0;
            double cx = dxu * u + dxv * v;
            double cy = dyu * u + dyv * v;

            double theta1 = (edge - 2.5) * Math.PI / 3;
            double theta2 = theta1 + Math.PI / 3;
            return new LineBorder(cx + Math.Cos(theta1), cy + Math.Sin(theta1),
                                  cx + Math.Cos(theta2), cy + Math.Sin(theta2));
        }

        private List<List<Edge>> BuildGraph()
        {
            var g = new List<List<Edge>>(count);
            for (int i = 0; i < count; i++)
                g.Add(new List<Edge>());

            for (int u = -length + 1; u < length; u++)
            {
                var (vmin, vmax) = VExtent(u);
                for (int v = vmin; v <= vmax; v++)
                {
                    int node = VertexIndex(u, v);

                    for (int n = 0; n < 6; n++)
                    {
                        int uu = u + Neighbors[n][0];
                        int vv = v + Neighbors[n][1];

                        if (IsValidNode(uu, vv))
                        {
                            int nnode = VertexIndex(uu, vv);
                            if (nnode <= node) continue;

                            var border = GetEdge(u, v, n);
                            g[node].Add(new Edge(nnode, border));
                            g[nnode].Add(new Edge(node, border));
                        }
                        else
                        {
                            var border = GetEdge(u, v, n);
                            g[node].Add(new Edge(-1, border));
                        }
                    }
                }
            }

            return g;
        }
    }
}
