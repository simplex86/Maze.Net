using System;
using BenchmarkDotNet.Attributes;
using SimplexLab.Maze;

namespace Maze.TBenchmark
{
    [MemoryDiagnoser]
    public class HoneycombMazeBenchmarker
    {
        private HoneycombMazeGenerator generator = null;

        public HoneycombMazeBenchmarker()
        {
            var random = Common.SharedRandom;
            generator = new HoneycombMazeGenerator(random);
        }

        [Benchmark]
        public void DFS()
        {
            generator.Generate(15, EMazeAlgorithm.DFS);
        }

        [Benchmark]
        public void BFS()
        {
            generator.Generate(15, EMazeAlgorithm.BFS);
        }

        [Benchmark]
        public void Prim()
        {
            generator.Generate(15, EMazeAlgorithm.Prim);
        }

        [Benchmark]
        public void Kruskal()
        {
            generator.Generate(15, EMazeAlgorithm.Kruskal);
        }

        [Benchmark]
        public void Wilson()
        {
            generator.Generate(15, EMazeAlgorithm.Wilson);
        }

        [Benchmark]
        public void Eller()
        {
            generator.Generate(15, EMazeAlgorithm.Eller);
        }

        [Benchmark]
        public void AldousBroder()
        {
            generator.Generate(15, EMazeAlgorithm.AldousBroder);
        }

        [Benchmark]
        public void HuntAndKill()
        {
            generator.Generate(15, EMazeAlgorithm.HuntAndKill);
        }
    }
}
