namespace SimpleX.Maze
{
    public static class MazeProvider
    {
        public static RectangleMazeField CreateRectangleMaze(int width, int height)
        {
            RectangleMaze maze = new RectangleMaze();
            return maze.Create(width, height);
        }
    }
}