using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形地牢
    /// </summary>
    public class RectangularDungeonGenerator
    {
        private IRectangularDungeonProvider provider;

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
                                     RectangularDungeonAlgorithm algorithm = RectangularDungeonAlgorithm.Nystroms)
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
                                                      RectangularDungeonAlgorithm algorithm = RectangularDungeonAlgorithm.Nystroms)
        {
            return await Task.Run(() => Create(width, height, minRoomWidth, maxRoomWidth, minRoomHeight, maxRoomHeight, maxRoomCount, mulConnector, tortuosity, algorithm));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        private IRectangularDungeonProvider CreateProvider(RectangularDungeonAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case RectangularDungeonAlgorithm.Nystroms:
                    return new RectangularDungeonNystromsProvider();
                case RectangularDungeonAlgorithm.OverlapR:
                    return new RectangularDungeonOverlaprProvider();
                default:
                    break;
            }

            return null;
        }
    }
}