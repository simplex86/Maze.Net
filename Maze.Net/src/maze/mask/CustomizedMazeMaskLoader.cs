using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 自定义迷宫遮罩加载器。从图片文件加载遮罩数据。
    /// 算法流程：读取图片像素 → 判断白色/黑色 → 查找连通区域 → 合并区域 → 生成遮罩
    /// </summary>
    public class CustomizedMazeMaskLoader
    {
        #region 辅助类

        /// <summary>
        /// 遮罩加载过程中使用的辅助单元格
        /// </summary>
        private class MaskCell
        {
            public int X { get; }
            public int Y { get; }
            public bool White { get; set; }
            public int Region { get; set; } = -1;
            public MaskCell? VisitedBy { get; set; }

            public MaskCell(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        #endregion

        /// <summary>
        /// 从图片文件同步加载遮罩
        /// </summary>
        /// <param name="filename">图片文件路径</param>
        /// <param name="samples">采样间隔（0=逐像素，1=每隔1像素，2=每隔2像素，以此类推）</param>
        /// <returns>加载后的遮罩对象</returns>
        public static CustomizedMazeMask Load(string filename, int samples = 0)
        {
            using var image = Image.Load<Rgba32>(filename);

            int step = samples + 1;
            int width = (image.Width + step - 1) / step;
            int height = (image.Height + step - 1) / step;

            var cells = CreateCells(image, width, height, step);
            FindRegions(cells, width, height);
            JoinRegions(cells, width, height);
            var mask = CreateMask(cells, width, height);

            return new CustomizedMazeMask(mask);
        }

        /// <summary>
        /// 从图片文件异步加载遮罩
        /// </summary>
        /// <param name="filename">图片文件路径</param>
        /// <param name="samples">采样间隔（0=逐像素，1=每隔1像素，2=每隔2像素，以此类推）</param>
        /// <returns>加载后的遮罩对象</returns>
        public static async Task<CustomizedMazeMask> LoadAsync(string filename, int samples = 0)
        {
            return await Task.Run(() => Load(filename, samples));
        }

        #region 像素转单元格

        /// <summary>
        /// 从图像像素数据创建辅助单元格数组。
        /// 判断白色条件：Alpha >= 128 且亮度（ITU-R BT.601）>= 128
        /// </summary>
        /// <param name="image">源图像</param>
        /// <param name="width">降采样后的宽度</param>
        /// <param name="height">降采样后的高度</param>
        /// <param name="step">采样步长（1=逐像素，2=每隔1像素，以此类推）</param>
        private static MaskCell[][] CreateCells(Image<Rgba32> image, int width, int height, int step)
        {
            var cells = new MaskCell[height][];

            for (int y = height - 1; y >= 0; --y)
            {
                cells[y] = new MaskCell[width];

                for (int x = width - 1; x >= 0; --x)
                {
                    var pixel = image[x * step, y * step];
                    var cell = new MaskCell(x, y);
                    cell.White = pixel.A >= 128 && 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B >= 128;
                    cells[y][x] = cell;
                }
            }

            return cells;
        }

        #endregion

        #region 区域查找

        /// <summary>
        /// 为所有白色单元格分配连通区域编号。使用 DFS 洪水填充。
        /// </summary>
        private static void FindRegions(MaskCell[][] cells, int width, int height)
        {
            int region = 0;
            for (int y = height - 1; y >= 0; --y)
            {
                for (int x = width - 1; x >= 0; --x)
                {
                    var cell = cells[y][x];
                    if (cell.White && cell.Region < 0)
                    {
                        FillRegion(cells, cell, region++);
                    }
                }
            }
        }

        /// <summary>
        /// 从种子单元格出发，DFS 填充同一区域的所有白色单元格
        /// </summary>
        private static void FillRegion(MaskCell[][] cells, MaskCell seed, int region)
        {
            seed.Region = region;
            var stack = new Stack<MaskCell>();
            stack.Push(seed);

            while (stack.Count > 0)
            {
                var cell = stack.Pop();

                if (cell.Y > 0)
                    PushRegion(cells, stack, region, cell.X, cell.Y - 1);
                if (cell.X < cells[0].Length - 1)
                    PushRegion(cells, stack, region, cell.X + 1, cell.Y);
                if (cell.Y < cells.Length - 1)
                    PushRegion(cells, stack, region, cell.X, cell.Y + 1);
                if (cell.X > 0)
                    PushRegion(cells, stack, region, cell.X - 1, cell.Y);
            }
        }

        private static void PushRegion(MaskCell[][] cells, Stack<MaskCell> stack, int region, int x, int y)
        {
            var c = cells[y][x];
            if (c.White && c.Region < 0)
            {
                c.Region = region;
                stack.Push(c);
            }
        }

        #endregion

        #region 区域合并

        /// <summary>
        /// 使用 BFS 从所有白色边界单元格向外扩展，当不同区域的白色单元格
        /// 通过黑色区域相邻时，打通黑色像素并合并区域，确保最终只有一个连通区域。
        /// </summary>
        private static void JoinRegions(MaskCell[][] cells, int width, int height)
        {
            var queue = new Queue<MaskCell>();

            // 将所有白色单元格的未访问黑色邻居入队
            for (int y = height - 1; y >= 0; --y)
            {
                for (int x = width - 1; x >= 0; --x)
                {
                    var cell = cells[y][x];
                    if (cell.White)
                    {
                        if (y > 0)
                            Enqueue(cells, queue, cell, x, y - 1);
                        if (x < width - 1)
                            Enqueue(cells, queue, cell, x + 1, y);
                        if (y < height - 1)
                            Enqueue(cells, queue, cell, x, y + 1);
                        if (x > 0)
                            Enqueue(cells, queue, cell, x - 1, y);
                    }
                }
            }

            // BFS 扩展，合并不同区域
            while (queue.Count > 0)
            {
                var cell = queue.Dequeue();

                if (cell.Y > 0)
                    Merge(cells, width, height, queue, cell, cell.X, cell.Y - 1);
                if (cell.X < width - 1)
                    Merge(cells, width, height, queue, cell, cell.X + 1, cell.Y);
                if (cell.Y < height - 1)
                    Merge(cells, width, height, queue, cell, cell.X, cell.Y + 1);
                if (cell.X > 0)
                    Merge(cells, width, height, queue, cell, cell.X - 1, cell.Y);
            }
        }

        /// <summary>
        /// 将白色单元格的未访问黑色邻居加入队列
        /// </summary>
        private static void Enqueue(MaskCell[][] cells, Queue<MaskCell> queue, MaskCell cell, int x, int y)
        {
            var c = cells[y][x];
            if (!c.White && c.VisitedBy == null)
            {
                c.VisitedBy = cell;
                c.Region = cell.Region;
                queue.Enqueue(c);
            }
        }

        /// <summary>
        /// 处理 BFS 扩展中的一个邻居。如果邻居已被访问或为白色且属于不同区域，则合并区域；
        /// 否则将邻居加入队列继续扩展。
        /// </summary>
        private static void Merge(MaskCell[][] cells, int width, int height, Queue<MaskCell> queue, MaskCell cell, int x, int y)
        {
            var c = cells[y][x];
            if (c.VisitedBy != null || c.White)
            {
                if (c.Region != cell.Region)
                {
                    MergeRegions(cells, width, height, cell, c);
                }
            }
            else
            {
                c.VisitedBy = cell;
                c.Region = cell.Region;
                queue.Enqueue(c);
            }
        }

        /// <summary>
        /// 合并两个区域：将较大编号区域合并到较小编号区域，
        /// 并沿 visitedBy 链将路径上的黑色像素标记为白色（打通通道）
        /// </summary>
        private static void MergeRegions(MaskCell[][] cells, int width, int height, MaskCell cell, MaskCell c)
        {
            int sourceRegion;
            int targetRegion;

            if (cell.Region > c.Region)
            {
                sourceRegion = cell.Region;
                targetRegion = c.Region;
            }
            else
            {
                sourceRegion = c.Region;
                targetRegion = cell.Region;
            }

            for (int y = height - 1; y >= 0; --y)
            {
                for (int x = width - 1; x >= 0; --x)
                {
                    var current = cells[y][x];
                    if (current.Region == sourceRegion)
                    {
                        current.Region = targetRegion;
                    }
                }
            }

            // 沿 visitedBy 链打通黑色像素
            var temp = cell;
            while (temp.VisitedBy != null && !temp.White)
            {
                temp.White = true;
                temp = temp.VisitedBy;
            }
            temp = c;
            while (temp.VisitedBy != null && !temp.White)
            {
                temp.White = true;
                temp = temp.VisitedBy;
            }
        }

        #endregion

        #region 生成遮罩

        /// <summary>
        /// 从辅助单元格数组生成最终的 bool[][] 遮罩数据。
        /// 裁剪到白色像素的边界框范围。
        /// </summary>
        private static bool[][] CreateMask(MaskCell[][] cells, int width, int height)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            for (int y = height - 1; y >= 0; --y)
            {
                for (int x = width - 1; x >= 0; --x)
                {
                    if (cells[y][x].White)
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }

            int w = maxX - minX + 1;
            int h = maxY - minY + 1;
            var mask = new bool[h][];

            for (int y = h - 1; y >= 0; --y)
            {
                mask[y] = new bool[w];
                for (int x = w - 1; x >= 0; --x)
                {
                    mask[y][x] = cells[minY + y][minX + x].White;
                }
            }

            return mask;
        }

        #endregion
    }
}
