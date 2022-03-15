namespace SimpleX.Maze
{
    public abstract class IRectangleMaze
    {
        // 创建
        public abstract RectangleMazeField Create();

        // 奇数
        protected int Odd(int value)
        {
            return (value / 2) * 2 + 1;
        }

        // 是否为墙
        protected bool IsWall(RectangleMazeField field, int x, int y)
        {
            return field[x, y] == TileType.Wall;
        }

        // 是否为迷宫的边界
        protected bool IsBorder(RectangleMazeField field, int x, int y)
        {
            return (x <= 0 || x >= field.width - 1 || y <= 0 || y >= field.height - 1);
        }
    }
}
