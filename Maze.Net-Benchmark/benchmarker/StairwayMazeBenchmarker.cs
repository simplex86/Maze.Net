using System;
using BenchmarkDotNet.Attributes;
using SimplexLab.Maze;

namespace Maze.TBenchmark
{
    [MemoryDiagnoser]
    public class StairwayMazeBenchmarker
    {
        private StairwayMazeGenerator generator = null;

        public StairwayMazeBenchmarker()
        {
            var random = Common.SharedRandom;
            generator = new StairwayMazeGenerator(random);
        }

        [Benchmark]
        public void DFS()
        {
            generator.Generate(20, EMazeAlgorithm.DFS);
        }

        [Benchmark]
        public void BFS()
        {
            generator.Generate(20, EMazeAlgorithm.BFS);
        }

        [Benchmark]
        public void Prim()
        {
            generator.Generate(20, EMazeAlgorithm.Prim);
        }

        [Benchmark]
        public void Kruskal()
        {
            generator.Generate(20, EMazeAlgorithm.Kruskal);
        }

        [Benchmark]
        public void Wilson()
        {
            generator.Generate(20, EMazeAlgorithm.Wilson);
        }

        [Benchmark]
        public void AldousBroder()
        {
            generator.Generate(20, EMazeAlgorithm.AldousBroder);
        }

        [Benchmark]
        public void HuntAndKill()
        {
            generator.Generate(20, EMazeAlgorithm.HuntAndKill);
        }
    }
}
