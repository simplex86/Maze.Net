namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫出入口
    /// </summary>
    public class MazeGate
    {
        public int Entrance;
        public int Exit;

        /// <summary>
        /// 入口处打开的朝外墙壁的边框
        /// </summary>
        internal IMazeBorder? EntranceBorder;

        /// <summary>
        /// 出口处打开的朝外墙壁的边框
        /// </summary>
        internal IMazeBorder? ExitBorder;

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
            this.EntranceBorder = null;
            this.ExitBorder = null;
        }

        public void Reset()
        {
            Entrance = INVALID;
            Exit = INVALID;
            EntranceBorder = null;
            ExitBorder = null;
        }
    }
}
