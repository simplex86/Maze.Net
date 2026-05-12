namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形地牢
    /// </summary>
    public class RectangleDungeon
    {
        private IRectangleDungeonProvider provider;

        /// <summary>
        /// 创建地牢
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="minRoomWidth"></param>
        /// <param name="maxRoomWidth"></param>
        /// <param name="minRoomHeight"></param>
        /// <param name="maxRoomHeight"></param>
        /// <param name="maxRoomCount"></param>
        /// <param name="mulConnector"></param>
        /// <param name="tortuosity"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public RectangleField Create(int width, 
                                     int height, 
                                     int minRoomWidth, 
                                     int maxRoomWidth, 
                                     int minRoomHeight, 
                                     int maxRoomHeight, 
                                     int maxRoomCount, 
                                     int mulConnector,
                                     int tortuosity,
                                     RectangleDungeonAlgorithm algorithm = RectangleDungeonAlgorithm.Nystroms)
        {
            if (provider == null || provider.algorithm != algorithm)
            {
                provider = CreateProvider(algorithm);
            }

            return provider == null ? new RectangleField()
                                    : provider.Create(width, height, minRoomWidth, maxRoomWidth, minRoomHeight, maxRoomHeight, maxRoomCount, mulConnector, tortuosity);
        }

        /// <summary>
        /// 创建地牢
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="minRoomWidth"></param>
        /// <param name="maxRoomWidth"></param>
        /// <param name="minRoomHeight"></param>
        /// <param name="maxRoomHeight"></param>
        /// <param name="maxRoomCount"></param>
        /// <param name="mulConnector"></param>
        /// <param name="tortuosity"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public async Task<RectangleField> CreateAsync(int width, 
                                                      int height, 
                                                      int minRoomWidth, 
                                                      int maxRoomWidth, 
                                                      int minRoomHeight, 
                                                      int maxRoomHeight, 
                                                      int maxRoomCount, 
                                                      int mulConnector,
                                                      int tortuosity,
                                                      RectangleDungeonAlgorithm algorithm = RectangleDungeonAlgorithm.Nystroms)
        {
            return await Task.Run(() => Create(width, height, minRoomWidth, maxRoomWidth, minRoomHeight, maxRoomHeight, maxRoomCount, mulConnector, tortuosity, algorithm));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        private IRectangleDungeonProvider CreateProvider(RectangleDungeonAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case RectangleDungeonAlgorithm.Nystroms:
                    return new RectangleDungeonNystromsProvider();
                case RectangleDungeonAlgorithm.OverlapR:
                    return new RectangleDungeonOverlaprProvider();
                default:
                    break;
            }

            return null;
        }
    }
}