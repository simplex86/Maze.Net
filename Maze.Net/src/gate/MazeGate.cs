namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫出入口
    /// </summary>
    public struct MazeGate
    {
        public int Entrance;
        public int Exit;

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
            this.Entrance = entrance;
            this.Exit = exit;
        }

        public void Reset()
        {
            Entrance = INVALID;
            Exit = INVALID;
        }
    }
}
