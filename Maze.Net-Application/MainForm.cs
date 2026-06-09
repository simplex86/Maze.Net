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
        private MazeGenerator mazeGenerator = new MazeGenerator();
        private MazeField mazeField = null;
        private MazeGate mazeGate;
        private MazeSolution mazeSolution;

        private List<Control> controls = new List<Control>();

        private int dx = 0;
        private int dy = 0;

        public MainForm()
        {
            InitializeComponent();

            controls.Add(rectangularMazeControl);
            controls.Add(circularMazeControl);
            controls.Add(honeycombMazeControl);
            controls.Add(triangularMazeControl);
            controls.Add(hexagonalMazeControl);
            controls.Add(circularHexagonMazeControl);
            controls.Add(stairwayMazeControl);
            controls.Add(customizedMazeControl);

            shape.SelectedIndex = (int)mazeShape;
            algorithm.SelectedIndex = (int)EMazeAlgorithm.Kruskal - 1;
        }

        private int thickness
        {
            get
            {
                switch (mazeShape)
                {
                    case EMazeShape.Rectangular:     return rectangularMazeControl.Thickness;
                    case EMazeShape.Circular:        return circularMazeControl.Thickness;
                    case EMazeShape.Honeycomb:       return honeycombMazeControl.Thickness;
                    case EMazeShape.Triangular:      return triangularMazeControl.Thickness;
                    case EMazeShape.Hexagonal:       return hexagonalMazeControl.Thickness;
                    case EMazeShape.CircularHexagon: return circularHexagonMazeControl.Thickness;
                    case EMazeShape.Stairway:        return stairwayMazeControl.Thickness;
                    case EMazeShape.Customized:      return customizedMazeControl.Thickness;
                    default: break;
                }

                return 3;
            }
        }

        #region Handler

        private void OnShapeChangedHandler(object sender, EventArgs e)
        {
            mazeShape = (EMazeShape)shape.SelectedIndex;

            foreach (var control in controls)
            {
                control.Visible = false;
            }

            var index = (int)mazeShape;
            if (index < controls.Count) controls[index].Visible = true;
        }

        private void OnGatesChangedHandler(object sender, EventArgs e)
        {
            showMarkers.Enabled = showGates.Checked;
            canvas.Refresh();
        }

        private void OnMarkersChangedHandler(object sender, EventArgs e)
        {
            canvas.Refresh();
        }

        private void OnSolutionChangedHandler(object sender, EventArgs e)
        {
            canvas.Refresh();
        }

        private void OnGenerationClickedHandler(object sender, EventArgs e)
        {
            mazeGate.Reset();
            OnGenerationClickedHandlerAsync();
        }

        private async Task OnGenerationClickedHandlerAsync()
        {
            PrevProcess();
            {
                await Generate();
            }
            PostProcess();
        }

        private void PrevProcess()
        {
            shape.Enabled = false;
            shapeLabel.Enabled = false;
            algorithm.Enabled = false;
            algorithmLabel.Enabled = false;

            foreach (var v in controls)
            {
                v.Enabled = false;
            }

            generation.Text = "...";
            generation.Enabled = false;

            showGates.Enabled = false;
            showMarkers.Enabled = false;
            showSolution.Enabled = false;
        }

        private async Task Generate()
        {
            switch (mazeShape)
            {
                case EMazeShape.Rectangular:
                    await GenerateRectangularMazeAsync();
                    await GenerateRectangularGateAsync();
                    break;
                case EMazeShape.Circular:
                    await GenerateCircularMazeAsync();
                    await GenerateCircularGateAsync();
                    break;
                case EMazeShape.Honeycomb:
                    await GenerateHoneycombMazeAsync();
                    await GenerateHoneycombGateAsync();
                    break;
                case EMazeShape.Triangular:
                    await GenerateTriangularMazeAsync();
                    await GenerateTriangularGateAsync();
                    break;
                case EMazeShape.Hexagonal:
                    await GenerateHexagonalMazeAsync();
                    await GenerateHexagonalGateAsync();
                    break;
                case EMazeShape.CircularHexagon:
                    await GenerateCircularHexagonMazeAsync();
                    await GenerateCircularHexagonGateAsync();
                    break;
                case EMazeShape.Stairway:
                    await GenerateStairwayMazeAsync();
                    await GenerateStairwayGateAsync();
                    break;
                case EMazeShape.Customized:
                    await GenerateCustomizedMazeAsync();
                    await GenerateCustomizedGateAsync();
                    break;
                default:
                    break;
            }

            await GenerateMazeSolutionAsync();
        }

        private void PostProcess()
        {
            shape.Enabled = true;
            shapeLabel.Enabled = true;
            algorithm.Enabled = true;
            algorithmLabel.Enabled = true;

            foreach (var v in controls)
            {
                v.Enabled = true;
            }

            generation.Text = "Generate";
            generation.Enabled = true;

            showGates.Enabled = true;
            showMarkers.Enabled = showGates.Checked;
            showSolution.Enabled = true;

            canvas.Refresh();
        }

        private void OnCanvasPaintHandler(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            DrawMaze(e.Graphics);
            DrawGateMarkers(e.Graphics);
            DrawSolution(e.Graphics);
        }

        private void DrawMaze(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new MazeRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(thickness)
                        .SetOffset(dx, dy)
                        .SetField(mazeField)
                        .SetGate(showGates.Checked ? mazeGate : new MazeGate())
                        .Draw(grap);
            }
        }

        private void DrawGateMarkers(Graphics grap)
        {
            if (showGates.Checked && showMarkers.Checked)
            {
                switch (mazeShape)
                {
                    case EMazeShape.Rectangular:
                        DrawRectangularGate(grap);
                        break;
                    case EMazeShape.Circular:
                        DrawCircularGate(grap);
                        break;
                    case EMazeShape.Honeycomb:
                        DrawHoneycombGate(grap);
                        break;
                    case EMazeShape.Triangular:
                        DrawTriangularGate(grap);
                        break;
                    case EMazeShape.Hexagonal:
                        DrawHexagonalGate(grap);
                        break;
                    case EMazeShape.CircularHexagon:
                        DrawCircularHexagonGate(grap);
                        break;
                    case EMazeShape.Stairway:
                        DrawStairwayGate(grap);
                        break;
                    case EMazeShape.Customized:
                        DrawCustomizedGate(grap);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task GenerateMazeSolutionAsync()
        {
            var generator = new MazeSolutionGenerator();
            mazeSolution = await generator.GenerateAsync(mazeField, mazeGate);
        }

        private void DrawSolution(Graphics grap)
        {
            if (showSolution.Checked)
            {
                var renderer = new MazeSolutionRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(thickness)
                        .SetOffset(dx, dy)
                        .SetField(mazeField)
                        .SetSolution(mazeSolution)
                        .SetGate(mazeGate)
                        .Draw(grap);
            }
        }

        #endregion

        #region Rectangular

        private async Task GenerateRectangularMazeAsync()
        {
            var width = rectangularMazeControl.MazeWidth;
            var height = rectangularMazeControl.MazeHeight;
            var thickness = rectangularMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (width  < 3) width  = canvas.Width  / thickness;
            if (height < 3) height = canvas.Height / thickness;

            mazeField = new RectangularMazeField(width, height);
            mazeField = await mazeGenerator.GenerateAsync(mazeField, algm);
        }

        private async Task GenerateRectangularGateAsync()
        {
            if (mazeField != null)
            {
                var generator = new MazeGateGenerator();
                mazeGate = await generator.GenerateAsync(mazeField);
            }
        }

        private void DrawRectangularGate(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new MazeGateRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(rectangularMazeControl.Thickness)
                        .SetOffset(dx, dy)
                        .SetField(mazeField)
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

            if (rings <= 0) rings = Math.Min(canvas.Width, canvas.Height) / (2 * thickness);
            rings = Math.Max(rings, 2);

            mazeField = new CircularMazeField(rings, sectors);
            mazeField = await mazeGenerator.GenerateAsync(mazeField, algm);
        }

        private async Task GenerateCircularGateAsync()
        {
            if (mazeField != null)
            {
                var generator = new MazeGateGenerator();
                mazeGate = await generator.GenerateAsync(mazeField);
            }
        }

        private void DrawCircularGate(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new MazeGateRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(circularMazeControl.Thickness)
                        .SetField(mazeField)
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

            mazeField = new HoneycombMazeField(length);
            mazeField = await mazeGenerator.GenerateAsync(mazeField, algm);
        }

        private async Task GenerateHoneycombGateAsync()
        {
            if (mazeField != null)
            {
                var generator = new MazeGateGenerator();
                mazeGate = await generator.GenerateAsync(mazeField);
            }
        }

        private void DrawHoneycombGate(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new MazeGateRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(honeycombMazeControl.Thickness)
                        .SetField(mazeField)
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

            mazeField = new TriangularMazeField(length, orientation);
            mazeField = await mazeGenerator.GenerateAsync(mazeField, algm);
        }

        private async Task GenerateTriangularGateAsync()
        {
            if (mazeField != null)
            {
                var generator = new MazeGateGenerator();
                mazeGate = await generator.GenerateAsync(mazeField);
            }
        }

        private void DrawTriangularGate(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new MazeGateRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(triangularMazeControl.Thickness)
                        .SetField(mazeField)
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

            mazeField = new HexagonalMazeField(length);
            mazeField = await mazeGenerator.GenerateAsync(mazeField, algm);
        }

        private async Task GenerateHexagonalGateAsync()
        {
            if (mazeField != null)
            {
                var generator = new MazeGateGenerator();
                mazeGate = await generator.GenerateAsync(mazeField);
            }
        }

        private void DrawHexagonalGate(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new MazeGateRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(hexagonalMazeControl.Thickness)
                        .SetField(mazeField)
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

            mazeField = new CircularHexagonMazeField(rings);
            mazeField = await mazeGenerator.GenerateAsync(mazeField, algm);
        }

        private async Task GenerateCircularHexagonGateAsync()
        {
            if (mazeField != null)
            {
                var generator = new MazeGateGenerator();
                mazeGate = await generator.GenerateAsync(mazeField);
            }
        }

        private void DrawCircularHexagonGate(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new MazeGateRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(circularHexagonMazeControl.Thickness)
                        .SetField(mazeField)
                        .SetGate(mazeGate)
                        .Draw(grap);
            }
        }

        #endregion

        #region Stairway

        private async Task GenerateStairwayMazeAsync()
        {
            var length = stairwayMazeControl.Length;
            var thickness = stairwayMazeControl.Thickness;
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            if (length <= 0) length = Math.Min(canvas.Width, canvas.Height) / thickness;
            length = Math.Max(length, 3);

            mazeField = new StairwayMazeField(length);
            mazeField = await mazeGenerator.GenerateAsync(mazeField, algm);
        }

        private async Task GenerateStairwayGateAsync()
        {
            if (mazeField != null)
            {
                var generator = new MazeGateGenerator();
                mazeGate = await generator.GenerateAsync(mazeField);
            }
        }

        private void DrawStairwayGate(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new MazeGateRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(stairwayMazeControl.Thickness)
                        .SetField(mazeField)
                        .SetGate(mazeGate)
                        .Draw(grap);
            }
        }

        #endregion

        #region Customized

        private async Task GenerateCustomizedMazeAsync()
        {
            if (string.IsNullOrEmpty(customizedMazeControl.FileName))
            {
                MessageBox.Show("The Mask Path Can Not Be Empty!", "Error");
                return;
            }

            var mask = CustomizedMazeMaskLoader.Load(customizedMazeControl.FileName);
            var algm = (EMazeAlgorithm)(algorithm.SelectedIndex + 1);

            mazeField = new CustomizedMazeField(mask);
            mazeField = await mazeGenerator.GenerateAsync(mazeField, algm);
        }

        private async Task GenerateCustomizedGateAsync()
        {
            if (mazeField != null)
            {
                var generator = new MazeGateGenerator();
                mazeGate = await generator.GenerateAsync(mazeField);
            }
        }

        private void DrawCustomizedGate(Graphics grap)
        {
            if (mazeField != null)
            {
                var renderer = new MazeGateRenderer();
                renderer.SetSize(canvas.Width, canvas.Height)
                        .SetThickness(customizedMazeControl.Thickness)
                        .SetField(mazeField)
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
