namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫出入口
    /// </summary>
    public struct MazeGate
    {
        public int entrance;
        public int exit;

        /// <summary>
        /// 无效索引
        /// </summary>
        public const int INVALID = -1;

        public MazeGate()
            : this(INVALID, INVALID)
        {

        }

        public MazeGate(int entrance, int exit)
        {
            this.entrance = entrance;
            this.exit = exit;
        }

        public void Reset()
        {
            entrance = INVALID;
            exit = INVALID;
        }
    }
}
