using System;
using System.IO;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    public static class MazeReader
    {
        /// <summary>
        /// 从内存流中重建迷宫数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static (MazeField field, MazeGate gate) Read(MemoryStream stream)
        {
            MazeField? field = null;
            MazeGate? gate = null;

            try
            {
                // 读取并验证魔数
                if (stream.ReadByte() != (byte)'M') return (null, null);

                // 读取迷宫类型
                int shapeByte = stream.ReadByte();
                if (shapeByte < 0 || shapeByte > (int)EMazeShape.Customized) return (null, null);
                var shape = (EMazeShape)shapeByte;

                // 根据类型创建空白实例
                field = shape switch
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
                if (field == null!) return (null, null);
                field.Shape = shape;

                gate = new MazeGate();

                var size = 2u;
                // 读取Body数据
                if (!ReadBody(field, stream, ref size)) return (null, null);
                // 读取邻接表数据
                if (!ReadGrap(field, stream, ref size)) return (null, null);
                // 读取Gate数据
                if (!ReadGate(gate, stream, ref size)) return (null, null);
                // 读取并验证总字节数
                if (!ReadSize(stream, size)) return (null, null);
            }
            catch (Exception ex)
            {
                field = null;
                gate = null;
            }

            return (field, gate);
        }

        /// <summary>
        /// 从内存流中重建迷宫数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<(MazeField field, MazeGate gate)> ReadAsync(MemoryStream stream)
        {
            return await Task.Run(() => Read(stream));
        }

        /// <summary>
        /// 读取迷宫数据（按类型分发）
        /// </summary>
        private static bool ReadBody(MazeField field, MemoryStream stream, ref uint size)
        {
            return field.Shape switch
            {
                EMazeShape.Rectangular     => ReadRectangularBody((RectangularMazeField)field, stream, ref size),
                EMazeShape.Circular        => ReadCircularBody((CircularMazeField)field, stream, ref size),
                EMazeShape.Honeycomb       => ReadHoneycombBody((HoneycombMazeField)field, stream, ref size),
                EMazeShape.Triangular      => ReadTriangularBody((TriangularMazeField)field, stream, ref size),
                EMazeShape.Hexagonal       => ReadHexagonalBody((HexagonalMazeField)field, stream, ref size),
                EMazeShape.CircularHexagon => ReadCircularHexagonBody((CircularHexagonMazeField)field, stream, ref size),
                EMazeShape.Stairway        => ReadStairwayBody((StairwayMazeField)field, stream, ref size),
                EMazeShape.Customized      => ReadCustomizedBody((CustomizedMazeField)field, stream, ref size),
                _ => false
            };
        }

        private static bool ReadRectangularBody(RectangularMazeField field, MemoryStream stream, ref uint size)
        {
            var w = stream.ReadByte();
            var h = stream.ReadByte();
            if (w <= 0 || h <= 0) return false;

            field.Width = w;
            field.Height = h;
            field.VertexCount = w * h;
            field.Graph = field.BuildGraph();
            size += 2;
            return true;
        }

        private static bool ReadCircularBody(CircularMazeField field, MemoryStream stream, ref uint size)
        {
            var rings = stream.ReadByte();
            var sectors = stream.ReadByte();
            if (rings <= 0 || (sectors > 0 && sectors < 3)) return false;

            field.Rings = rings;
            field.Sectors = sectors;

            // 重新计算SectorsPerRing
            field.SectorsPerRing = new int[rings];
            field.SectorsPerRing[0] = 3;

            if (sectors > 0)
            {
                var normalizedMaxSectors = 3;
                while (normalizedMaxSectors * 2 <= sectors)
                    normalizedMaxSectors *= 2;

                for (var r = 1; r < rings; r++)
                {
                    field.SectorsPerRing[r] = field.SectorsPerRing[r - 1];
                    var arcLength = (2 * Math.PI * (r + 1)) / field.SectorsPerRing[r - 1];
                    if (arcLength > 2.0 && field.SectorsPerRing[r] * 2 <= normalizedMaxSectors)
                        field.SectorsPerRing[r] *= 2;
                }
                if (field.SectorsPerRing[rings - 1] < normalizedMaxSectors)
                    field.SectorsPerRing[rings - 1] = normalizedMaxSectors;
            }
            else
            {
                // 不设上限，仅受弧长条件约束
                for (var r = 1; r < rings; r++)
                {
                    field.SectorsPerRing[r] = field.SectorsPerRing[r - 1];
                    var arcLength = (2 * Math.PI * (r + 1)) / field.SectorsPerRing[r - 1];
                    if (arcLength > 2.0)
                        field.SectorsPerRing[r] *= 2;
                }
            }

            field.VertexCount = 0;
            for (var r = 0; r < rings; r++)
                field.VertexCount += field.SectorsPerRing[r];

            field.Graph = field.BuildGraph();
            size += 2;
            return true;
        }

        private static bool ReadHoneycombBody(HoneycombMazeField field, MemoryStream stream, ref uint size)
        {
            var length = stream.ReadByte();
            var padding = stream.ReadByte();
            if (length <= 0) return false;

            field.Length = length;
            field.VertexCount = 3 * length * (length - 1) + 1;
            field.Graph = field.BuildGraph();
            size += 2;
            return true;
        }

        private static bool ReadTriangularBody(TriangularMazeField field, MemoryStream stream, ref uint size)
        {
            var order = stream.ReadByte();
            var orientation = stream.ReadByte();
            if (order <= 0) return false;
            if (orientation != (int)ETriangleOrientation.Upward &&
                orientation != (int)ETriangleOrientation.Downward) return false;

            field.Order = order;
            field.Orientation = (ETriangleOrientation)orientation;
            field.VertexCount = order * order;
            field.Graph = field.BuildGraph();
            size += 2;
            return true;
        }

        private static bool ReadHexagonalBody(HexagonalMazeField field, MemoryStream stream, ref uint size)
        {
            var sz = stream.ReadByte();
            var padding = stream.ReadByte();
            if (sz <= 0) return false;

            field.Size = sz;
            field.VertexCount = 6 * sz * sz;
            field.Graph = field.BuildGraph();
            size += 2;
            return true;
        }

        private static bool ReadCircularHexagonBody(CircularHexagonMazeField field, MemoryStream stream, ref uint size)
        {
            if (!ReadHexagonalBody(field, stream, ref size)) return false;
            field.Shape = EMazeShape.CircularHexagon;
            return true;
        }

        private static bool ReadStairwayBody(StairwayMazeField field, MemoryStream stream, ref uint size)
        {
            var steps = stream.ReadByte();
            var padding = stream.ReadByte();
            if (steps <= 0) return false;

            field.Steps = steps;
            field.VertexCount = steps * (steps + 1) / 2;
            field.Graph = field.BuildGraph();
            size += 2;
            return true;
        }

        private static bool ReadCustomizedBody(CustomizedMazeField field, MemoryStream stream, ref uint size)
        {
            var w = stream.ReadByte();
            var h = stream.ReadByte();
            if (w <= 0 || h <= 0) return false;

            field.Width = w;
            field.Height = h;

            // 读取Mask数据
            int totalCells = w * h;
            int byteCount = (totalCells + 7) / 8;
            var maskData = new byte[byteCount];
            if (stream.Read(maskData, 0, byteCount) < byteCount) return false;

            // 从位图还原Mask
            var data = new bool[h][];
            for (int y = 0; y < h; y++)
            {
                data[y] = new bool[w];
                for (int x = 0; x < w; x++)
                {
                    int bitIndex = y * w + x;
                    data[y][x] = (maskData[bitIndex / 8] & (1 << (bitIndex % 8))) != 0;
                }
            }
            field.Mask = new CustomizedMazeMask(data);

            // 构建坐标到顶点索引的映射
            field.VertexMap = new int[h, w];
            int vertexCount = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (field.Mask[y, x])
                    {
                        field.VertexMap[y, x] = vertexCount++;
                    }
                    else
                    {
                        field.VertexMap[y, x] = -1;
                    }
                }
            }

            field.VertexCount = vertexCount;
            field.Graph = field.BuildGraph();
            size += 2 + (uint)byteCount;
            return true;
        }

        /// <summary>
        /// 读取邻接表数据。
        /// 格式：4字节小端序数据长度 + IsOpen位图数据
        /// </summary>
        private static bool ReadGrap(MazeField field, MemoryStream stream, ref uint size)
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
            foreach (var edges in field.Graph)
            {
                foreach (var v in edges)
                {
                    v.IsOpen = (graphData[bitIndex / 8] & (1 << (bitIndex % 8))) != 0;
                    bitIndex++;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gate"></param>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static bool ReadGate(MazeGate gate, MemoryStream stream, ref uint size)
        {
            if (stream.Position + 12 > stream.Length) return false;

            var gateBytes = new byte[8];
            if (stream.Read(gateBytes, 0, 8) != 8) return false;

            gate.Entrance = gateBytes[0] | (gateBytes[1] << 8) | (gateBytes[2] << 16) | (gateBytes[3] << 24);
            gate.Exit = gateBytes[4] | (gateBytes[5] << 8) | (gateBytes[6] << 16) | (gateBytes[7] << 24);
            size += 8;

            return true;
        }

        /// <summary>
        /// 读取总字节数（小端序），并与累计字节数比较是否相等（不含字段本身）
        /// </summary>
        private static bool ReadSize(MemoryStream stream, uint size)
        {
            var sizeBytes = new byte[4];
            if (stream.Read(sizeBytes, 0, 4) < 4) return false;
            uint storedSize = (uint)(sizeBytes[0] | (sizeBytes[1] << 8) | (sizeBytes[2] << 16) | (sizeBytes[3] << 24));
            return storedSize == size;
        }
    }
}
