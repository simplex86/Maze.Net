using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    public partial class MainForm : Form
    {
        private MazeShape mazeShape = MazeShape.Rectangular;
        private RectangularMazeField rectangularMazeField = null;
        private CircularMazeField circularMazeField = null;
        private HoneycombMazeField honeycombMazeField = null;

        private List<Control> controls = new List<Control>();

        public MainForm()
        {
            InitializeComponent();

            controls.Add(rectangularMazeControl);
            controls.Add(circularMazeControl);
            controls.Add(honeycombMazeControl);

            shape.SelectedIndex = (int)mazeShape;
            algorithm.SelectedIndex = (int)MazeAlgorithm.Kruskal - 1;
        }

        private void OnShapeChangedHandler(object sender, EventArgs e)
        {
            mazeShape = (MazeShape)shape.SelectedIndex;

            foreach (var control in controls)
            {
                control.Visible = false;
            }

            controls[(int)mazeShape].Visible = true;
            controls[(int)mazeShape].Location = new Point(9, 91);
        }

        private void OnGenerationClickedHandler(object sender, EventArgs e)
        {
            switch (mazeShape)
            {
                case MazeShape.Rectangular:
                    GenerateRectangularMaze();
                    break;
                case MazeShape.Circular:
                    GenerateCircularMaze();
                    break;
                case MazeShape.Honeycomb:
                    GenerateHoneycombMaze();
                    break;
                default:
                    break;
            }
        }

        private void OnCanvasPaintHandler(object sender, PaintEventArgs e)
        {
            switch (mazeShape)
            {
                case MazeShape.Rectangular:
                    DrawRectangularMaze(e.Graphics);
                    break;
                case MazeShape.Circular:
                    DrawCircularMaze(e.Graphics);
                    break;
                case MazeShape.Honeycomb:
                    DrawHoneycombMaze(e.Graphics);
                    break;
                default: 
                    break;
            }
        }

        private void GenerateRectangularMaze()
        {
            var width = rectangularMazeControl.MazeWidth;
            var height = rectangularMazeControl.MazeHeight;
            var thickness = rectangularMazeControl.Thickness;
            var algm = (MazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (width  < 3) width  = canvas.Width  / thickness;
            if (height < 3) height = canvas.Height / thickness;

            GenerateRectangularMazeAsync(width, height, algm);
        }

        private async Task GenerateRectangularMazeAsync(int width, int height, MazeAlgorithm algorithm)
        {
            var genrator = new RectangularMazeGenerator();
            rectangularMazeField = await genrator.CreateAsync(width, height, algorithm);

            canvas.Refresh();
        }

        private void GenerateCircularMaze()
        {
            var rings = circularMazeControl.Rings;
            var sectors = circularMazeControl.Sectors;
            var algm = (MazeAlgorithm)(algorithm.SelectedIndex + 1);
            var strategy = circularMazeControl.SectorStrategy;

            GenerateCircularMazeAsync(rings, sectors, algm, strategy);
        }

        private async Task GenerateCircularMazeAsync(int rings, int sectors, MazeAlgorithm algorithm, SectorStrategy strategy)
        {
            var genrator = new CircularMazeGenerator();
            circularMazeField = await genrator.CreateAsync(rings, sectors, algorithm, strategy);

            canvas.Refresh();
        }

        private void GenerateHoneycombMaze()
        {
            var length = honeycombMazeControl.Length;
            var thickness = honeycombMazeControl.Thickness;
            var algm = (MazeAlgorithm)(algorithm.SelectedIndex + 1);

            GenerateHoneycombMazeAsync(length, algm);
        }

        private async Task GenerateHoneycombMazeAsync(int length, MazeAlgorithm algorithm)
        {
            var genrator = new HoneycombMazeGenerator();
            honeycombMazeField = await genrator.CreateAsync(length, algorithm);

            canvas.Refresh();
        }

        private void DrawRectangularMaze(Graphics grap)
        {
            if (rectangularMazeField != null)
            {
                var renderer = new RectangularMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(rectangularMazeControl.Thickness)
                        .Draw(grap, rectangularMazeField);
            }
        }

        private void DrawCircularMaze(Graphics grap)
        {
            if (circularMazeField != null)
            {
                var renderer = new CircularMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(circularMazeControl.Thickness)
                        .Draw(grap, circularMazeField);
            }
        }

        private void DrawHoneycombMaze(Graphics grap)
        {
            if (honeycombMazeField != null)
            {
                var renderer = new HoneycombMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(honeycombMazeControl.Thickness)
                        .Draw(grap, honeycombMazeField);
            }
        }

        //private bool IsDragabled()
        //{
        //    if (field.width == 0 || field.height == 0) return false;

        //    var t = (int)thickness.Value;
        //    return field.width  * t > canvas.Width || 
        //           field.height * t > canvas.Height;
        //}

        //private void OnCanvasMouseDown(object sender, MouseEventArgs e)
        //{
        //    if (IsDragabled())
        //    {
        //        dragging = true;
        //        mousepos = e.Location;
        //        canvas.Cursor = Cursors.Hand;
        //    }
        //}

        //private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (dragging)
        //    {
        //        var dx = e.X - mousepos.X;
        //        var dy = e.Y - mousepos.Y;

        //        var t = (int)thickness.Value;
        //        var width = field.width * t;
        //        var height = field.height * t;

        //        dx = offsetx + dx;
        //        dy = offsety + dy;

        //        var maxx = -(canvas.Width - width) / 2;
        //        var minx =  (canvas.Width - width) / 2;
        //        var maxy = -(canvas.Height - height) / 2;
        //        var miny =  (canvas.Height - height) / 2;

        //        offsetx = Math.Clamp(dx, minx, maxx);
        //        offsety = Math.Clamp(dy, miny, maxy);

        //        mousepos = e.Location;
        //        canvas.Refresh();
        //    }
        //}

        //private void OnCanvasMouseUp(object sender, MouseEventArgs e)
        //{
        //    dragging = false;
        //    canvas.Cursor = Cursors.Default;
        //}

        //private void OnCanvasMouseLeave(object sender, EventArgs e)
        //{
        //    dragging = false;
        //    canvas.Cursor = Cursors.Default;
        //}
    }
}
