using System;

namespace Maze.TBenchmark
{
    internal static class Common
    {
        public static Random SharedRandom { get; } = new Random(24555637);
    }
}
