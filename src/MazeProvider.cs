namespace SimpleX.Maze
{
    public static class MazeProvider
    {
        public static RectangleMazeField CreateRectangleMaze(int width, int height)
        {
            RectangleMaze maze = new RectangleMaze();
            return maze.Create(width, height);
        }

        public static RectangleMazeField CreateRectangleWithRoomMaze(int width, int height, int minRoomWidth, int maxRoomWidth, int minRoomHeight, int maxRoomHeight, int maxRoomCount)
        {
            RectangleWithRoomMaze maze = new RectangleWithRoomMaze();
            return maze.Create(width, height, minRoomWidth, maxRoomWidth, minRoomHeight, maxRoomHeight, maxRoomCount);
        }
    }
}