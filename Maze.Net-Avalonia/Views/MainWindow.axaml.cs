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

    private MazeField CreateRectangularField(int canvasW, int canvasH)
    {
        var width = _vm.RectWidth;
        var height = _vm.RectHeight;
        var thickness = _vm.RectThickness;
        if (width < 3) width = canvasW / thickness;
        if (height < 3) height = canvasH / thickness;
        return new RectangularMazeField(width, height);
    }

    private MazeField CreateCircularField(int canvasW, int canvasH)
    {
        var rings = _vm.CircRings;
        var sectors = _vm.CircSectors;
        var thickness = _vm.CircThickness;
        if (rings <= 0) rings = Math.Min(canvasW, canvasH) / (2 * thickness);
        rings = Math.Max(rings, 2);
        return new CircularMazeField(rings, sectors);
    }

    private MazeField CreateHoneycombField(int canvasW, int canvasH)
    {
        var length = _vm.HoneyLength;
        var thickness = _vm.HoneyThickness;
        if (length <= 0) length = (int)Math.Min(canvasW / (thickness * 3.464), canvasH / (thickness * 3.0));
        length = Math.Max(length, 2);
        return new HoneycombMazeField(length);
    }

    private MazeField CreateTriangularField(int canvasW, int canvasH)
    {
        var length = _vm.TriLength;
        var orientation = (ETriangleOrientation)(_vm.TriOrientationIndex + 1);
        var thickness = _vm.TriThickness;
        if (length <= 0) length = (int)Math.Min(canvasW, canvasH / 0.866) / thickness;
        length = Math.Max(length, 2);
        return new TriangularMazeField(length, orientation);
    }

    private MazeField CreateHexagonalField(int canvasW, int canvasH)
    {
        var length = _vm.HexLength;
        var thickness = _vm.HexThickness;
        if (length <= 0) length = (int)Math.Min(canvasW, canvasH / 0.866) / (2 * thickness);
        length = Math.Max(length, 2);
        return new HexagonalMazeField(length);
    }

    private MazeField CreateCircularHexagonField(int canvasW, int canvasH)
    {
        var rings = _vm.CircHexRings;
        var thickness = _vm.CircHexThickness;
        if (rings <= 0) rings = Math.Min(canvasW, canvasH) / (2 * thickness);
        rings = Math.Max(rings, 2);
        return new CircularHexagonMazeField(rings);
    }

    private MazeField CreateStairwayField(int canvasW, int canvasH)
    {
        var length = _vm.StairLength;
        var thickness = _vm.StairThickness;
        if (length <= 0) length = Math.Min(canvasW, canvasH) / thickness;
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

    private int GetThickness()
    {
        var shape = (EMazeShape)_vm.SelectedShapeIndex;
        return shape switch
        {
            EMazeShape.Rectangular => _vm.RectThickness,
            EMazeShape.Circular => _vm.CircThickness,
            EMazeShape.Honeycomb => _vm.HoneyThickness,
            EMazeShape.Triangular => _vm.TriThickness,
            EMazeShape.Hexagonal => _vm.HexThickness,
            EMazeShape.CircularHexagon => _vm.CircHexThickness,
            EMazeShape.Stairway => _vm.StairThickness,
            EMazeShape.Customized => _vm.CustomThickness,
            _ => 3
        };
    }

    private void RedrawCanvas()
    {
        MazeCanvas.Children.Clear();

        if (_mazeField == null || _mazeField.VertexCount == 0) return;

        var canvasW = (int)MazeCanvas.Bounds.Width;
        var canvasH = (int)MazeCanvas.Bounds.Height;
        if (canvasW <= 0 || canvasH <= 0) return;

        var thickness = GetThickness();

        var drawingGroup = new DrawingGroup();
        using (var context = drawingGroup.Open())
        {
            var gc = new GraphicsContext(context);

            // Draw maze walls
            new MazeRenderer()
                .SetSize(canvasW, canvasH)
                .SetThickness(thickness)
                .SetOffset(0, 0)
                .SetField(_mazeField)
                .SetGate(_vm.ShowGates ? _mazeGate : new MazeGate())
                .Draw(gc);

            // Draw gate markers
            if (_vm.ShowGates && _vm.ShowMarkers)
            {
                new MazeGateRenderer()
                    .SetSize(canvasW, canvasH)
                    .SetThickness(thickness)
                    .SetOffset(0, 0)
                    .SetField(_mazeField)
                    .SetGate(_mazeGate)
                    .Draw(gc);
            }

            // Draw solution
            if (_vm.ShowSolution)
            {
                new MazeSolutionRenderer()
                    .SetSize(canvasW, canvasH)
                    .SetThickness(thickness)
                    .SetOffset(0, 0)
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

    private int GetReconThickness()
    {
        if (_reconField == null) return 3;

        var canvasW = (int)ReconCanvas.Bounds.Width;
        var canvasH = (int)ReconCanvas.Bounds.Height;
        if (canvasW <= 0 || canvasH <= 0) return 30;

        return _reconField.Shape switch
        {
            EMazeShape.Rectangular => CalcRectThickness(canvasW, canvasH),
            EMazeShape.Circular => CalcCircularThickness(canvasW, canvasH),
            EMazeShape.Honeycomb => CalcHoneycombThickness(canvasW, canvasH),
            EMazeShape.Triangular => CalcTriangularThickness(canvasW, canvasH),
            EMazeShape.Hexagonal => CalcHexagonalThickness(canvasW, canvasH),
            EMazeShape.CircularHexagon => CalcCircularHexagonThickness(canvasW, canvasH),
            EMazeShape.Stairway => CalcStairwayThickness(canvasW, canvasH),
            EMazeShape.Customized => CalcCustomizedThickness(canvasW, canvasH),
            _ => 30
        };
    }

    private int CalcRectThickness(int canvasW, int canvasH)
    {
        var rect = (RectangularMazeField)_reconField!;
        int tw = rect.Width > 0 ? canvasW / rect.Width : 30;
        int th = rect.Height > 0 ? canvasH / rect.Height : 30;
        return Math.Max(3, Math.Min(tw, th));
    }

    private int CalcCircularThickness(int canvasW, int canvasH)
    {
        var circ = (CircularMazeField)_reconField!;
        int rings = circ.Rings > 0 ? circ.Rings : 2;
        return Math.Max(3, Math.Min(canvasW, canvasH) / (2 * rings));
    }

    private int CalcHoneycombThickness(int canvasW, int canvasH)
    {
        var honey = (HoneycombMazeField)_reconField!;
        int length = honey.Length > 0 ? honey.Length : 2;
        var tw = (int)(canvasW / (length * 3.464));
        var th = (int)(canvasH / (length * 3.0));
        return Math.Max(3, Math.Min(tw, th));
    }

    private int CalcTriangularThickness(int canvasW, int canvasH)
    {
        var tri = (TriangularMazeField)_reconField!;
        int order = tri.Order > 0 ? tri.Order : 2;
        return Math.Max(3, (int)Math.Min(canvasW, canvasH / 0.866) / order);
    }

    private int CalcHexagonalThickness(int canvasW, int canvasH)
    {
        var hex = (HexagonalMazeField)_reconField!;
        int size = hex.Size > 0 ? hex.Size : 2;
        return Math.Max(3, (int)Math.Min(canvasW, canvasH / 0.866) / (2 * size));
    }

    private int CalcCircularHexagonThickness(int canvasW, int canvasH)
    {
        var circHex = (CircularHexagonMazeField)_reconField!;
        int size = circHex.Size > 0 ? circHex.Size : 2;
        return Math.Max(3, Math.Min(canvasW, canvasH) / (2 * size));
    }

    private int CalcStairwayThickness(int canvasW, int canvasH)
    {
        var stair = (StairwayMazeField)_reconField!;
        int steps = stair.Steps > 0 ? stair.Steps : 3;
        return Math.Max(3, Math.Min(canvasW, canvasH) / steps);
    }

    private int CalcCustomizedThickness(int canvasW, int canvasH)
    {
        var custom = (CustomizedMazeField)_reconField!;
        int tw = custom.Width > 0 ? canvasW / custom.Width : 9;
        int th = custom.Height > 0 ? canvasH / custom.Height : 9;
        return Math.Max(3, Math.Min(tw, th));
    }

    private void RedrawReconCanvas()
    {
        ReconCanvas.Children.Clear();

        if (_reconField == null || _reconField.VertexCount == 0) return;

        var canvasW = (int)ReconCanvas.Bounds.Width;
        var canvasH = (int)ReconCanvas.Bounds.Height;
        if (canvasW <= 0 || canvasH <= 0) return;

        var thickness = GetReconThickness();

        var drawingGroup = new DrawingGroup();
        using (var context = drawingGroup.Open())
        {
            var gc = new GraphicsContext(context);

            new MazeRenderer()
                .SetSize(canvasW, canvasH)
                .SetThickness(thickness)
                .SetOffset(0, 0)
                .SetField(_reconField)
                .SetGate(_vm.ReconShowGates ? _reconGate : new MazeGate())
                .Draw(gc);

            if (_vm.ReconShowSolution)
            {
                new MazeSolutionRenderer()
                    .SetSize(canvasW, canvasH)
                    .SetThickness(thickness)
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
