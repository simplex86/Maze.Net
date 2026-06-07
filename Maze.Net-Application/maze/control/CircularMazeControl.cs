using System.Windows.Forms;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    public partial class CircularMazeControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public int Rings => (int)rings.Value;
        /// <summary>
        /// 
        /// </summary>
        public int Sectors => (int)sectors.Value;
        /// <summary>
        /// 
        /// </summary>
        public int Thickness => (int)thickness.Value;

        public CircularMazeControl()
        {
            InitializeComponent();
        }
    }
}
