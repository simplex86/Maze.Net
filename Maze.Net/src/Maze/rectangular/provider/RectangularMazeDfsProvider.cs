using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于深度优先搜索算法生成随机迷宫
    /// </summary>
    public class RectangularMazeDfsProvider : IRectangularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.DFS;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="width">迷宫宽度</param>
        /// <param name="height">迷宫高度</param>
        /// <returns>生成的迷宫场地</returns>
        public RectangularField Create(int width, int height)
        {
            width  = Utils.Odd(width);
            height = Utils.Odd(height);

            var field = new RectangularField(width, height);

            // 随机选择起点（必须是奇数坐标）
            int x = random.Next(1, width  / 2) * 2 + 1;
            int y = random.Next(1, height / 2) * 2 + 1;
            field[x, y] = TileType.Path;

            // 使用栈实现深度优先搜索
            var stack = new Stack<RectangularTile>();
            stack.Push(new RectangularTile(x, y));

            while (stack.Count > 0)
            {
                // 获取当前位置
                var current = stack.Peek();
                int cx = current.x;
                int cy = current.y;

                // 获取未访问的邻居列表
                SearchNeighbours(field, cx, cy);

                if (neighbors.Count > 0)
                {
                    // 随机选择一个邻居
                    int idx = random.Next(neighbors.Count);
                    var neighbor = neighbors[idx];
                    int nx = neighbor.x;
                    int ny = neighbor.y;

                    // 打通中间的墙（偶数坐标位置）
                    int wx = (cx + nx) / 2;
                    int wy = (cy + ny) / 2;
                    field[wx, wy] = TileType.Path;

                    // 标记邻居为路径
                    field[nx, ny] = TileType.Path;

                    // 压入栈继续探索
                    stack.Push(new RectangularTile(nx, ny));
                }
                else
                {
                    // 回溯：没有未访问邻居，弹出当前位置
                    stack.Pop();
                }
            }

            return field;
        }

        private List<RectangularTile> neighbors = new List<RectangularTile>();

        /// <summary>
        /// 获取指定位置的未访问邻居
        /// 邻居位置为隔一格的奇数坐标（符合墙-路径交替结构）
        /// </summary>
        /// <param name="field">迷宫场地</param>
        /// <param name="x">当前X坐标</param>
        /// <param name="y">当前Y坐标</param>
        /// <returns>未访问邻居列表</returns>
        private void SearchNeighbours(RectangularField field, int x, int y)
        {
            neighbors.Clear();

            // 上（隔一格）
            if (!Utils.IsBorder(field, x, y - 2) && Utils.IsWall(field, x, y - 2)) neighbors.Add(new RectangularTile(x, y - 2));
            // 下（隔一格）
            if (!Utils.IsBorder(field, x, y + 2) && Utils.IsWall(field, x, y + 2)) neighbors.Add(new RectangularTile(x, y + 2));
            // 左（隔一格）
            if (!Utils.IsBorder(field, x - 2, y) && Utils.IsWall(field, x - 2, y)) neighbors.Add(new RectangularTile(x - 2, y));
            // 右（隔一格）
            if (!Utils.IsBorder(field, x + 2, y) && Utils.IsWall(field, x + 2, y)) neighbors.Add(new RectangularTile(x + 2, y));
        }
    }
}