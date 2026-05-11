namespace SimplexLab.Maze
{
    public static class MazeProvider
    {
        /// <summary>
        /// 创建矩形迷宫
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static RectangleMazeField CreateRectangleMaze(int width, int height)
        {
            RectangleMaze maze = new RectangleMaze(width, height);
            return maze.Create();
        }

        /// <summary>
        /// 创建有房间的矩形迷宫，即地牢
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="minRoomWidth"></param>
        /// <param name="maxRoomWidth"></param>
        /// <param name="minRoomHeight"></param>
        /// <param name="maxRoomHeight"></param>
        /// <param name="maxRoomCount"></param>
        /// <param name="tortuosity"></param>
        /// <returns></returns>
        public static RectangleMazeField CreateRectangleDungeon(int width, int height, int minRoomWidth, int maxRoomWidth, int minRoomHeight, int maxRoomHeight, int maxRoomCount, int tortuosity = 50)
        {
            RectangleDungeon dungeon = new RectangleDungeon(width, height, minRoomWidth, maxRoomWidth, minRoomHeight, maxRoomHeight, maxRoomCount, tortuosity);
            return dungeon.Create();
        }
    }
}