using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 阶梯形迷宫场地（邻接表方案）
    /// 从上到下，每一行增加一个方格
    /// 第 i 行有 i+1 个方格（i 从 0 开始）
    /// 总格子数 = Steps * (Steps + 1) / 2
    /// </summary>
    public class StairwayMazeField : MazeField
    {
        public int Steps { get; }

        public StairwayMazeField(int steps)
        {
            Shape = EMazeShape.Stairway;
            Steps = Math.Max(1, steps);
            VertexCount = Steps * (Steps + 1) / 2;
            Graph = BuildGraph();
        }

        private List<List<Adjacency>> BuildGraph()
        {
            var g = new List<List<Adjacency>>(VertexCount);

            for (int row = 0; row < Steps; row++)
            {
                int colsInRow = row + 1;
                for (int col = 0; col < colsInRow; col++)
                {
                    var edges = new List<Adjacency>();

                    // 右邻居
                    if (col < colsInRow - 1)
                    {
                        var neighbor = VertexIndex(row, col + 1);
                        edges.Add(new Adjacency(neighbor, new LineBorder(col + 1, row, col + 1, row + 1)));
                    }
                    // 左邻居
                    if (col > 0)
                    {
                        var neighbor = VertexIndex(row, col - 1);
                        edges.Add(new Adjacency(neighbor, new LineBorder(col, row, col, row + 1)));
                    }
                    // 下邻居（row+1 有 row+2 个格子，col <= row < row+2，总是有效）
                    if (row < Steps - 1)
                    {
                        var neighbor = VertexIndex(row + 1, col);
                        edges.Add(new Adjacency(neighbor, new LineBorder(col, row + 1, col + 1, row + 1)));
                    }
                    // 上邻居（row-1 有 row 个格子，col 必须 < row）
                    if (row > 0 && col < row)
                    {
                        var neighbor = VertexIndex(row - 1, col);
                        edges.Add(new Adjacency(neighbor, new LineBorder(col, row, col + 1, row)));
                    }

                    // 边界边
                    if (col == 0)               edges.Add(new Adjacency(-1, new LineBorder(0, row, 0, row + 1)));
                    if (col == colsInRow - 1)   edges.Add(new Adjacency(-1, new LineBorder(col + 1, row, col + 1, row + 1)));
                    if (row == Steps - 1)       edges.Add(new Adjacency(-1, new LineBorder(col, row + 1, col + 1, row + 1)));
                    if (row == 0 || col == row) edges.Add(new Adjacency(-1, new LineBorder(col, row, col + 1, row)));

                    g.Add(edges);
                }
            }

            return g;
        }

        internal int VertexIndex(int row, int col)
        {
            return row * (row + 1) / 2 + col;
        }
    }
}
