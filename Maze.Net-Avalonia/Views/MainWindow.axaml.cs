using System;
using System.Collections.Generic;
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
        if (length <= 0) length = (int)Math.Min(canvasW / (thickness * 3.464), canvasH / (1.732 * thickness));
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
            // Draw maze walls
            new MazeRenderer()
                .SetSize(canvasW, canvasH)
                .SetThickness(thickness)
                .SetOffset(0, 0)
                .SetField(_mazeField)
                .SetGate(_vm.ShowGates ? _mazeGate : new MazeGate())
                .Draw(context);

            // Draw gate markers
            if (_vm.ShowGates && _vm.ShowMarkers)
            {
                new MazeGateRenderer()
                    .SetSize(canvasW, canvasH)
                    .SetThickness(thickness)
                    .SetOffset(0, 0)
                    .SetField(_mazeField)
                    .SetGate(_mazeGate)
                    .Draw(context);
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
                    .Draw(context);
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
        ShowGatesCheckBox.IsEnabled = enabled;
        ShowMarkersCheckBox.IsEnabled = enabled && _vm.ShowGates;
        ShowSolutionCheckBox.IsEnabled = enabled;

        foreach (var panel in _panels)
        {
            panel.IsEnabled = enabled;
        }
    }
}
