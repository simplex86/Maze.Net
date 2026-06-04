using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class CircularHexagonMazeGenerator : MazeGenerator<CircularHexagonMazeField>
    {
        public CircularHexagonMazeGenerator() 
        { 
        
        }

        public CircularHexagonMazeGenerator(Random random) 
            : base(random) 
        {
        
        }

        public CircularHexagonMazeField Generate(int size, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            var field = new CircularHexagonMazeField(size);
            return Generate(field, algorithm);
        }

        public async Task<CircularHexagonMazeField> GenerateAsync(int size, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            return await Task.Run(() => Generate(size, algorithm));
        }

        /// <summary>
        /// 圆环六边形迷宫不支持 Eller 算法（继承自六边形迷宫的三角剖分结构）
        /// </summary>
        protected override bool IsAlgorithmSupported(EMazeAlgorithm algorithm) => algorithm != EMazeAlgorithm.Eller;
    }
}
