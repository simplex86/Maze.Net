using System;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 三角形朝向
    /// </summary>
    public enum ETriangleOrientation
    {
        /// <summary>
        /// 朝上
        /// </summary>
        Upward = 1,
        /// <summary>
        /// 朝下
        /// </summary>
        Downward = 2,
    }

    public class TriangularMazeGenerator : MazeGenerator<TriangularMazeField>
    {
        public TriangularMazeGenerator() 
        {
        
        }

        public TriangularMazeGenerator(Random random) 
            : base(random) 
        { 
        
        }

        public TriangularMazeField Generate(int order,
                                            ETriangleOrientation orientation = ETriangleOrientation.Upward,
                                            EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            var field = new TriangularMazeField(order, orientation);
            return Generate(field, algorithm);
        }

        public async Task<TriangularMazeField> GenerateAsync(int order,
                                                             ETriangleOrientation orientation = ETriangleOrientation.Upward,
                                                             EMazeAlgorithm algorithm = EMazeAlgorithm.Kruskal)
        {
            return await Task.Run(() => Generate(order, orientation, algorithm));
        }
    }
}
