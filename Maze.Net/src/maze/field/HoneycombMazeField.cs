using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 蜂窝状迷宫
    /// </summary>
    public class HoneycombMazeField : MazeField
    {
        public int Length { get; }

        /// <summary>
        /// Y轴朝上
        /// </summary>
        public override bool FlipY => true;

        /// <summary>
        /// 
        /// </summary>
        internal static readonly int[][] Neighbors = new int[][]
        {
            new int[] { -1, 0 },
            new int[] { -1, 1 },
            new int[] { 0, 1 },
            new int[] { 1, 0 },
            new int[] { 1, -1 },
            new int[] { 0, -1 },
        };

        internal HoneycombMazeField(int length)
        {
            Shape = EMazeShape.Honeycomb;
            Length = Math.Max(1, length);
            VertexCount = 3 * Length * (Length - 1) + 1;
            Graph = BuildGraph();
        }

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(VertexCount);
            for (int i = 0; i < VertexCount; i++) g.Add(new List<Adjacency>());

            for (int u = -Length + 1; u < Length; u++)
            {
                var (vmin, vmax) = VExtent(u);
                for (int v = vmin; v <= vmax; v++)
                {
                    var node = VertexIndex(u, v);

                    for (int n = 0; n < 6; n++)
                    {
                        var uu = u + Neighbors[n][0];
                        var vv = v + Neighbors[n][1];

                        if (IsValidNode(uu, vv))
                        {
                            var nnode = VertexIndex(uu, vv);
                            if (nnode <= node) continue;

                            var border = GetEdge(u, v, n);
                            g[node].Add(new Adjacency(nnode, border));
                            g[nnode].Add(new Adjacency(node, border));
                        }
                        else
                        {
                            var border = GetEdge(u, v, n);
                            g[node].Add(new Adjacency(-1, border));
                        }
                    }
                }
            }

            return g;
        }

        internal (int min, int max) VExtent(int u)
        {
            return (u < 0) ? (-Length - u + 1, Length - 1)
                           : (-Length + 1,     Length - 1 - u);
        }

        internal bool IsValidNode(int u, int v)
        {
            if (u <= -Length || u >= Length) 
                return false;

            var (min, max) = VExtent(u);
            return v >= min && v <= max;
        }

        internal int VertexIndex(int u, int v)
        {
            return (u <= 0) ? ((3 * Length + u) * (Length + u - 1)) / 2 + v
                            : (3 * Length * (Length - 1) + (4 * Length - u - 1) * u) / 2 + v;
        }

        private LineBorder GetEdge(int u, int v, int edge)
        {
            var dxu = Math.Sqrt(3) / 2;
            var dyu = 1.5;
            var dxv = Math.Sqrt(3);
            var dyv = 0;

            var cx = dxu * u + dxv * v;
            var cy = dyu * u + dyv * v;

            var theta1 = (edge - 2.5) * Math.PI / 3;
            var theta2 = theta1 + Math.PI / 3;

            return new LineBorder(cx + Math.Cos(theta1), 
                                  cy + Math.Sin(theta1),
                                  cx + Math.Cos(theta2), 
                                  cy + Math.Sin(theta2));
        }
    }
}
