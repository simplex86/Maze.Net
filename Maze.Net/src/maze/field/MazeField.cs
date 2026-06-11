using System;
using System.Collections.Generic;
using System.IO;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫场地基类
    /// </summary>
    public class MazeField
    {
        /// <summary>
        /// 迷宫的形状
        /// </summary>
        public EMazeShape Shape { get; protected set; }

        /// <summary>
        /// 顶点总数
        /// </summary>
        public int VertexCount { get; protected set; }

        /// <summary>
        /// 邻接表（图）
        /// </summary>
        internal List<List<Adjacency>> Graph { get; set; }

        private CoordinateBounds? bounds;

        /// <summary>
        /// 坐标范围（延迟计算）
        /// </summary>
        internal CoordinateBounds Bounds
        {
            get
            {
                if (!bounds.HasValue) bounds = ComputeBounds();
                return bounds.Value;
            }
        }

        /// <summary>
        /// Y轴是否需要翻转（默认false，Y朝下的坐标系）
        /// </summary>
        public virtual bool FlipY => false;

        protected MazeField()
        {

        }

        /// <summary>
        /// 获取指定顶点对应格子的几何形状，用于渲染出入口标记。
        /// 子类应覆写此方法以提供特定形状的格子几何信息。
        /// </summary>
        /// <param name="vertex">顶点索引</param>
        /// <returns>格子的几何形状</returns>
        internal virtual CellShape GetCellShape(int vertex)
        {
            return CellShape.Polygon(new Vertex[0]);
        }

        /// <summary>
        /// 遍历邻接表中所有边界，计算几何包围盒
        /// </summary>
        private CoordinateBounds ComputeBounds()
        {
            double minx = double.MaxValue, miny = double.MaxValue;
            double maxx = double.MinValue, maxy = double.MinValue;

            foreach (var edges in Graph)
            {
                foreach (var edge in edges)
                {
                    if (edge.Border is LineBorder line)
                    {
                        minx = Math.Min(minx, Math.Min(line.X1, line.X2));
                        maxx = Math.Max(maxx, Math.Max(line.X1, line.X2));
                        miny = Math.Min(miny, Math.Min(line.Y1, line.Y2));
                        maxy = Math.Max(maxy, Math.Max(line.Y1, line.Y2));
                    }
                    else if (edge.Border is ArcBorder arc)
                    {
                        minx = Math.Min(minx, arc.CenterX - arc.Radius);
                        maxx = Math.Max(maxx, arc.CenterX + arc.Radius);
                        miny = Math.Min(miny, arc.CenterY - arc.Radius);
                        maxy = Math.Max(maxy, arc.CenterY + arc.Radius);
                    }
                }
            }

            return new CoordinateBounds(minx, miny, maxx, maxy);
        }

        /// <summary>
        /// 写为二进制数据
        /// </summary>
        /// <returns></returns>
        public MemoryStream ToBinary()
        {
            var ms = new MemoryStream();

            var size = 0u;
            size += WriteHead(ms);
            size += WriteBody(ms);
            size += WriteGrap(ms);
            WriteSize(ms, size);

            return ms;
        }

        /// <summary>
        /// 写入头部数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>写入的字节数</returns>
        private uint WriteHead(MemoryStream stream)
        {
            stream.WriteByte((byte)'M');
            stream.WriteByte((byte)Shape);

            return 2U;
        }

        /// <summary>
        /// 写入迷宫数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>写入的字节数</returns>
        protected virtual uint WriteBody(MemoryStream stream)
        {
            return 0U;
        }

        /// <summary>
        /// 写入邻接表数据。
        /// 格式：4字节小端序数据长度 + 数据体（每个Adjacency的IsOpen按位打包）
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>写入的字节数</returns>
        private uint WriteGrap(MemoryStream stream)
        {
            // 收集所有邻接项的IsOpen状态，按顶点顺序遍历
            var bits = new List<bool>();
            foreach (var edges in Graph)
            {
                foreach (var v in edges)
                {
                    bits.Add(v.IsOpen);
                }
            }

            // 将位打包为字节数组（LSB优先）
            int count = (bits.Count + 7) / 8;
            var datas = new byte[count];
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i]) datas[i / 8] |= (byte)(1 << (i % 8));
            }

            // 写入4字节小端序数据长度
            stream.WriteByte((byte)(count & 0xFF));
            stream.WriteByte((byte)((count >> 8) & 0xFF));
            stream.WriteByte((byte)((count >> 16) & 0xFF));
            stream.WriteByte((byte)((count >> 24) & 0xFF));

            // 写入数据体
            stream.Write(datas, 0, datas.Length);

            return 4 + (uint)datas.Length;
        }

        /// <summary>
        /// 写入总字节数（小端序），作为文件尾部的校验字段
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size">总字节数（不含本字段自身）</param>
        private void WriteSize(MemoryStream stream, uint size)
        {
            stream.WriteByte((byte)(size & 0xFF));
            stream.WriteByte((byte)((size >> 8) & 0xFF));
            stream.WriteByte((byte)((size >> 16) & 0xFF));
            stream.WriteByte((byte)((size >> 24) & 0xFF));
        }

        /// <summary>
        /// 从二进制数据重建迷宫（静态工厂方法）
        /// </summary>
        /// <param name="array">二进制字节数组</param>
        /// <returns>重建的迷宫实例，失败返回null</returns>
        public static MazeField? FromBinary(byte[] array)
        {
            using var ms = new MemoryStream(array);
            return FromBinary(ms);
        }

        /// <summary>
        /// 从二进制数据重建迷宫（静态工厂方法）
        /// </summary>
        /// <param name="stream">二进制数据流</param>
        /// <returns>重建的迷宫实例，失败返回null</returns>
        public static MazeField? FromBinary(MemoryStream stream)
        {
            // 读取并验证魔数
            if (stream.ReadByte() != (byte)'M') return null;

            // 读取迷宫类型
            int shapeByte = stream.ReadByte();
            if (shapeByte < 0 || shapeByte > (int)EMazeShape.Customized) return null;
            var shape = (EMazeShape)shapeByte;

            // 根据类型创建空白实例
            MazeField field = shape switch
            {
                EMazeShape.Rectangular => new RectangularMazeField(),
                EMazeShape.Circular => new CircularMazeField(),
                EMazeShape.Honeycomb => new HoneycombMazeField(),
                EMazeShape.Hexagonal => new HexagonalMazeField(),
                EMazeShape.CircularHexagon => new CircularHexagonMazeField(),
                EMazeShape.Triangular => new TriangularMazeField(),
                EMazeShape.Stairway => new StairwayMazeField(),
                EMazeShape.Customized => new CustomizedMazeField(),
                _ => null!
            };
            if (field == null!) return null;

            field.Shape = shape;

            var size = 2u;

            // 读取Body数据
            if (!field.ReadBody(stream, ref size)) return null;

            // 读取邻接表数据
            if (!field.ReadGrap(stream, ref size)) return null;

            // 读取并验证总字节数
            if (!field.ReadSize(stream, size)) return null;

            return field;
        }

        /// <summary>
        /// 读取迷宫数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size">累计读取的总字节数</param>
        /// <returns>是否成功</returns>
        protected virtual bool ReadBody(MemoryStream stream, ref uint size)
        {
            return true;
        }

        /// <summary>
        /// 读取邻接表数据。
        /// 格式：4字节小端序数据长度 + IsOpen位图数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size">累计读取的总字节数</param>
        /// <returns>是否成功</returns>
        private bool ReadGrap(MemoryStream stream, ref uint size)
        {
            // 读取4字节小端序数据长度
            var lenBytes = new byte[4];
            if (stream.Read(lenBytes, 0, 4) < 4) return false;
            int dataLen = lenBytes[0] | (lenBytes[1] << 8) | (lenBytes[2] << 16) | (lenBytes[3] << 24);
            size += 4;

            // 读取数据体
            var graphData = new byte[dataLen];
            if (stream.Read(graphData, 0, dataLen) < dataLen) return false;
            size += (uint)dataLen;

            // 按写入顺序遍历邻接表，还原IsOpen状态
            int bitIndex = 0;
            foreach (var edges in Graph)
            {
                foreach (var adj in edges)
                {
                    adj.IsOpen = (graphData[bitIndex / 8] & (1 << (bitIndex % 8))) != 0;
                    bitIndex++;
                }
            }

            return true;
        }

        /// <summary>
        /// 读取总字节数（小端序），并与累计字节数比较是否相等（不含字段本身）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size">累计读取的总字节数（不含本字段）</param>
        /// <returns>是否成功</returns>
        private bool ReadSize(MemoryStream stream, uint size)
        {
            var sizeBytes = new byte[4];
            if (stream.Read(sizeBytes, 0, 4) < 4) return false;
            uint storedSize = (uint)(sizeBytes[0] | (sizeBytes[1] << 8) | (sizeBytes[2] << 16) | (sizeBytes[3] << 24));
            return storedSize == size;
        }
    }
}
