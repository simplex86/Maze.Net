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
            generator.Generate(20, 20, EMazeAlgorithm.DFS);
        }

        [Benchmark]
        public void BFS()
        {
            generator.Generate(20, 20, EMazeAlgorithm.BFS);
        }

        [Benchmark]
        public void Prim()
        {
            generator.Generate(20, 20, EMazeAlgorithm.Prim);
        }

        [Benchmark]
        public void Kruskal()
        {
            generator.Generate(20, 20, EMazeAlgorithm.Kruskal);
        }

        [Benchmark]
        public void Wilson()
        {
            generator.Generate(20, 20, EMazeAlgorithm.Wilson);
        }

        [Benchmark]
        public void Eller()
        {
            generator.Generate(20, 20, EMazeAlgorithm.Eller);
        }

        [Benchmark]
        public void AldousBroder()
        {
            generator.Generate(20, 20, EMazeAlgorithm.AldousBroder);
        }
    }
}
