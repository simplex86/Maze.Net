using System;
using BenchmarkDotNet.Attributes;
using SimplexLab.Maze;

namespace Maze.TBenchmark
{
    [MemoryDiagnoser]
    public class TriangularMazeBenchmarker
    {
        private MazeGenerator generator = null;

        public TriangularMazeBenchmarker()
        {
            var random = Common.SharedRandom;
            generator = new MazeGenerator(random);
        }

        [Benchmark]
        public void DFS()
        {
            var field = new TriangularMazeField(20, ETriangleOrientation.Upward);
            generator.Generate(field, EMazeAlgorithm.DFS);
        }

        [Benchmark]
        public void BFS()
        {
            var field = new TriangularMazeField(20, ETriangleOrientation.Upward);
            generator.Generate(field, EMazeAlgorithm.BFS);
        }

        [Benchmark]
        public void Prim()
        {
            var field = new TriangularMazeField(20, ETriangleOrientation.Upward);
            generator.Generate(field, EMazeAlgorithm.Prim);
        }

        [Benchmark]
        public void Kruskal()
        {
            var field = new TriangularMazeField(20, ETriangleOrientation.Upward);
            generator.Generate(field, EMazeAlgorithm.Kruskal);
        }

        [Benchmark]
        public void Wilson()
        {
            var field = new TriangularMazeField(20, ETriangleOrientation.Upward);
            generator.Generate(field, EMazeAlgorithm.Wilson);
        }

        [Benchmark]
        public void Eller()
        {
            var field = new TriangularMazeField(20, ETriangleOrientation.Upward);
            generator.Generate(field, EMazeAlgorithm.Eller);
        }

        [Benchmark]
        public void AldousBroder()
        {
            var field = new TriangularMazeField(20, ETriangleOrientation.Upward);
            generator.Generate(field, EMazeAlgorithm.AldousBroder);
        }

        [Benchmark]
        public void HuntAndKill()
        {
            var field = new TriangularMazeField(20, ETriangleOrientation.Upward);
            generator.Generate(field, EMazeAlgorithm.HuntAndKill);
        }
    }
}
