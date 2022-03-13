namespace SimpleX.Maze
{
    public static class MazeProvider
    {
        // 创建矩形迷宫
        public static RectangleMazeField CreateRectangleMaze(int width, int height)
        {
            RectangleMaze maze = new RectangleMaze();
            return maze.Create(width, height);
        }

        // 创建有房间的矩形迷宫，即地牢
        public static RectangleMazeField CreateRectangleWithRoomMaze(int width, int height, int minRoomWidth, int maxRoomWidth, int minRoomHeight, int maxRoomHeight, int maxRoomCount)
        {
            RectangleWithRoomMaze maze = new RectangleWithRoomMaze();
            return maze.Create(width, height, minRoomWidth, maxRoomWidth, minRoomHeight, maxRoomHeight, maxRoomCount);
        }
    }
}