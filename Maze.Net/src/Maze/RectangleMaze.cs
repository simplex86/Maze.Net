namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫
    /// </summary>
    public class RectangleMaze
    {
        private IRectangleMazeProvider provider = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        public RectangleMaze()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public RectangleField Create(int width, int height, RectangleMazeAlgorithm algorithm = RectangleMazeAlgorithm.Prim)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = CreateProvider(algorithm);
            }
            
            var field = provider == null ? new RectangleField(width, height) 
                                         : provider.Create(width, height);
            SetEntryAndExit(field);

            return field;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public async Task<RectangleField> CreateAsync(int width, int height, RectangleMazeAlgorithm algorithm = RectangleMazeAlgorithm.Prim)
        {
            return await Task.Run(() => Create(width, height, algorithm));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        private IRectangleMazeProvider CreateProvider(RectangleMazeAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case RectangleMazeAlgorithm.DFS:
                    return new RectangleMazeDfsProvider();
                case RectangleMazeAlgorithm.Prim:
                    return new RectangleMazePrimProvider();
                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// 添加入口和出口
        /// </summary>
        private void SetEntryAndExit(RectangleField field)
        {
            // 收集所有边缘上的路径格子
            var paths = new List<Tile>();

            // 顶部边缘（第一行）
            for (int x = 1; x < field.width - 1; x++)
            {
                if (field[x, 1] == TileType.Path) paths.Add(new Tile(x, 1));
            }
            // 底部边缘（最后一行）
            for (int x = 1; x < field.width - 1; x++)
            {
                if (field[x, field.height - 2] == TileType.Path) paths.Add(new Tile(x, field.height - 2));
            }
            // 左侧边缘（第一列，排除角落）
            for (int y = 2; y < field.height - 2; y++)
            {
                if (field[1, y] == TileType.Path) paths.Add(new Tile(1, y));
            }
            // 右侧边缘（最后一列，排除角落）
            for (int y = 2; y < field.height - 2; y++)
            {
                if (field[field.width - 2, y] == TileType.Path) paths.Add(new Tile(field.width - 2, y));
            }

            if (paths.Count >= 2)
            {
                // 随机选择入口位置（优先选择顶部边缘）
                var tops = paths.Where(p => p.y == 1).ToList();
                var entry = tops.Count > 0 ? tops[Random.Shared.Next(tops.Count)] : paths[Random.Shared.Next(paths.Count)];

                // 移除入口位置，从剩余中选择出口（优先选择底部边缘）
                paths.Remove(entry);
                var bottoms = paths.Where(p => p.y == field.height - 2).ToList();
                var exit = bottoms.Count > 0 ? bottoms[Random.Shared.Next(bottoms.Count)] : paths[Random.Shared.Next(paths.Count)];

                // 标记入口和出口
                field[entry.x, entry.y] = TileType.Entry;
                field[exit.x, exit.y] = TileType.Exit;
            }
        }
    }
}