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
            generator.Generate(20, ETriangleOrientation.Upward, EMazeAlgorithm.DFS);
        }

        [Benchmark]
        public void BFS()
        {
            generator.Generate(20, ETriangleOrientation.Upward, EMazeAlgorithm.BFS);
        }

        [Benchmark]
        public void Prim()
        {
            generator.Generate(20, ETriangleOrientation.Upward, EMazeAlgorithm.Prim);
        }

        [Benchmark]
        public void Kruskal()
        {
            generator.Generate(20, ETriangleOrientation.Upward, EMazeAlgorithm.Kruskal);
        }

        [Benchmark]
        public void Wilson()
        {
            generator.Generate(20, ETriangleOrientation.Upward, EMazeAlgorithm.Wilson);
        }

        [Benchmark]
        public void Eller()
        {
            generator.Generate(20, ETriangleOrientation.Upward, EMazeAlgorithm.Eller);
        }

        [Benchmark]
        public void AldousBroder()
        {
            generator.Generate(20, ETriangleOrientation.Upward, EMazeAlgorithm.AldousBroder);
        }

        [Benchmark]
        public void HuntAndKill()
        {
            generator.Generate(20, ETriangleOrientation.Upward, EMazeAlgorithm.HuntAndKill);
        }
    }
}
