using System.Windows.Forms;

namespace Maze.TApplication
{
    public partial class HoneycombMazeControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public int Length => (int)length.Value;
        /// <summary>
        /// 
        /// </summary>
        public int Thickness => (int)thickness.Value;

        public HoneycombMazeControl()
        {
            InitializeComponent();
        }
    }
}
