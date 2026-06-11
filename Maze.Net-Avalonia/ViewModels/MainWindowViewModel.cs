using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimplexLab.Maze;

namespace Maze.Avalonia.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public List<string> Shapes { get; } = new()
    {
        "Rectangular", "Circular", "Honeycomb", "Triangular",
        "Hexagonal", "CircularHexagon", "Stairway", "Customized"
    };

    public List<string> Algorithms { get; } = new()
    {
        "DFS", "BFS", "Prim", "Kruskal", "Wilson", "Eller", "AldousBroder", "HuntAndKill"
    };

    private int _selectedShapeIndex = 0;
    public int SelectedShapeIndex { get => _selectedShapeIndex; set => SetProperty(ref _selectedShapeIndex, value); }

    private int _selectedAlgorithmIndex = 3;
    public int SelectedAlgorithmIndex { get => _selectedAlgorithmIndex; set => SetProperty(ref _selectedAlgorithmIndex, value); }

    private bool _showGates = true;
    public bool ShowGates { get => _showGates; set => SetProperty(ref _showGates, value); }

    private bool _showMarkers = false;
    public bool ShowMarkers { get => _showMarkers; set => SetProperty(ref _showMarkers, value); }

    private bool _showSolution = false;
    public bool ShowSolution { get => _showSolution; set => SetProperty(ref _showSolution, value); }

    private bool _isGenerating = false;
    public bool IsGenerating { get => _isGenerating; set => SetProperty(ref _isGenerating, value); }

    // Rectangular
    private int _rectWidth = 0;
    public int RectWidth { get => _rectWidth; set => SetProperty(ref _rectWidth, value); }

    private int _rectHeight = 0;
    public int RectHeight { get => _rectHeight; set => SetProperty(ref _rectHeight, value); }

    private int _rectThickness = 30;
    public int RectThickness { get => _rectThickness; set => SetProperty(ref _rectThickness, value); }

    // Circular
    private int _circRings = 0;
    public int CircRings { get => _circRings; set => SetProperty(ref _circRings, value); }

    private int _circSectors = 100;
    public int CircSectors { get => _circSectors; set => SetProperty(ref _circSectors, value); }

    private int _circThickness = 30;
    public int CircThickness { get => _circThickness; set => SetProperty(ref _circThickness, value); }

    // Honeycomb
    private int _honeyLength = 0;
    public int HoneyLength { get => _honeyLength; set => SetProperty(ref _honeyLength, value); }

    private int _honeyThickness = 30;
    public int HoneyThickness { get => _honeyThickness; set => SetProperty(ref _honeyThickness, value); }

    // Triangular
    private int _triLength = 0;
    public int TriLength { get => _triLength; set => SetProperty(ref _triLength, value); }

    private int _triOrientationIndex = 0;
    public int TriOrientationIndex { get => _triOrientationIndex; set => SetProperty(ref _triOrientationIndex, value); }

    private int _triThickness = 20;
    public int TriThickness { get => _triThickness; set => SetProperty(ref _triThickness, value); }

    // Hexagonal
    private int _hexLength = 0;
    public int HexLength { get => _hexLength; set => SetProperty(ref _hexLength, value); }

    private int _hexThickness = 30;
    public int HexThickness { get => _hexThickness; set => SetProperty(ref _hexThickness, value); }

    // CircularHexagon
    private int _circHexRings = 0;
    public int CircHexRings { get => _circHexRings; set => SetProperty(ref _circHexRings, value); }

    private int _circHexThickness = 30;
    public int CircHexThickness { get => _circHexThickness; set => SetProperty(ref _circHexThickness, value); }

    // Stairway
    private int _stairLength = 0;
    public int StairLength { get => _stairLength; set => SetProperty(ref _stairLength, value); }

    private int _stairThickness = 30;
    public int StairThickness { get => _stairThickness; set => SetProperty(ref _stairThickness, value); }

    // Customized
    private string _customFileName = "";
    public string CustomFileName { get => _customFileName; set => SetProperty(ref _customFileName, value); }

    private int _customThickness = 9;
    public int CustomThickness { get => _customThickness; set => SetProperty(ref _customThickness, value); }

    // Reconstruction
    private string _reconFileName = "";
    public string ReconFileName { get => _reconFileName; set => SetProperty(ref _reconFileName, value); }

    private bool _reconShowGates = true;
    public bool ReconShowGates { get => _reconShowGates; set => SetProperty(ref _reconShowGates, value); }

    private bool _reconShowSolution = false;
    public bool ReconShowSolution { get => _reconShowSolution; set => SetProperty(ref _reconShowSolution, value); }

    private int _reconShapeIndex = 0;
    public int ReconShapeIndex { get => _reconShapeIndex; set => SetProperty(ref _reconShapeIndex, value); }

    // Reconstruction - Rectangular
    private int _reconRectWidth = 0;
    public int ReconRectWidth { get => _reconRectWidth; set => SetProperty(ref _reconRectWidth, value); }

    private int _reconRectHeight = 0;
    public int ReconRectHeight { get => _reconRectHeight; set => SetProperty(ref _reconRectHeight, value); }

    // Reconstruction - Circular
    private int _reconCircRings = 0;
    public int ReconCircRings { get => _reconCircRings; set => SetProperty(ref _reconCircRings, value); }

    private int _reconCircSectors = 0;
    public int ReconCircSectors { get => _reconCircSectors; set => SetProperty(ref _reconCircSectors, value); }

    // Reconstruction - Honeycomb
    private int _reconHoneyLength = 0;
    public int ReconHoneyLength { get => _reconHoneyLength; set => SetProperty(ref _reconHoneyLength, value); }

    // Reconstruction - Triangular
    private int _reconTriOrder = 0;
    public int ReconTriOrder { get => _reconTriOrder; set => SetProperty(ref _reconTriOrder, value); }

    private int _reconTriOrientationIndex = 0;
    public int ReconTriOrientationIndex { get => _reconTriOrientationIndex; set => SetProperty(ref _reconTriOrientationIndex, value); }

    // Reconstruction - Hexagonal
    private int _reconHexSize = 0;
    public int ReconHexSize { get => _reconHexSize; set => SetProperty(ref _reconHexSize, value); }

    // Reconstruction - CircularHexagon
    private int _reconCircHexSize = 0;
    public int ReconCircHexSize { get => _reconCircHexSize; set => SetProperty(ref _reconCircHexSize, value); }

    // Reconstruction - Stairway
    private int _reconStairSteps = 0;
    public int ReconStairSteps { get => _reconStairSteps; set => SetProperty(ref _reconStairSteps, value); }

    // Reconstruction - Customized
    private int _reconCustomWidth = 0;
    public int ReconCustomWidth { get => _reconCustomWidth; set => SetProperty(ref _reconCustomWidth, value); }

    private int _reconCustomHeight = 0;
    public int ReconCustomHeight { get => _reconCustomHeight; set => SetProperty(ref _reconCustomHeight, value); }

    // Reconstruction - Common
    private int _reconVertexCount = 0;
    public int ReconVertexCount { get => _reconVertexCount; set => SetProperty(ref _reconVertexCount, value); }

    public void LogGenerate()
    {
        var shape = (EMazeShape)SelectedShapeIndex;
        var algorithm = (EMazeAlgorithm)(SelectedAlgorithmIndex + 1);
        var paramInfo = GetParameterInfo(shape);

        Console.WriteLine($"=== Generate ===");
        Console.WriteLine($"Shape: {shape}");
        Console.WriteLine($"Algorithm: {algorithm}");
        Console.WriteLine($"Parameters: {paramInfo}");
        Console.WriteLine($"ShowGates: {ShowGates}, ShowMarkers: {ShowMarkers}, ShowSolution: {ShowSolution}");
    }

    private string GetParameterInfo(EMazeShape shape) => shape switch
    {
        EMazeShape.Rectangular => $"Width={RectWidth}, Height={RectHeight}, Thickness={RectThickness}",
        EMazeShape.Circular => $"Rings={CircRings}, Sectors={CircSectors}, Thickness={CircThickness}",
        EMazeShape.Honeycomb => $"Length={HoneyLength}, Thickness={HoneyThickness}",
        EMazeShape.Triangular => $"Length={TriLength}, Orientation={((ETriangleOrientation)(TriOrientationIndex + 1))}, Thickness={TriThickness}",
        EMazeShape.Hexagonal => $"Length={HexLength}, Thickness={HexThickness}",
        EMazeShape.CircularHexagon => $"Rings={CircHexRings}, Thickness={CircHexThickness}",
        EMazeShape.Stairway => $"Length={StairLength}, Thickness={StairThickness}",
        EMazeShape.Customized => $"FileName={CustomFileName}, Thickness={CustomThickness}",
        _ => ""
    };
}
