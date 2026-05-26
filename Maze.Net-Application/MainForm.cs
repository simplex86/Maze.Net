using SimplexLab.Maze;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze.TApplication
{
    public partial class MainForm : Form
    {
        private EMazeShape mazeShape = EMazeShape.Rectangular;
        private MazeField mazeField = null;

        private List<Control> controls = new List<Control>();

        public MainForm()
        {
            InitializeComponent();

            controls.Add(rectangularMazeControl);
            controls.Add(circularMazeControl);
            controls.Add(honeycombMazeControl);
            controls.Add(triangularMazeControl);
            controls.Add(hexagonalMazeControl);
            controls.Add(circularHexagonMazeControl);

            shape.SelectedIndex = (int)mazeShape;
            algorithm.SelectedIndex = (int)EMazeAlgorithm.Kruskal - 1;
        }

        private void OnShapeChangedHandler(object sender, EventArgs e)
        {
            mazeShape = (EMazeShape)shape.SelectedIndex;

            foreach (var control in controls)
            {
                control.Visible = false;
            }

            var location = algorithmLabel.Location;
            location.X -= 1;
            location.Y += 26;

            controls[(int)mazeShape].Visible = true;
            controls[(int)mazeShape].Location = location;
        }

        private void OnGenerationClickedHandler(object sender, EventArgs e)
        {
            switch (mazeShape)
            {
                case EMazeShape.Rectangular:
                    GenerateRectangularMaze();
                    break;
                case EMazeShape.Circular:
                    GenerateCircularMaze();
                    break;
                case EMazeShape.Honeycomb:
                    GenerateHoneycombMaze();
                    break;
                case EMazeShape.Triangular:
                    GenerateTriangularMaze();
                    break;
                case EMazeShape.Hexagonal:
                    GenerateHexagonalMaze();
                    break;
                case EMazeShape.CircularHexagon:
                    GenerateCircularHexagonMaze();
                    break;
                default:
                    break;
            }
        }

        private void OnCanvasPaintHandler(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            switch (mazeShape)
            {
                case EMazeShape.Rectangular:
                    DrawRectangularMaze(e.Graphics);
                    break;
                case EMazeShape.Circular:
                    DrawCircularMaze(e.Graphics);
                    break;
                case EMazeShape.Honeycomb:
                    DrawHoneycombMaze(e.Graphics);
                    break;
                case EMazeShape.Triangular:
                    DrawTriangularMaze(e.Graphics);
                    break;
                case EMazeShape.Hexagonal:
                    DrawHexagonalMaze(e.Graphics);
                    break;
                case EMazeShape.CircularHexagon:
                    DrawCircularHexagonMaze(e.Graphics);
                    break;
                default: 
                    break;
            }
        }

        #region Rectangular

        private void GenerateRectangularMaze()
        {
            var width = rectangularMazeControl.MazeWidth;
            var height = rectangularMazeControl.MazeHeight;
            var thickness = rectangularMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (width  < 3) width  = canvas.Width  / thickness;
            if (height < 3) height = canvas.Height / thickness;

            GenerateRectangularMazeAsync(width, height, algm);
        }

        private async Task GenerateRectangularMazeAsync(int width, int height, EMazeAlgorithm algorithm)
        {
            var genrator = new RectangularMazeGenerator();
            mazeField = await genrator.GenerateAsync(width, height, algorithm);

            canvas.Refresh();
        }

        private void DrawRectangularMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new RectangularMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(rectangularMazeControl.Thickness)
                        .Draw(grap, mazeField as RectangularMazeField);
            }
        }

        #endregion

        #region Circular

        private void GenerateCircularMaze()
        {
            var rings = circularMazeControl.Rings;
            var sectors = circularMazeControl.Sectors;
            var thickness = circularMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);
            var strategy = circularMazeControl.SectorStrategy;

            if (rings <= 0) rings = Math.Min(canvas.Width, canvas.Height)  / (2 * thickness);
            rings = Math.Max(rings, 2);

            GenerateCircularMazeAsync(rings, sectors, algm, strategy);
        }

        private async Task GenerateCircularMazeAsync(int rings, int sectors, EMazeAlgorithm algorithm, ESectorStrategy strategy)
        {
            var genrator = new CircularMazeGenerator();
            mazeField = await genrator.GenerateAsync(rings, sectors, algorithm, strategy);

            canvas.Refresh();
        }

        private void DrawCircularMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new CircularMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(circularMazeControl.Thickness)
                        .Draw(grap, mazeField as CircularMazeField);
            }
        }

        #endregion

        #region Honeycomb

        private void GenerateHoneycombMaze()
        {
            var length = honeycombMazeControl.Length;
            var thickness = honeycombMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (length <= 0) length = (int)Math.Min(canvas.Width / (thickness * 3.464), canvas.Height / (1.732 * thickness));
            length = Math.Max(length, 2);

            GenerateHoneycombMazeAsync(length, algm);
        }

        private async Task GenerateHoneycombMazeAsync(int length, EMazeAlgorithm algorithm)
        {
            var genrator = new HoneycombMazeGenerator();
            mazeField = await genrator.GenerateAsync(length, algorithm);

            canvas.Refresh();
        }

        private void DrawHoneycombMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new HoneycombMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(honeycombMazeControl.Thickness)
                        .Draw(grap, mazeField as HoneycombMazeField);
            }
        }

        #endregion

        #region Triangular

        private void GenerateTriangularMaze()
        {
            var length = triangularMazeControl.Length;
            var orientation = triangularMazeControl.Orientation;
            var thickness = triangularMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (length <= 0) length = (int)Math.Min(canvas.Width, canvas.Height / 0.866) / thickness;
            length = Math.Max(length, 2);

            GenerateTriangularMazeAsync(length, orientation, algm);
        }

        private async Task GenerateTriangularMazeAsync(int length, TriangleOrientation orientation, EMazeAlgorithm algorithm)
        {
            var genrator = new TriangularMazeGenerator();
            mazeField = await genrator.GenerateAsync(length, orientation, algorithm);

            canvas.Refresh();
        }

        private void DrawTriangularMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new TriangularMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(triangularMazeControl.Thickness)
                        .Draw(grap, mazeField as TriangularMazeField);
            }
        }

        #endregion

        #region Hexagonal

        private void GenerateHexagonalMaze()
        {
            var length = hexagonalMazeControl.Length;
            var thickness = hexagonalMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (length <= 0) length = (int)Math.Min(canvas.Width, canvas.Height / 0.866) / (2 * thickness);
            length = Math.Max(length, 2);

            GenerateHexagonalMazeAsync(length, algm);
        }

        private async Task GenerateHexagonalMazeAsync(int length, EMazeAlgorithm algorithm)
        {
            var genrator = new HexagonalMazeGenerator();
            mazeField = await genrator.GenerateAsync(length, algorithm);

            canvas.Refresh();
        }

        private void DrawHexagonalMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new HexagonalMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(hexagonalMazeControl.Thickness)
                        .Draw(grap, mazeField as HexagonalMazeField);
            }
        }

        #endregion

        #region CircularHexagon

        private void GenerateCircularHexagonMaze()
        {
            var rings = circularHexagonMazeControl.Rings;
            var thickness = circularHexagonMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (rings <= 0) rings = Math.Min(canvas.Width, canvas.Height) / (2 * thickness);
            rings = Math.Max(rings, 2);

            GenerateCircularHexagonMazeAsync(rings, algm);
        }

        private async Task GenerateCircularHexagonMazeAsync(int rings, EMazeAlgorithm algorithm)
        {
            var genrator = new CircularHexagonMazeGenerator();
            mazeField = await genrator.GenerateAsync(rings, algorithm);

            canvas.Refresh();
        }

        private void DrawCircularHexagonMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new CircularHexagonMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(circularHexagonMazeControl.Thickness)
                        .Draw(grap, mazeField as CircularHexagonMazeField);
            }
        }

        #endregion

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
