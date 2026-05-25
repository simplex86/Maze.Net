using System.Windows.Forms;

namespace Maze.TApplication
{
    public partial class CircularHexagonControl : UserControl
    {
        /// <summary>
        /// ±ß³¤
        /// </summary>
        public int Length => (int)length.Value;
        /// <summary>
        /// 
        /// </summary>
        public int Thickness => (int)thickness.Value;

        public CircularHexagonControl()
        {
            InitializeComponent();
        }
    }
}
