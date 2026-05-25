using System.Windows.Forms;

namespace Maze.TApplication
{
    public partial class RectangularMazeControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public int Width => (int)width.Value;
        /// <summary>
        /// 
        /// </summary>
        public int Height => (int)height.Value;
        /// <summary>
        /// 
        /// </summary>
        public int Thickness => (int)thickness.Value;

        public RectangularMazeControl()
        {
            InitializeComponent();
        }
    }
}
