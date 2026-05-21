using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫场地
    /// 每个格子本身就是通路，通过记录横向墙和纵向墙的状态来表示迷宫
    /// </summary>
    public struct RectangularMazeField
    {
        /// <summary>
        /// 横向墙（水平墙）- 分隔格子 (x,y) 和 (x,y+1) 的墙
        /// [y][x]：表示行 y 下方的墙，即格子 (x,y) 和 (x,y+1) 之间
        /// </summary>
        private bool[][] horizontalWalls = null;

        /// <summary>
        /// 纵向墙（垂直墙）- 分隔格子 (x,y) 和 (x+1,y) 的墙
        /// [y][x]：表示列 x 右方的墙，即格子 (x,y) 和 (x+1,y) 之间
        /// </summary>
        private bool[][] verticalWalls = null;

        /// <summary>
        /// 格子数量的宽度（不是像素）
        /// </summary>
        public int width { get; private set; } = 10;

        /// <summary>
        /// 格子数量的高度（不是像素）
        /// </summary>
        public int height { get; private set; } = 10;

        /// <summary>
        /// 初始化矩形迷宫场地
        /// </summary>
        /// <param name="width">格子数量的宽度</param>
        /// <param name="height">格子数量的高度</param>
        public RectangularMazeField(int width, int height)
        {
            // 确保最小尺寸
            this.width = Math.Max(1, width);
            this.height = Math.Max(1, height);

            // 初始化墙数组
            // 横向墙：height + 1 行，每行 width 列（顶部和底部边界）
            horizontalWalls = new bool[this.height + 1][];
            // 纵向墙：height 行，每行 width + 1 列（左侧和右侧边界）
            verticalWalls = new bool[this.height][];

            // 所有墙初始化为存在（true）
            for (int y = 0; y <= this.height; y++)
            {
                if (y < this.height)
                {
                    horizontalWalls[y] = new bool[this.width];
                    verticalWalls[y] = new bool[this.width + 1];
                    for (int x = 0; x <= this.width; x++)
                    {
                        if (x < this.width)
                        {
                            horizontalWalls[y][x] = true;
                        }
                        verticalWalls[y][x] = true;
                    }
                }
                else
                {
                    horizontalWalls[y] = new bool[this.width];
                    for (int x = 0; x < this.width; x++)
                    {
                        horizontalWalls[y][x] = true;
                    }
                }
            }
        }

        /// <summary>
        /// 获取横向墙状态（分隔格子 (x,y) 和 (x,y+1)）
        /// </summary>
        public bool GetHorizontalWall(int x, int y)
        {
            // 只有 y=0 和 y=height 是真正的边界墙
            if (y == 0 || y == height)
                return true;
            // 其他墙从数组读取
            if (x < 0 || x >= width)
                return true;
            return horizontalWalls[y][x];
        }

        /// <summary>
        /// 设置横向墙状态（分隔格子 (x,y) 和 (x,y+1)）
        /// </summary>
        public void SetHorizontalWall(int x, int y, bool exists)
        {
            // 禁止修改真正的边界墙（y=0 或 y=height）
            if (y == 0 || y == height)
                return;
            // 其他墙可以修改
            if (x < 0 || x >= width)
                return;
            horizontalWalls[y][x] = exists;
        }

        /// <summary>
        /// 获取纵向墙状态（分隔格子 (x,y) 和 (x+1,y)）
        /// </summary>
        public bool GetVerticalWall(int x, int y)
        {
            // 只有 x=0 和 x=width 是真正的边界墙
            if (x == 0 || x == width)
                return true;
            // 其他墙从数组读取
            if (y < 0 || y >= height)
                return true;
            return verticalWalls[y][x];
        }

        /// <summary>
        /// 设置纵向墙状态（分隔格子 (x,y) 和 (x+1,y)）
        /// </summary>
        public void SetVerticalWall(int x, int y, bool exists)
        {
            // 禁止修改真正的边界墙（x=0 或 x=width）
            if (x == 0 || x == width)
                return;
            // 其他墙可以修改
            if (y < 0 || y >= height)
                return;
            verticalWalls[y][x] = exists;
        }

        /// <summary>
        /// 检查两个相邻格子之间是否有墙
        /// (x1,y1) 和 (x2,y2) 必须是相邻的（上下或左右相邻）
        /// </summary>
        public bool HasWallBetween(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2)
            {
                // 上下相邻，分隔 (x,y1) 和 (x,y2) 的墙是 y = Math.Max(y1, y2)
                int y = Math.Max(y1, y2);
                return GetHorizontalWall(x1, y);
            }
            else if (y1 == y2)
            {
                // 左右相邻，分隔 (x1,y) 和 (x2,y) 的墙是 x = Math.Max(x1, x2)
                int x = Math.Max(x1, x2);
                return GetVerticalWall(x, y1);
            }
            return true; // 不相邻，当作有墙
        }

        /// <summary>
        /// 移除两个相邻格子之间的墙
        /// (x1,y1) 和 (x2,y2) 必须是相邻的
        /// </summary>
        public void RemoveWallBetween(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2)
            {
                // 上下相邻，分隔 (x,y1) 和 (x,y2) 的墙是 y = Math.Max(y1, y2)
                int y = Math.Max(y1, y2);
                SetHorizontalWall(x1, y, false);
            }
            else if (y1 == y2)
            {
                // 左右相邻，分隔 (x1,y) 和 (x2,y) 的墙是 x = Math.Max(x1, x2)
                int x = Math.Max(x1, x2);
                SetVerticalWall(x, y1, false);
            }
        }

        /// <summary>
        /// 将格子坐标 (x,y) 映射到唯一的整数索引
        /// </summary>
        public int GetTileIndex(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                throw new System.ArgumentOutOfRangeException();
            return y * width + x;
        }
    }
}
