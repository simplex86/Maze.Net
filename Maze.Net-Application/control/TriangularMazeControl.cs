using SimplexLab.Maze;
using System.Windows.Forms;

namespace Maze.TApplication
{
    public partial class TriangularMazeControl : UserControl
    {
        /// <summary>
        /// ±ß³¤
        /// </summary>
        public int Length => (int)length.Value;
        /// <summary>
        /// ³¯Ïò
        /// </summary>
        public TriangleOrientation Orientation => (TriangleOrientation)(orientation.SelectedIndex + 1);
        /// <summary>
        /// 
        /// </summary>
        public int Thickness => (int)thickness.Value;

        public TriangularMazeControl()
        {
            InitializeComponent();
            orientation.SelectedIndex = 0;
        }
    }
}
