using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 自定义迷宫场地（基于遮罩的矩形网格）。
    /// 只为遮罩中白色位置创建顶点，黑色位置视为不存在。
    /// 白色格子与黑色格子相邻处视为迷宫外墙（Neighbor == -1）。
    /// </summary>
    public class CustomizedMazeField : MazeField
    {
        /// <summary>遮罩宽度（列数）</summary>
        public int Width { get; }

        /// <summary>遮罩高度（行数）</summary>
        public int Height { get; }

        /// <summary>遮罩数据</summary>
        public CustomizedMazeMask Mask { get; }

        /// <summary>
        /// 坐标映射表：(x, y) → 顶点索引。仅白色位置有有效索引。
        /// </summary>
        private readonly int[,] vertexMap;

        public CustomizedMazeField(CustomizedMazeMask mask)
        {
            Mask = mask ?? throw new ArgumentNullException(nameof(mask));
            Width = mask.Width;
            Height = mask.Height;
            Shape = EMazeShape.Customized;

            // 构建坐标到顶点索引的映射
            vertexMap = new int[Height, Width];
            int vertexCount = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (mask[y, x])
                    {
                        vertexMap[y, x] = vertexCount++;
                    }
                    else
                    {
                        vertexMap[y, x] = -1;
                    }
                }
            }

            VertexCount = vertexCount;
            Graph = BuildGraph();
        }

        /// <summary>
        /// 获取指定坐标的顶点索引。如果是黑色位置，返回 -1。
        /// </summary>
        public int GetVertexIndex(int x, int y)
        {
            return vertexMap[y, x];
        }

        internal override CellShape GetCellShape(int vertex)
        {
            // 从顶点索引反推 (x, y) 坐标
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (vertexMap[y, x] == vertex)
                    {
                        return CellShape.Polygon(new Vertex[]
                        {
                            new Vertex(x, y),
                            new Vertex(x + 1, y),
                            new Vertex(x + 1, y + 1),
                            new Vertex(x, y + 1),
                        });
                    }
                }
            }
            return CellShape.Polygon(new Vertex[0]);
        }

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(VertexCount);

            // 先按顶点索引顺序预分配
            for (int i = 0; i < VertexCount; i++)
            {
                g.Add(null!);
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (!Mask[y, x]) continue;

                    int v = vertexMap[y, x];
                    var edges = new List<Adjacency>();

                    // 右邻居
                    if (x < Width - 1 && Mask[y, x + 1])
                    {
                        edges.Add(new Adjacency(vertexMap[y, x + 1], new LineBorder(x + 1, y, x + 1, y + 1)));
                    }
                    else
                    {
                        // 右侧为黑色或越界 → 外墙
                        edges.Add(new Adjacency(-1, new LineBorder(x + 1, y, x + 1, y + 1)));
                    }

                    // 左邻居
                    if (x > 0 && Mask[y, x - 1])
                    {
                        edges.Add(new Adjacency(vertexMap[y, x - 1], new LineBorder(x, y, x, y + 1)));
                    }
                    else
                    {
                        edges.Add(new Adjacency(-1, new LineBorder(x, y, x, y + 1)));
                    }

                    // 下邻居
                    if (y < Height - 1 && Mask[y + 1, x])
                    {
                        edges.Add(new Adjacency(vertexMap[y + 1, x], new LineBorder(x, y + 1, x + 1, y + 1)));
                    }
                    else
                    {
                        edges.Add(new Adjacency(-1, new LineBorder(x, y + 1, x + 1, y + 1)));
                    }

                    // 上邻居
                    if (y > 0 && Mask[y - 1, x])
                    {
                        edges.Add(new Adjacency(vertexMap[y - 1, x], new LineBorder(x, y, x + 1, y)));
                    }
                    else
                    {
                        edges.Add(new Adjacency(-1, new LineBorder(x, y, x + 1, y)));
                    }

                    g[v] = edges;
                }
            }

            return g;
        }
    }
}
