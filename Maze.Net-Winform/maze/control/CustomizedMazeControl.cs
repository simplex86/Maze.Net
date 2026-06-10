using System.Windows.Forms;

namespace Maze.TApplication
{
    public partial class CustomizedMazeControl : UserControl
    {
        /// <summary>
        /// ÎÄ¼þÃû
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Thickness => (int)thickness.Value;

        public CustomizedMazeControl()
        {
            InitializeComponent();
        }

        private void OnBrowerClickedHandler(object sender, System.EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "(*.bmp)|*.bmp|(*.png)|*.png|(*.jpg)|*.jpg",
                FilterIndex = 2,
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileName = dialog.FileName;
                filename.Text = FileName;
            }
        }
    }
}
