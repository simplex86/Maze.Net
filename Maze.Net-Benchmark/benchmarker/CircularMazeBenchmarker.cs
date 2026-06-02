using System;
using BenchmarkDotNet.Attributes;
using SimplexLab.Maze;

namespace Maze.TBenchmark
{
    [MemoryDiagnoser]
    public class CircularMazeBenchmarker
    {
        private CircularMazeGenerator generator = null;

        public CircularMazeBenchmarker()
        {
            var random = Common.SharedRandom;
            generator = new CircularMazeGenerator(random);
        }

        [Benchmark]
        public void DFS()
        {
            generator.Generate(20, 100, EMazeAlgorithm.DFS);
        }

        [Benchmark]
        public void BFS()
        {
            generator.Generate(20, 100, EMazeAlgorithm.BFS);
        }

        [Benchmark]
        public void Prim()
        {
            generator.Generate(20, 100, EMazeAlgorithm.Prim);
        }

        [Benchmark]
        public void Kruskal()
        {
            generator.Generate(20, 100, EMazeAlgorithm.Kruskal);
        }

        [Benchmark]
        public void Wilson()
        {
            generator.Generate(20, 100, EMazeAlgorithm.Wilson);
        }

        [Benchmark]
        public void Eller()
        {
            generator.Generate(20, 100, EMazeAlgorithm.Eller);
        }

        [Benchmark]
        public void AldousBroder()
        {
            generator.Generate(20, 100, EMazeAlgorithm.AldousBroder);
        }

        [Benchmark]
        public void HuntAndKill()
        {
            generator.Generate(20, 100, EMazeAlgorithm.HuntAndKill);
        }
    }
}
