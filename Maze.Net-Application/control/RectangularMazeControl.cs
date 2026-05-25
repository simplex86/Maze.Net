using SimplexLab.Maze;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze.TApplication
{
    public partial class RectangularMazeControl : UserControl
    {
        private RectangularMazeGenerator generator = new RectangularMazeGenerator();
        private RectangularMazeField? field;
        private RectangularMazeRenderer renderer = new RectangularMazeRenderer();

        private int offsetx = 0;
        private int offsety = 0;
        private bool generating = false;

        public RectangularMazeControl()
        {
            InitializeComponent();
            algorithm.SelectedIndex = 0;
        }

        private void OnGenerationClickedHandler(object sender, System.EventArgs e)
        {
            if (generating) return;

            offsetx = 0;
            offsety = 0;

            OnGenerationClickedHandler();
        }

        private async Task OnGenerationClickedHandler()
        {
            PrevProcess();
            {
                await Generate();
            }
            PostProcess();
        }

        private void PrevProcess()
        {
            generation.Text = "...";
            generation.Enabled = false;
        }

        private async Task Generate()
        {
            generating = true;

            var t = (int)thickness.Value;
            var w = (width.Value == 0) ? canvas.Width / t - 1 : (int)width.Value;
            var h = (height.Value == 0) ? canvas.Height / t - 1 : (int)height.Value;
            var alm = (MazeAlgorithm)(algorithm.SelectedIndex + 1);

            field = await generator.CreateAsync(w, h, alm);

            generating = false;
        }

        private void PostProcess()
        {
            canvas.Refresh();

            generation.Text = "Generate";
            generation.Enabled = true;
        }

        private void OnCanvasPaintHandler(object sender, PaintEventArgs e)
        {
            renderer.SetSize(canvas.Width, canvas.Height)
                    .SetThickness((int)thickness.Value)
                    .SetOffset(offsetx, offsety)
                    .Draw(e.Graphics, field);
        }
    }
}
