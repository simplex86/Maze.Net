using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    public class StairwayMazeGenerator : MazeGenerator<StairwayMazeField>
    {
        public StairwayMazeGenerator()
        {

        }

        public StairwayMazeGenerator(Random random)
            : base(random)
        {

        }

        public StairwayMazeField Generate(int steps, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            var field = new StairwayMazeField(steps);
            return Generate(field, algorithm);
        }

        public async Task<StairwayMazeField> GenerateAsync(int steps, EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            return await Task.Run(() => Generate(steps, algorithm));
        }

        /// <summary>
        /// 阶梯形迷宫不支持 Eller 算法：每行最右侧格子没有上方邻居，导致 Eller 算法无法保证每个连通集都有垂直连接
        /// </summary>
        protected override bool IsAlgorithmSupported(EMazeAlgorithm algorithm) => algorithm != EMazeAlgorithm.Eller;
    }
}
