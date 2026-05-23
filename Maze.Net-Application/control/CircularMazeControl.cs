using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    public partial class CircularMazeControl : UserControl
    {
        private CircularMazeGenerator generator = new CircularMazeGenerator();
        private CircularMazeField field;
        private CircularMazeRenderer renderer = new CircularMazeRenderer();

        private int offsetx = 0;
        private int offsety = 0;
        private bool generating = false;

        public CircularMazeControl()
        {
            InitializeComponent();
            algorithm.SelectedIndex = 0;
            strategy.SelectedIndex = 0;
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

            field = await generator.CreateAsync((int)rings.Value, 
                                                (int)sectors.Value, 
                                                (MazeAlgorithm)(algorithm.SelectedIndex + 1),
                                                (SectorStrategy)(strategy.SelectedIndex + 1));

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
