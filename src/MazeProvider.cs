namespace SimpleX.Maze
{
    public static class MazeProvider
    {
        // 创建矩形迷宫
        public static RectangleMazeField CreateRectangleMaze(int width, int height)
        {
            RectangleMaze maze = new RectangleMaze(width, height);
            return maze.Create();
        }

        // 创建有房间的矩形迷宫，即地牢
        public static RectangleMazeField CreateRectangleDungeon(int width, int height, int minRoomWidth, int maxRoomWidth, int minRoomHeight, int maxRoomHeight, int maxRoomCount)
        {
            RectangleDungeon dungeon = new RectangleDungeon(width, height, minRoomWidth, maxRoomWidth, minRoomHeight, maxRoomHeight, maxRoomCount);
            return dungeon.Create();
        }
    }
}