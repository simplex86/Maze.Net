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

        /// <summary>
        /// 出入口在对边：6个方向，对边方向相差3
        /// </summary>
        public override MazeGate GenerateOppositeEdgeGate(Random random)
        {
            // 按边界方向分组：方向n的边界顶点
            var sides = new List<int>[6];
            for (int i = 0; i < 6; i++) sides[i] = new List<int>();

            for (int u = -length + 1; u < length; u++)
            {
                var (vmin, vmax) = VExtent(u);
                for (int v = vmin; v <= vmax; v++)
                {
                    var node = VertexIndex(u, v);
                    for (int n = 0; n < 6; n++)
                    {
                        var uu = u + Neighbors[n][0];
                        var vv = v + Neighbors[n][1];
                        if (!IsValidNode(uu, vv))
                            sides[n].Add(node);
                    }
                }
            }

            // 3组对边：0↔3, 1↔4, 2↔5
            var pair = random.Next(3);
            var entranceSide = pair;
            var exitSide = pair + 3;

            if (random.Next(2) == 0)
                (entranceSide, exitSide) = (exitSide, entranceSide);

            if (sides[entranceSide].Count == 0 || sides[exitSide].Count == 0)
                return base.GenerateOppositeEdgeGate(random);

            var entrance = sides[entranceSide][random.Next(sides[entranceSide].Count)];
            var exit = sides[exitSide][random.Next(sides[exitSide].Count)];

            return new MazeGate(entrance, exit);
        }

        /// <summary>
        /// 
        /// </summary>
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

        public Vertex GetVertexHexagon(int vertex)
        {
            // 反推 (u, v) 坐标
            var totalUp = length * (3 * length - 1) / 2;
            if (vertex < totalUp)
            {
                // u <= 0 的部分
                for (int u = -length + 1; u <= 0; u++)
                {
                    var (vmin, vmax) = VExtent(u);
                    var rowSize = vmax - vmin + 1;
                    if (vertex < rowSize)
                    {
                        int v = vmin + vertex;
                        return ComputeCenter(u, v);
                    }
                    vertex -= rowSize;
                }
            }
            else
            {
                vertex -= totalUp;
                // u > 0 的部分
                for (int u = 1; u < length; u++)
                {
                    var (vmin, vmax) = VExtent(u);
                    int rowSize = vmax - vmin + 1;
                    if (vertex < rowSize)
                    {
                        int v = vmin + vertex;
                        return ComputeCenter(u, v);
                    }
                    vertex -= rowSize;
                }
            }
            return new Vertex(0, 0);
        }

        private Vertex ComputeCenter(int u, int v)
        {
            var dxu = Math.Sqrt(3) / 2;
            var dyu = 1.5;
            var dxv = Math.Sqrt(3);
            var dyv = 0;

            return new Vertex(dxu * u + dxv * v, dyu * u + dyv * v);
        }

        public HoneycombMazeField(int length)
        {
            this.length = Math.Max(1, length);
            Count = 3 * this.length * (this.length - 1) + 1;
            Graph = BuildGraph();
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

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(Count);
            for (int i = 0; i < Count; i++) g.Add(new List<Adjacency>());

            for (int u = -length + 1; u < length; u++)
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
    }
}
