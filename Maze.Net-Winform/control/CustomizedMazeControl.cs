using System.Windows.Forms;

namespace Maze.TApplication
{
    public partial class CustomizedMazeControl : UserControl
    {
        /// <summary>
        /// �ļ���
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Thickness => (int)thickness.Value;

        /// <summary>
        /// 采样间隔（0=逐像素，1=每隔1像素，2=每隔2像素，以此类推）
        /// </summary>
        public int Samples => (int)samples.Value;

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
