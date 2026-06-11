using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    public static class MazeWriter
    {
        /// <summary>
        /// 将迷宫数据写入内存流中
        /// </summary>
        /// <param name="field"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool Write(MazeField field, MemoryStream stream)
        {
            var size = 0u;
            try
            {
                size += WriteHead(field, stream);
                size += WriteBody(field, stream);
                size += WriteGrap(field, stream);
                WriteSize(stream, size);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 将迷宫数据写入内存流中
        /// </summary>
        /// <param name="field"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<bool> WriteAsync(MazeField field, MemoryStream stream)
        {
            return await Task.Run(() => Write(field, stream));
        }

        /// <summary>
        /// 将迷宫数据写入内存流中
        /// </summary>
        /// <param name="field"></param>
        /// <param name="gate"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool Write(MazeField field, MazeGate gate, MemoryStream stream)
        {
            var size = 0u;
            try
            {
                size += WriteHead(field, stream);
                size += WriteBody(field, stream);
                size += WriteGrap(field, stream);
                size += WriteGate(gate, stream);
                WriteSize(stream, size);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 将迷宫数据写入内存流中
        /// </summary>
        /// <param name="field"></param>
        /// <param name="gate"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<bool> WriteAsync(MazeField field, MazeGate gate, MemoryStream stream)
        {
            return await Task.Run(() => Write(field, gate, stream));
        }

        /// <summary>
        /// 写入头部数据
        /// </summary>
        private static uint WriteHead(MazeField field, MemoryStream stream)
        {
            stream.WriteByte((byte)'M');
            stream.WriteByte((byte)field.Shape);

            return 2U;
        }

        /// <summary>
        /// 写入迷宫数据（按类型分发）
        /// </summary>
        private static uint WriteBody(MazeField field, MemoryStream stream)
        {
            return field.Shape switch
            {
                EMazeShape.Rectangular     => WriteRectangularBody((RectangularMazeField)field, stream),
                EMazeShape.Circular        => WriteCircularBody((CircularMazeField)field, stream),
                EMazeShape.Honeycomb       => WriteHoneycombBody((HoneycombMazeField)field, stream),
                EMazeShape.Triangular      => WriteTriangularBody((TriangularMazeField)field, stream),
                EMazeShape.Hexagonal       => WriteHexagonalBody((HexagonalMazeField)field, stream),
                EMazeShape.CircularHexagon => WriteHexagonalBody((CircularHexagonMazeField)field, stream),
                EMazeShape.Stairway        => WriteStairwayBody((StairwayMazeField)field, stream),
                EMazeShape.Customized      => WriteCustomizedBody((CustomizedMazeField)field, stream),
                _ => 0U
            };
        }

        private static uint WriteRectangularBody(RectangularMazeField field, MemoryStream stream)
        {
            stream.WriteByte((byte)field.Width);
            stream.WriteByte((byte)field.Height);
            return 2;
        }

        private static uint WriteCircularBody(CircularMazeField field, MemoryStream stream)
        {
            stream.WriteByte((byte)field.Rings);
            stream.WriteByte((byte)field.Sectors);
            return 2;
        }

        private static uint WriteHoneycombBody(HoneycombMazeField field, MemoryStream stream)
        {
            stream.WriteByte((byte)field.Length);
            stream.WriteByte(0);
            return 2;
        }

        private static uint WriteTriangularBody(TriangularMazeField field, MemoryStream stream)
        {
            stream.WriteByte((byte)field.Order);
            stream.WriteByte((byte)field.Orientation);
            return 2;
        }

        private static uint WriteHexagonalBody(HexagonalMazeField field, MemoryStream stream)
        {
            stream.WriteByte((byte)field.Size);
            stream.WriteByte(0);
            return 2;
        }

        private static uint WriteStairwayBody(StairwayMazeField field, MemoryStream stream)
        {
            stream.WriteByte((byte)field.Steps);
            stream.WriteByte(0);
            return 2;
        }

        private static uint WriteCustomizedBody(CustomizedMazeField field, MemoryStream stream)
        {
            stream.WriteByte((byte)field.Width);
            stream.WriteByte((byte)field.Height);

            // 写入Mask数据：每个格子1个bit，true=可用，打包为字节数组
            int totalCells = field.Width * field.Height;
            int byteCount = (totalCells + 7) / 8;
            var maskData = new byte[byteCount];
            for (int y = 0; y < field.Height; y++)
            {
                for (int x = 0; x < field.Width; x++)
                {
                    if (field.Mask[y, x])
                    {
                        int bitIndex = y * field.Width + x;
                        maskData[bitIndex / 8] |= (byte)(1 << (bitIndex % 8));
                    }
                }
            }
            stream.Write(maskData, 0, maskData.Length);

            return 2 + (uint)maskData.Length;
        }

        /// <summary>
        /// 写入邻接表数据。
        /// 格式：4字节小端序数据长度 + 数据体（每个Adjacency的IsOpen按位打包）
        /// </summary>
        private static uint WriteGrap(MazeField field, MemoryStream stream)
        {
            // 收集所有邻接项的IsOpen状态，按顶点顺序遍历
            var bits = new List<bool>();
            foreach (var edges in field.Graph)
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
        /// 写入出入口数据
        /// </summary>
        /// <param name="gate"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static uint WriteGate(MazeGate gate, MemoryStream stream)
        {
            stream.WriteByte((byte)(gate.Entrance & 0xFF));
            stream.WriteByte((byte)((gate.Entrance >> 8) & 0xFF));
            stream.WriteByte((byte)((gate.Entrance >> 16) & 0xFF));
            stream.WriteByte((byte)((gate.Entrance >> 24) & 0xFF));

            stream.WriteByte((byte)(gate.Exit & 0xFF));
            stream.WriteByte((byte)((gate.Exit >> 8) & 0xFF));
            stream.WriteByte((byte)((gate.Exit >> 16) & 0xFF));
            stream.WriteByte((byte)((gate.Exit >> 24) & 0xFF));

            return 8u;
        }

        /// <summary>
        /// 写入总字节数（小端序），作为文件尾部的校验字段
        /// </summary>
        private static void WriteSize(MemoryStream stream, uint size)
        {
            stream.WriteByte((byte)(size & 0xFF));
            stream.WriteByte((byte)((size >> 8) & 0xFF));
            stream.WriteByte((byte)((size >> 16) & 0xFF));
            stream.WriteByte((byte)((size >> 24) & 0xFF));
        }
    }
}
