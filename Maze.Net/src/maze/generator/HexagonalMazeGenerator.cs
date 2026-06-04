using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class HexagonalMazeGenerator : MazeGenerator<HexagonalMazeField>
    {
        public HexagonalMazeGenerator() 
        {
        
        }

        public HexagonalMazeGenerator(Random random) 
            : base(random) 
        { 
        
        }

        public HexagonalMazeField Generate(int size, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            var field = new HexagonalMazeField(size);
            return Generate(field, algorithm);
        }

        public async Task<HexagonalMazeField> GenerateAsync(int size, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            return await Task.Run(() => Generate(size, algorithm));
        }

        /// <summary>
        /// 六边形迷宫不支持 Eller 算法（同 updown 组内顶点互不邻接，行检测完全失效）
        /// </summary>
        protected override bool IsAlgorithmSupported(EMazeAlgorithm algorithm) => algorithm != EMazeAlgorithm.Eller;
    }
}
