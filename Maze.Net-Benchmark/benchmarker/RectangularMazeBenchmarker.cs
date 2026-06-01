using System;
using BenchmarkDotNet.Attributes;
using SimplexLab.Maze;

namespace Maze.TBenchmark
{
    [MemoryDiagnoser]
    public class RectangularMazeBenchmarker
    {
        private RectangularMazeGenerator generator = null;

        public RectangularMazeBenchmarker()
        {
            var random = Common.SharedRandom;
            generator = new RectangularMazeGenerator(random);
        }

        [Benchmark]
        public void DFS()
        {
            generator.Generate(100, 100, EMazeAlgorithm.DFS);
        }

        [Benchmark]
        public void BFS()
        {
            generator.Generate(100, 100, EMazeAlgorithm.BFS);
        }

        [Benchmark]
        public void Prim()
        {
            generator.Generate(100, 100, EMazeAlgorithm.Prim);
        }

        [Benchmark]
        public void Kruskal()
        {
            generator.Generate(100, 100, EMazeAlgorithm.Kruskal);
        }

        [Benchmark]
        public void Wilson()
        {
            generator.Generate(100, 100, EMazeAlgorithm.Wilson);
        }

        [Benchmark]
        public void Eller()
        {
            generator.Generate(100, 100, EMazeAlgorithm.Eller);
        }

        [Benchmark]
        public void AldousBroder()
        {
            generator.Generate(100, 100, EMazeAlgorithm.AldousBroder);
        }
    }
}
