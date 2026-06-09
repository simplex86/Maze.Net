using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 自定义迷宫遮罩。描述迷宫中哪些位置可用（白色）哪些被屏蔽（黑色）。
    /// 数据为 bool[][]，索引为 [y][x]，true 表示可用，false 表示屏蔽。
    /// </summary>
    public class CustomizedMazeMask
    {
        /// <summary>遮罩数据。索引为 [y][x]，true 表示该位置可用</summary>
        public bool[][] Data { get; }

        /// <summary>遮罩宽度（列数）</summary>
        public int Width { get; }

        /// <summary>遮罩高度（行数）</summary>
        public int Height { get; }

        public CustomizedMazeMask(bool[][] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Height = data.Length;
            Width = data.Length > 0 ? data[0].Length : 0;
        }

        /// <summary>获取指定位置的遮罩值</summary>
        public bool this[int y, int x] => Data[y][x];
    }
}
