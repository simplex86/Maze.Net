using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫场地（邻接表方案）
    /// </summary>
    public class RectangularMazeField : MazeField
    {
        public int Width { get; internal protected set; }
        public int Height { get; internal protected set; }

        public RectangularMazeField(int width, int height)
        {
            Shape = EMazeShape.Rectangular;
            Width = Math.Max(1, width);
            Height = Math.Max(1, height);
            VertexCount = width * height;
            Graph = BuildGraph();
        }

        internal RectangularMazeField() { }

        internal override CellShape GetCellShape(int vertex)
        {
            var cx = vertex % Width;
            var cy = vertex / Width;
            return CellShape.Polygon(new Vertex[]
            {
                new Vertex(cx, cy),
                new Vertex(cx + 1, cy),
                new Vertex(cx + 1, cy + 1),
                new Vertex(cx, cy + 1),
            });
        }

        internal List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(VertexCount);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var edges = new List<Adjacency>();

                    // 右邻居
                    if (x < Width - 1)
                    {
                        var neighbor = y * Width + (x + 1);
                        edges.Add(new Adjacency(neighbor, new LineBorder(x + 1, y, x + 1, y + 1)));
                    }
                    // 左邻居
                    if (x > 0)
                    {
                        var neighbor = y * Width + (x - 1);
                        edges.Add(new Adjacency(neighbor, new LineBorder(x, y, x, y + 1)));
                    }
                    // 下邻居
                    if (y < Height - 1)
                    {
                        var neighbor = (y + 1) * Width + x;
                        edges.Add(new Adjacency(neighbor, new LineBorder(x, y + 1, x + 1, y + 1)));
                    }
                    // 上邻居
                    if (y > 0)
                    {
                        var neighbor = (y - 1) * Width + x;
                        edges.Add(new Adjacency(neighbor, new LineBorder(x, y, x + 1, y)));
                    }

                    // 边界边
                    if (x == 0)          edges.Add(new Adjacency(-1, new LineBorder(0, y, 0, y + 1)));
                    if (x == Width - 1)  edges.Add(new Adjacency(-1, new LineBorder(Width, y, Width, y + 1)));
                    if (y == 0)          edges.Add(new Adjacency(-1, new LineBorder(x, 0, x + 1, 0)));
                    if (y == Height - 1) edges.Add(new Adjacency(-1, new LineBorder(x, Height, x + 1, Height)));

                    g.Add(edges);
                }
            }

            return g;
        }
    }
}
