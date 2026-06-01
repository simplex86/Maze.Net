// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Maze.TBenchmark;

BenchmarkRunner.Run<RectangularMazeBenchmarker>();
BenchmarkRunner.Run<CircularMazeBenchmarker>();
BenchmarkRunner.Run<HoneycombMazeBenchmarker>();
BenchmarkRunner.Run<TriangularMazeBenchmarker>();
BenchmarkRunner.Run<CircularHexagonMazeBenchmarker>();
BenchmarkRunner.Run<HexagonalMazeBenchmarker>();