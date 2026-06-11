using System;
using System.Collections.Generic;
using System.IO;

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
        public int Width { get; protected set; }

        /// <summary>遮罩高度（行数）</summary>
        public int Height { get; protected set; }

        /// <summary>遮罩数据</summary>
        public CustomizedMazeMask Mask { get; protected set; }

        /// <summary>
        /// 坐标映射表：(x, y) → 顶点索引。仅白色位置有有效索引。
        /// </summary>
        private int[,] vertexMap;

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

        internal CustomizedMazeField()
        {
            Mask = null!;
            vertexMap = null!;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected override uint WriteBody(MemoryStream stream)
        {
            stream.WriteByte((byte)Width);
            stream.WriteByte((byte)Height);

            // 写入Mask数据：每个格子1个bit，true=可用，打包为字节数组
            int totalCells = Width * Height;
            int byteCount = (totalCells + 7) / 8;
            var maskData = new byte[byteCount];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Mask[y, x])
                    {
                        int bitIndex = y * Width + x;
                        maskData[bitIndex / 8] |= (byte)(1 << (bitIndex % 8));
                    }
                }
            }
            stream.Write(maskData, 0, maskData.Length);

            return 2 + (uint)maskData.Length;
        }

        protected override bool ReadBody(MemoryStream stream, ref uint size)
        {
            var w = stream.ReadByte();
            var h = stream.ReadByte();
            if (w <= 0 || h <= 0) return false;

            Width = w;
            Height = h;

            // 读取Mask数据
            int totalCells = Width * Height;
            int byteCount = (totalCells + 7) / 8;
            var maskData = new byte[byteCount];
            if (stream.Read(maskData, 0, byteCount) < byteCount) return false;

            // 从位图还原Mask
            var data = new bool[Height][];
            for (int y = 0; y < Height; y++)
            {
                data[y] = new bool[Width];
                for (int x = 0; x < Width; x++)
                {
                    int bitIndex = y * Width + x;
                    data[y][x] = (maskData[bitIndex / 8] & (1 << (bitIndex % 8))) != 0;
                }
            }
            Mask = new CustomizedMazeMask(data);

            // 构建坐标到顶点索引的映射
            vertexMap = new int[Height, Width];
            int vertexCount = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Mask[y, x])
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
            size += 2 + (uint)byteCount;
            return true;
        }
    }
}
