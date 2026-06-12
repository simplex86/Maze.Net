using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Maze.Avalonia.Rendering;
using Maze.Avalonia.ViewModels;
using SimplexLab.Maze;

namespace Maze.Avalonia.Views;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _vm = new();
    private readonly List<StackPanel> _panels = new();

    private readonly MazeGenerator _mazeGenerator = new();
    private MazeField? _mazeField;
    private MazeGate _mazeGate;
    private MazeSolution _mazeSolution;

    private MazeField? _reconField;
    private MazeGate _reconGate;
    private MazeSolution _reconSolution;

    private readonly List<StackPanel> _reconPanels = new();

    public MainWindow()
    {
        InitializeComponent();

        ShapeComboBox.ItemsSource = _vm.Shapes;
        AlgorithmComboBox.ItemsSource = _vm.Algorithms;

        _panels.Add(RectangularPanel);
        _panels.Add(CircularPanel);
        _panels.Add(HoneycombPanel);
        _panels.Add(TriangularPanel);
        _panels.Add(HexagonalPanel);
        _panels.Add(CircularHexagonPanel);
        _panels.Add(StairwayPanel);
        _panels.Add(CustomizedPanel);

        _reconPanels.Add(ReconRectPanel);
        _reconPanels.Add(ReconCircPanel);
        _reconPanels.Add(ReconHoneyPanel);
        _reconPanels.Add(ReconTriPanel);
        _reconPanels.Add(ReconHexPanel);
        _reconPanels.Add(ReconCircHexPanel);
        _reconPanels.Add(ReconStairPanel);
        _reconPanels.Add(ReconCustomPanel);

        ReconShapeComboBox.ItemsSource = _vm.Shapes;

        DataContext = _vm;
    }

    private void OnShapeSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var index = ShapeComboBox.SelectedIndex;
        for (var i = 0; i < _panels.Count; i++)
        {
            _panels[i].IsVisible = (i == index);
        }
    }

    private async void OnGenerateClick(object? sender, RoutedEventArgs e)
    {
        SetControlsEnabled(false);
        GenerateButton.Content = "...";

        try
        {
            await GenerateMazeAsync();
            RedrawCanvas();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        GenerateButton.Content = "Generate";
        SetControlsEnabled(true);
    }

    private async Task GenerateMazeAsync()
    {
        var shape = (EMazeShape)_vm.SelectedShapeIndex;
        var algorithm = (EMazeAlgorithm)(_vm.SelectedAlgorithmIndex + 1);

        _mazeField = await Task.Run(() => CreateAndGenerateField(shape, algorithm));
        _mazeGate = await new MazeGateGenerator().GenerateAsync(_mazeField);
        _mazeSolution = await new MazeSolutionGenerator().GenerateAsync(_mazeField, _mazeGate);
    }

    private MazeField CreateAndGenerateField(EMazeShape shape, EMazeAlgorithm algorithm)
    {
        var canvasW = (int)MazeCanvas.Bounds.Width;
        var canvasH = (int)MazeCanvas.Bounds.Height;

        MazeField field = shape switch
        {
            EMazeShape.Rectangular => CreateRectangularField(canvasW, canvasH),
            EMazeShape.Circular => CreateCircularField(canvasW, canvasH),
            EMazeShape.Honeycomb => CreateHoneycombField(canvasW, canvasH),
            EMazeShape.Triangular => CreateTriangularField(canvasW, canvasH),
            EMazeShape.Hexagonal => CreateHexagonalField(canvasW, canvasH),
            EMazeShape.CircularHexagon => CreateCircularHexagonField(canvasW, canvasH),
            EMazeShape.Stairway => CreateStairwayField(canvasW, canvasH),
            EMazeShape.Customized => CreateCustomizedField(),
            _ => throw new ArgumentException($"Unknown shape: {shape}")
        };

        return _mazeGenerator.Generate(field, algorithm);
    }

    private const int DefaultCellSize = 30;

    private MazeField CreateRectangularField(int canvasW, int canvasH)
    {
        var width = _vm.RectWidth;
        var height = _vm.RectHeight;
        if (width < 3) width = Math.Max(3, canvasW / DefaultCellSize);
        if (height < 3) height = Math.Max(3, canvasH / DefaultCellSize);
        return new RectangularMazeField(width, height);
    }

    private MazeField CreateCircularField(int canvasW, int canvasH)
    {
        var rings = _vm.CircRings;
        var sectors = _vm.CircSectors;
        if (rings <= 0) rings = Math.Max(2, Math.Min(canvasW, canvasH) / (2 * DefaultCellSize));
        rings = Math.Max(rings, 2);
        return new CircularMazeField(rings, sectors);
    }

    private MazeField CreateHoneycombField(int canvasW, int canvasH)
    {
        var length = _vm.HoneyLength;
        if (length <= 0) length = Math.Max(2, Math.Min(canvasW, canvasH) / DefaultCellSize);
        length = Math.Max(length, 2);
        return new HoneycombMazeField(length);
    }

    private MazeField CreateTriangularField(int canvasW, int canvasH)
    {
        var length = _vm.TriLength;
        var orientation = (ETriangleOrientation)(_vm.TriOrientationIndex + 1);
        if (length <= 0) length = Math.Max(2, Math.Min(canvasW, canvasH) / DefaultCellSize);
        length = Math.Max(length, 2);
        return new TriangularMazeField(length, orientation);
    }

    private MazeField CreateHexagonalField(int canvasW, int canvasH)
    {
        var length = _vm.HexLength;
        if (length <= 0) length = Math.Max(2, Math.Min(canvasW, canvasH) / DefaultCellSize);
        length = Math.Max(length, 2);
        return new HexagonalMazeField(length);
    }

    private MazeField CreateCircularHexagonField(int canvasW, int canvasH)
    {
        var rings = _vm.CircHexRings;
        if (rings <= 0) rings = Math.Max(2, Math.Min(canvasW, canvasH) / (2 * DefaultCellSize));
        rings = Math.Max(rings, 2);
        return new CircularHexagonMazeField(rings);
    }

    private MazeField CreateStairwayField(int canvasW, int canvasH)
    {
        var length = _vm.StairLength;
        if (length <= 0) length = Math.Max(3, Math.Min(canvasW, canvasH) / DefaultCellSize);
        length = Math.Max(length, 3);
        return new StairwayMazeField(length);
    }

    private MazeField CreateCustomizedField()
    {
        if (string.IsNullOrEmpty(_vm.CustomFileName))
            throw new InvalidOperationException("The Mask Path Can Not Be Empty!");

        var mask = CustomizedMazeMaskLoader.Load(_vm.CustomFileName);
        return new CustomizedMazeField(mask);
    }

    private (float scaleX, float scaleY) CalcAutoScale(MazeField field, int canvasW, int canvasH)
    {
        var bounds = field.Bounds;
        var scaleX = (float)(canvasW / bounds.Width);
        var scaleY = (float)(canvasH / bounds.Height);

        var scale = Math.Max(3f, Math.Min(scaleX, scaleY));
        return (scale, scale);
    }

    private const int DefaultPadding = 2;

    private void RedrawCanvas()
    {
        MazeCanvas.Children.Clear();

        if (_mazeField == null || _mazeField.VertexCount == 0) return;

        var canvasW = (int)MazeCanvas.Bounds.Width;
        var canvasH = (int)MazeCanvas.Bounds.Height;
        if (canvasW <= 0 || canvasH <= 0) return;

        var paddingX = DefaultPadding;
        var paddingY = DefaultPadding;
        var (scaleX, scaleY) = CalcAutoScale(_mazeField, canvasW - 2 * paddingX, canvasH - 2 * paddingY);

        var drawingGroup = new DrawingGroup();
        using (var context = drawingGroup.Open())
        {
            var gc = new GraphicsContext(context);

            // Draw maze walls
            new MazeRenderer()
                .SetSize(canvasW, canvasH)
                .SetThickness(scaleX, scaleY)
                .SetOffset(0, 0)
                .SetPadding(paddingX, paddingY)
                .SetField(_mazeField)
                .SetGate(_vm.ShowGates ? _mazeGate : new MazeGate())
                .Draw(gc);

            // Draw gate markers
            if (_vm.ShowGates && _vm.ShowMarkers)
            {
                new MazeGateRenderer()
                    .SetSize(canvasW, canvasH)
                    .SetThickness(scaleX, scaleY)
                    .SetOffset(0, 0)
                    .SetPadding(paddingX, paddingY)
                    .SetField(_mazeField)
                    .SetGate(_mazeGate)
                    .Draw(gc);
            }

            // Draw solution
            if (_vm.ShowSolution)
            {
                new MazeSolutionRenderer()
                    .SetSize(canvasW, canvasH)
                    .SetThickness(scaleX, scaleY)
                    .SetOffset(0, 0)
                    .SetPadding(paddingX, paddingY)
                    .SetField(_mazeField)
                    .SetSolution(_mazeSolution)
                    .SetGate(_mazeGate)
                    .Draw(gc);
            }
        }

        var drawingImage = new DrawingImage(drawingGroup);
        var image = new Image { Source = drawingImage };
        Canvas.SetLeft(image, 0);
        Canvas.SetTop(image, 0);
        MazeCanvas.Children.Add(image);
    }

    private void OnShowGatesChanged(object? sender, RoutedEventArgs e)
    {
        _vm.ShowGates = ShowGatesCheckBox.IsChecked == true;
        ShowMarkersCheckBox.IsEnabled = _vm.ShowGates;
        if (!_vm.ShowGates)
        {
            ShowMarkersCheckBox.IsChecked = false;
            _vm.ShowMarkers = false;
        }
        RedrawCanvas();
    }

    private void OnShowMarkersChanged(object? sender, RoutedEventArgs e)
    {
        _vm.ShowMarkers = ShowMarkersCheckBox.IsChecked == true;
        RedrawCanvas();
    }

    private void OnShowSolutionChanged(object? sender, RoutedEventArgs e)
    {
        _vm.ShowSolution = ShowSolutionCheckBox.IsChecked == true;
        RedrawCanvas();
    }

    private async void OnBrowseClick(object? sender, RoutedEventArgs e)
    {
        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider == null) return;

        var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Mask Image",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Image Files")
                {
                    Patterns = new[] { "*.bmp", "*.png", "*.jpg" }
                }
            }
        });

        if (result.Count > 0)
        {
            _vm.CustomFileName = result[0].Path.LocalPath;
            CustomFileNameInput.Text = _vm.CustomFileName;
        }
    }

    private void SetControlsEnabled(bool enabled)
    {
        ShapeComboBox.IsEnabled = enabled;
        AlgorithmComboBox.IsEnabled = enabled;
        GenerateButton.IsEnabled = enabled;
        SaveButton.IsEnabled = enabled;
        ShowGatesCheckBox.IsEnabled = enabled;
        ShowMarkersCheckBox.IsEnabled = enabled && _vm.ShowGates;
        ShowSolutionCheckBox.IsEnabled = enabled;

        foreach (var panel in _panels)
        {
            panel.IsEnabled = enabled;
        }
    }

    private async void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        if (_mazeField == null) return;

        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider == null) return;

        var result = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Maze",
            DefaultExtension = "maze",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Maze Files")
                {
                    Patterns = new[] { "*.maze" }
                }
            }
        });

        if (result == null) return;

        try
        {
            using var ms = new MemoryStream();
            bool ok = _vm.ShowGates
                ? MazeWriter.Write(_mazeField, _mazeGate, ms)
                : MazeWriter.Write(_mazeField, ms);
            if (!ok) return;
            var data = ms.ToArray();
            await File.WriteAllBytesAsync(result.Path.LocalPath, data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Save error: {ex.Message}");
        }
    }

    private async void OnReconBrowseClick(object? sender, RoutedEventArgs e)
    {
        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider == null) return;

        var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Maze File",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Maze Files")
                {
                    Patterns = new[] { "*.maze" }
                }
            }
        });

        if (result.Count > 0)
        {
            _vm.ReconFileName = result[0].Path.LocalPath;
            ReconFileNameInput.Text = _vm.ReconFileName;

            try
            {
                var data = await File.ReadAllBytesAsync(_vm.ReconFileName);
                using var ms = new MemoryStream(data);

                var (field, gate) = await MazeReader.ReadAsync(ms);
                _reconField = field;
                _reconGate = gate;
                if (_reconField != null)
                {
                    bool hasGate = gate.Entrance != MazeGate.INVALID && gate.Exit != MazeGate.INVALID;
                    ReconShowGatesCheckBox.IsEnabled = hasGate;
                    ReconShowSolutionCheckBox.IsEnabled = hasGate;
                    if (!hasGate)
                    {
                        _vm.ReconShowGates = false;
                        _vm.ReconShowSolution = false;
                        ReconShowGatesCheckBox.IsChecked = false;
                        ReconShowSolutionCheckBox.IsChecked = false;
                    }

                    if (hasGate)
                        _reconSolution = await new MazeSolutionGenerator().GenerateAsync(_reconField, _reconGate);

                    UpdateReconParams();
                    // Defer redraw to next layout pass so canvas has valid bounds
                    global::Avalonia.Threading.Dispatcher.UIThread.Post(() => RedrawReconCanvas());
                }
                else
                {
                    Console.WriteLine("Failed to read maze file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Read error: {ex.Message}");
            }
        }
    }

    private void UpdateReconParams()
    {
        if (_reconField == null) return;

        var shape = _reconField.Shape;
        _vm.ReconShapeIndex = (int)shape;

        // Hide all param panels
        foreach (var panel in _reconPanels)
            panel.IsVisible = false;

        // Show the relevant panel
        var index = (int)shape;
        if (index < _reconPanels.Count)
            _reconPanels[index].IsVisible = true;

        switch (shape)
        {
            case EMazeShape.Rectangular:
                var rect = (RectangularMazeField)_reconField;
                _vm.ReconRectWidth = rect.Width;
                _vm.ReconRectHeight = rect.Height;
                break;
            case EMazeShape.Circular:
                var circ = (CircularMazeField)_reconField;
                _vm.ReconCircRings = circ.Rings;
                _vm.ReconCircSectors = circ.Sectors;
                break;
            case EMazeShape.Honeycomb:
                var honey = (HoneycombMazeField)_reconField;
                _vm.ReconHoneyLength = honey.Length;
                break;
            case EMazeShape.Triangular:
                var tri = (TriangularMazeField)_reconField;
                _vm.ReconTriOrder = tri.Order;
                _vm.ReconTriOrientationIndex = (int)tri.Orientation - 1;
                break;
            case EMazeShape.Hexagonal:
                var hex = (HexagonalMazeField)_reconField;
                _vm.ReconHexSize = hex.Size;
                break;
            case EMazeShape.CircularHexagon:
                var circHex = (CircularHexagonMazeField)_reconField;
                _vm.ReconCircHexSize = circHex.Size;
                break;
            case EMazeShape.Stairway:
                var stair = (StairwayMazeField)_reconField;
                _vm.ReconStairSteps = stair.Steps;
                break;
            case EMazeShape.Customized:
                var custom = (CustomizedMazeField)_reconField;
                _vm.ReconCustomWidth = custom.Width;
                _vm.ReconCustomHeight = custom.Height;
                break;
        }

        _vm.ReconVertexCount = _reconField.VertexCount;
    }

    private void RedrawReconCanvas()
    {
        ReconCanvas.Children.Clear();

        if (_reconField == null || _reconField.VertexCount == 0) return;

        var canvasW = (int)ReconCanvas.Bounds.Width;
        var canvasH = (int)ReconCanvas.Bounds.Height;
        if (canvasW <= 0 || canvasH <= 0) return;

        var paddingX = DefaultPadding;
        var paddingY = DefaultPadding;
        var (scaleX, scaleY) = CalcAutoScale(_reconField, canvasW, canvasH);

        var drawingGroup = new DrawingGroup();
        using (var context = drawingGroup.Open())
        {
            var gc = new GraphicsContext(context);

            new MazeRenderer()
                .SetSize(canvasW, canvasH)
                .SetThickness(scaleX, scaleY)
                .SetPadding(paddingX, paddingY)
                .SetOffset(0, 0)
                .SetField(_reconField)
                .SetGate(_vm.ReconShowGates ? _reconGate : new MazeGate())
                .Draw(gc);

            if (_vm.ReconShowSolution)
            {
                new MazeSolutionRenderer()
                    .SetSize(canvasW, canvasH)
                    .SetThickness(scaleX, scaleY)
                    .SetPadding(paddingX, paddingY)
                    .SetOffset(0, 0)
                    .SetField(_reconField)
                    .SetSolution(_reconSolution)
                    .SetGate(_reconGate)
                    .Draw(gc);
            }
        }

        var drawingImage = new DrawingImage(drawingGroup);
        var image = new Image { Source = drawingImage };
        Canvas.SetLeft(image, 0);
        Canvas.SetTop(image, 0);
        ReconCanvas.Children.Add(image);
    }

    private void OnReconShowGatesChanged(object? sender, RoutedEventArgs e)
    {
        _vm.ReconShowGates = ReconShowGatesCheckBox.IsChecked == true;
        RedrawReconCanvas();
    }

    private void OnReconShowSolutionChanged(object? sender, RoutedEventArgs e)
    {
        _vm.ReconShowSolution = ReconShowSolutionCheckBox.IsChecked == true;
        RedrawReconCanvas();
    }

    private void OnReconCanvasSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (_reconField != null)
            RedrawReconCanvas();
    }
}
