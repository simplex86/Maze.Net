using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using SimplexLab.Maze;

namespace Maze.Avalonia.ViewModels;

public class MainWindowViewModel
{
    public List<string> Shapes { get; } = new()
    {
        "Rectangular", "Circular", "Honeycomb", "Triangular",
        "Hexagonal", "CircularHexagon", "Stairway", "Customized"
    };

    public List<string> Algorithms { get; } = new()
    {
        "DFS", "BFS", "Prim", "Kruskal", "Wilson", "Eller", "AldousBroder", "HuntAndKill"
    };

    public int SelectedShapeIndex { get; set; } = 0;
    public int SelectedAlgorithmIndex { get; set; } = 3; // Kruskal

    public bool ShowGates { get; set; } = true;
    public bool ShowMarkers { get; set; } = false;
    public bool ShowSolution { get; set; } = false;
    public bool IsGenerating { get; set; } = false;

    // Rectangular
    public int RectWidth { get; set; } = 0;
    public int RectHeight { get; set; } = 0;
    public int RectThickness { get; set; } = 30;

    // Circular
    public int CircRings { get; set; } = 0;
    public int CircSectors { get; set; } = 100;
    public int CircThickness { get; set; } = 30;

    // Honeycomb
    public int HoneyLength { get; set; } = 0;
    public int HoneyThickness { get; set; } = 30;

    // Triangular
    public int TriLength { get; set; } = 0;
    public int TriOrientationIndex { get; set; } = 0; // Upward
    public int TriThickness { get; set; } = 20;

    // Hexagonal
    public int HexLength { get; set; } = 0;
    public int HexThickness { get; set; } = 30;

    // CircularHexagon
    public int CircHexRings { get; set; } = 0;
    public int CircHexThickness { get; set; } = 30;

    // Stairway
    public int StairLength { get; set; } = 0;
    public int StairThickness { get; set; } = 30;

    // Customized
    public string CustomFileName { get; set; } = "";
    public int CustomThickness { get; set; } = 9;

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
