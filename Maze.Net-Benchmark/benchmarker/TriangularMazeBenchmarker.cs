using System;
using BenchmarkDotNet.Attributes;
using SimplexLab.Maze;

namespace Maze.TBenchmark
{
    [MemoryDiagnoser]
    public class TriangularMazeBenchmarker
    {
        private TriangularMazeGenerator generator = null;

        public TriangularMazeBenchmarker()
        {
            var random = Common.SharedRandom;
            generator = new TriangularMazeGenerator(random);
        }

        [Benchmark]
        public void DFS()
        {
            generator.Generate(20, TriangleOrientation.Upward, EMazeAlgorithm.DFS);
        }

        [Benchmark]
        public void BFS()
        {
            generator.Generate(20, TriangleOrientation.Upward, EMazeAlgorithm.BFS);
        }

        [Benchmark]
        public void Prim()
        {
            generator.Generate(20, TriangleOrientation.Upward, EMazeAlgorithm.Prim);
        }

        [Benchmark]
        public void Kruskal()
        {
            generator.Generate(20, TriangleOrientation.Upward, EMazeAlgorithm.Kruskal);
        }

        [Benchmark]
        public void Wilson()
        {
            generator.Generate(20, TriangleOrientation.Upward, EMazeAlgorithm.Wilson);
        }

        [Benchmark]
        public void Eller()
        {
            generator.Generate(20, TriangleOrientation.Upward, EMazeAlgorithm.Eller);
        }

        [Benchmark]
        public void AldousBroder()
        {
            generator.Generate(20, TriangleOrientation.Upward, EMazeAlgorithm.AldousBroder);
        }

        [Benchmark]
        public void HuntAndKill()
        {
            generator.Generate(20, TriangleOrientation.Upward, EMazeAlgorithm.HuntAndKill);
        }
    }
}
