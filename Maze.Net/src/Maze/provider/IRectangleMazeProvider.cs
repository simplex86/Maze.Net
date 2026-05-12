namespace SimplexLab.Maze
{
    internal interface IRectangleMazeProvider
    {
        RectangleMazeAlgorithm algorithm { get; }

        RectangleField Create(int width, int height);
    }
}
