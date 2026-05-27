using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    public partial class MainForm : Form
    {
        private EMazeShape mazeShape = EMazeShape.Rectangular;
        private MazeField mazeField = null;
        private MazeGate mazeGate;
        
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
            mazeGate.Reset();
            OnGenerationClickedHandlerAsync();
        }

        private async Task OnGenerationClickedHandlerAsync()
        {
            switch (mazeShape)
            {
                case EMazeShape.Rectangular:
                    await GenerateRectangularMazeAsync();
                    break;
                case EMazeShape.Circular:
                    await GenerateCircularMazeAsync();
                    break;
                case EMazeShape.Honeycomb:
                    await GenerateHoneycombMazeAsync();
                    break;
                case EMazeShape.Triangular:
                    await GenerateTriangularMazeAsync();
                    break;
                case EMazeShape.Hexagonal:
                    await GenerateHexagonalMazeAsync();
                    break;
                case EMazeShape.CircularHexagon:
                    await GenerateCircularHexagonMazeAsync();
                    break;
                default:
                    break;
            }

            if (true)
            {
                var generator = new MazeGateGenerator();
                mazeGate = generator.Generate(mazeField);
            }

            canvas.Refresh();
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

        private async Task GenerateRectangularMazeAsync()
        {
            var width = rectangularMazeControl.MazeWidth;
            var height = rectangularMazeControl.MazeHeight;
            var thickness = rectangularMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (width < 3) width = canvas.Width / thickness;
            if (height < 3) height = canvas.Height / thickness;

            var genrator = new RectangularMazeGenerator();
            mazeField = await genrator.GenerateAsync(width, height, algm);
        }

        private void DrawRectangularMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new RectangularMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(rectangularMazeControl.Thickness)
                        .SetField(mazeField as RectangularMazeField)
                        .Draw(grap);

                var gateRenderer = new RectangularGateRenderer();
                gateRenderer.SetSize(canvas.Width, canvas.Height)
                            .SetThickness(rectangularMazeControl.Thickness)
                            .SetField(mazeField as RectangularMazeField)
                            .SetGate(mazeGate)
                            .Draw(grap);
            }
        }

        #endregion

        #region Circular

        private async Task GenerateCircularMazeAsync()
        {
            var rings = circularMazeControl.Rings;
            var sectors = circularMazeControl.Sectors;
            var thickness = circularMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);
            var strategy = circularMazeControl.SectorStrategy;

            if (rings <= 0) rings = Math.Min(canvas.Width, canvas.Height) / (2 * thickness);
            rings = Math.Max(rings, 2);

            var genrator = new CircularMazeGenerator();
            mazeField = await genrator.GenerateAsync(rings, sectors, algm, strategy);
        }

        private void DrawCircularMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new CircularMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(circularMazeControl.Thickness)
                        .SetField(mazeField as CircularMazeField)
                        .Draw(grap);

                var gateRenderer = new CircularGateRenderer();
                gateRenderer.SetSize(canvas.Width, canvas.Height)
                            .SetThickness(circularMazeControl.Thickness)
                            .SetField(mazeField as CircularMazeField)
                            .SetGate(mazeGate)
                            .Draw(grap);
            }
        }

        #endregion

        #region Honeycomb

        private async Task GenerateHoneycombMazeAsync()
        {
            var length = honeycombMazeControl.Length;
            var thickness = honeycombMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (length <= 0) length = (int)Math.Min(canvas.Width / (thickness * 3.464), canvas.Height / (1.732 * thickness));
            length = Math.Max(length, 2);

            var genrator = new HoneycombMazeGenerator();
            mazeField = await genrator.GenerateAsync(length, algm);
        }

        private void DrawHoneycombMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new HoneycombMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(honeycombMazeControl.Thickness)
                        .SetField(mazeField as HoneycombMazeField)
                        .Draw(grap);

                var gateRenderer = new HoneycombGateRenderer();
                gateRenderer.SetSize(canvas.Width, canvas.Height)
                            .SetThickness(honeycombMazeControl.Thickness)
                            .SetField(mazeField as HoneycombMazeField)
                            .SetGate(mazeGate)
                            .Draw(grap);
            }
        }

        #endregion

        #region Triangular

        private async Task GenerateTriangularMazeAsync()
        {
            var length = triangularMazeControl.Length;
            var orientation = triangularMazeControl.Orientation;
            var thickness = triangularMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (length <= 0) length = (int)Math.Min(canvas.Width, canvas.Height / 0.866) / thickness;
            length = Math.Max(length, 2);

            var genrator = new TriangularMazeGenerator();
            mazeField = await genrator.GenerateAsync(length, orientation, algm);
        }

        private void DrawTriangularMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new TriangularMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(triangularMazeControl.Thickness)
                        .SetField(mazeField as TriangularMazeField)
                        .Draw(grap);

                var gateRenderer = new TriangularGateRenderer();
                gateRenderer.SetSize(canvas.Width, canvas.Height)
                            .SetThickness(triangularMazeControl.Thickness)
                            .SetField(mazeField as TriangularMazeField)
                            .SetGate(mazeGate)
                            .Draw(grap);
            }
        }

        #endregion

        #region Hexagonal

        private async Task GenerateHexagonalMazeAsync()
        {
            var length = hexagonalMazeControl.Length;
            var thickness = hexagonalMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (length <= 0) length = (int)Math.Min(canvas.Width, canvas.Height / 0.866) / (2 * thickness);
            length = Math.Max(length, 2);

            var genrator = new HexagonalMazeGenerator();
            mazeField = await genrator.GenerateAsync(length, algm);
        }

        private void DrawHexagonalMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new HexagonalMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(hexagonalMazeControl.Thickness)
                        .SetField(mazeField as HexagonalMazeField)
                        .Draw(grap);

                var gateRenderer = new HexagonalGateRenderer();
                gateRenderer.SetSize(canvas.Width, canvas.Height)
                            .SetThickness(hexagonalMazeControl.Thickness)
                            .SetField(mazeField as HexagonalMazeField)
                            .SetGate(mazeGate)
                            .Draw(grap);
            }
        }

        #endregion

        #region CircularHexagon

        private async Task GenerateCircularHexagonMazeAsync()
        {
            var rings = circularHexagonMazeControl.Rings;
            var thickness = circularHexagonMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (rings <= 0) rings = Math.Min(canvas.Width, canvas.Height) / (2 * thickness);
            rings = Math.Max(rings, 2);

            var genrator = new CircularHexagonMazeGenerator();
            mazeField = await genrator.GenerateAsync(rings, algm);
        }

        private void DrawCircularHexagonMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new CircularHexagonMazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(circularHexagonMazeControl.Thickness)
                        .SetField(mazeField as CircularHexagonMazeField)
                        .Draw(grap);

                var gateRenderer = new CircularHexagonGateRenderer();
                gateRenderer.SetSize(canvas.Width, canvas.Height)
                            .SetThickness(circularHexagonMazeControl.Thickness)
                            .SetField(mazeField as CircularHexagonMazeField)
                            .SetGate(mazeGate)
                            .Draw(grap);
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
