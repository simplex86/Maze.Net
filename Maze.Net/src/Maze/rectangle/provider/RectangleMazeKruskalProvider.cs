namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于Kruskal最小生成树算法生成随机迷宫
    /// </summary>
    public class RectangleMazeKruskalProvider : IRectangleMazeProvider
    {
        /// <summary>
        /// 墙及其两侧的路径格子
        /// </summary>
        private struct Wall
        {
            public int x1, y1; // 第一个路径格子坐标（奇数）
            public int x2, y2; // 第二个路径格子坐标（奇数）
            public int wx, wy; // 墙的坐标（偶数）

            public Wall(int x1, int y1, int x2, int y2, int wx, int wy)
            {
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
                this.wx = wx;
                this.wy = wy;
            }
        }

        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public RectangleMazeAlgorithm algorithm { get; } = RectangleMazeAlgorithm.Kruskal;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangleField Create(int width, int height)
        {
            // 确保尺寸为奇数（墙-路径交替结构要求）
            width = Utils.Odd(width);
            height = Utils.Odd(height);

            // 创建迷宫场地，初始化为全墙
            var field = new RectangleField(width, height);

            // 计算路径格子的数量（奇数坐标位置）
            int pathCount = (width / 2) * (height / 2);
            
            // 初始化并查集，每个路径格子是一个独立的集合
            var dsu = new DisjointSet(pathCount);

            // 收集所有可能打通的内部墙
            var walls = CollectInternalWalls(width, height);

            // 随机打乱墙的顺序（Kruskal算法的关键）
            ShuffleWalls(walls);

            // Kruskal算法主循环：遍历所有墙，尝试打通
            foreach (var wall in walls)
            {
                // 获取墙两侧的路径格子在并查集中的索引
                int cell1 = GetCellIndex(wall.x1, wall.y1, width);
                int cell2 = GetCellIndex(wall.x2, wall.y2, width);

                // 如果两个格子不在同一连通分量，则打通这堵墙
                if (dsu.Union(cell1, cell2))
                {
                    // 标记两个路径格子
                    field[wall.x1, wall.y1] = TileType.Path;
                    field[wall.x2, wall.y2] = TileType.Path;
                    // 打通中间的墙
                    field[wall.wx, wall.wy] = TileType.Path;
                }

                // 优化：所有格子已连通时提前退出
                if (dsu.Count == 1)
                    break;
            }

            return field;
        }

        /// <summary>
        /// 收集所有内部墙（水平墙和垂直墙）
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>墙的列表</returns>
        private List<Wall> CollectInternalWalls(int width, int height)
        {
            var walls = new List<Wall>();

            // 收集水平墙（分隔上下两个路径格子的墙）
            // 水平墙的y坐标是偶数，位于两个奇数y坐标的路径格子之间
            for (int x = 1; x < width - 1; x += 2)
            {
                for (int y = 2; y < height - 1; y += 2)
                {
                    // 墙位于 (x, y)，分隔 (x, y-1) 和 (x, y+1)
                    walls.Add(new Wall(x, y - 1, x, y + 1, x, y));
                }
            }

            // 收集垂直墙（分隔左右两个路径格子的墙）
            // 垂直墙的x坐标是偶数，位于两个奇数x坐标的路径格子之间
            for (int x = 2; x < width - 1; x += 2)
            {
                for (int y = 1; y < height - 1; y += 2)
                {
                    // 墙位于 (x, y)，分隔 (x-1, y) 和 (x+1, y)
                    walls.Add(new Wall(x - 1, y, x + 1, y, x, y));
                }
            }

            return walls;
        }

        /// <summary>
        /// Fisher-Yates 洗牌算法：随机打乱墙的顺序
        /// </summary>
        /// <param name="walls">墙的列表</param>
        private void ShuffleWalls(List<Wall> walls)
        {
            int n = walls.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // 交换 i 和 j 位置的墙
                var temp = walls[i];
                walls[i] = walls[j];
                walls[j] = temp;
            }
        }

        /// <summary>
        /// 将路径格子坐标转换为并查集索引
        /// 路径格子坐标为奇数，需要映射到 0 ~ (pathCount-1)
        /// </summary>
        /// <param name="x">路径格子X坐标（奇数）</param>
        /// <param name="y">路径格子Y坐标（奇数）</param>
        /// <param name="width">迷宫宽度</param>
        /// <returns>并查集索引</returns>
        private int GetCellIndex(int x, int y, int width)
        {
            // 将奇数坐标转换为索引：(x-1)/2 + (y-1)/2 * (width/2)
            int col = (x - 1) / 2;
            int row = (y - 1) / 2;
            int cols = width / 2;
            return col + row * cols;
        }
    }
}