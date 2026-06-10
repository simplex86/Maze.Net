using System;
using BenchmarkDotNet.Attributes;
using SimplexLab.Maze;

namespace Maze.TBenchmark
{
    [MemoryDiagnoser]
    public class StairwayMazeBenchmarker
    {
        private MazeGenerator generator = null;

        public StairwayMazeBenchmarker()
        {
            var random = Common.SharedRandom;
            generator = new MazeGenerator(random);
        }

        [Benchmark]
        public void DFS()
        {
            var field = new StairwayMazeField(20);
            generator.Generate(field, EMazeAlgorithm.DFS);
        }

        [Benchmark]
        public void BFS()
        {
            var field = new StairwayMazeField(20);
            generator.Generate(field, EMazeAlgorithm.BFS);
        }

        [Benchmark]
        public void Prim()
        {
            var field = new StairwayMazeField(20);
            generator.Generate(field, EMazeAlgorithm.Prim);
        }

        [Benchmark]
        public void Kruskal()
        {
            var field = new StairwayMazeField(20);
            generator.Generate(field, EMazeAlgorithm.Kruskal);
        }

        [Benchmark]
        public void Wilson()
        {
            var field = new StairwayMazeField(20);
            generator.Generate(field, EMazeAlgorithm.Wilson);
        }

        [Benchmark]
        public void AldousBroder()
        {
            var field = new StairwayMazeField(20);
            generator.Generate(field, EMazeAlgorithm.AldousBroder);
        }

        [Benchmark]
        public void HuntAndKill()
        {
            var field = new StairwayMazeField(20);
            generator.Generate(field, EMazeAlgorithm.HuntAndKill);
        }
    }
}
