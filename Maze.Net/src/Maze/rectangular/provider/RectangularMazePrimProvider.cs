using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫生成器
    /// 基于PRIM算法
    /// </summary>
    public class RectangularMazePrimProvider : IRectangularMazeProvider
    {
        // 随机数
        private Random random = new Random();
        // 开链表
        private List<RectangularTile> openlist = new List<RectangularTile>();

        /// <summary>
        /// 
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Prim;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <returns></returns>
        public RectangularField Create(int width, int height)
        {
            width = Utils.Odd(width);
            height = Utils.Odd(height);
            openlist.Clear();

            var field = new RectangularField(width, height);

            // 随机起点
            var x = random.Next(1, field.width - 1);
            var y = random.Next(1, field.height - 1);
            field[x, y] = TileType.Path;

            // 从起点开始探索
            SearchNeighbours(field, x, y);

            while (openlist.Count > 0)
            {
                var idx = random.Next(0, openlist.Count);
                var cur = openlist[idx];

                x = cur.x;
                y = cur.y;

                switch (cur.d)
                {
                    case (int)Dir.Up:
                        y = y - 1;
                        break;
                    case (int)Dir.Down:
                        y = y + 1;
                        break;
                    case (int)Dir.Left:
                        x = x - 1;
                        break;
                    case (int)Dir.Right:
                        x = x + 1;
                        break;
                }

                if (Utils.IsWall(field, x, y))
                {
                    field[cur.x, cur.y] = TileType.Path;
                    if (!Utils.IsBorder(field, x, y))
                    {
                        field[x, y] = TileType.Path;
                        SearchNeighbours(field, x, y);
                    }
                }
                openlist.RemoveAt(idx);
            }

            return field;
        }

        /// <summary>
        /// 获取邻居
        /// </summary>
        /// <param name="field"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SearchNeighbours(RectangularField field, int x, int y)
        {
            // 上
            if (!Utils.IsBorder(field, x, y - 1) && Utils.IsWall(field, x, y - 1)) openlist.Add(new RectangularTile(x, y - 1, (int)Dir.Up));
            // 下
            if (!Utils.IsBorder(field, x, y + 1) && Utils.IsWall(field, x, y + 1)) openlist.Add(new RectangularTile(x, y + 1, (int)Dir.Down));
            // 左
            if (!Utils.IsBorder(field, x - 1, y) && Utils.IsWall(field, x - 1, y)) openlist.Add(new RectangularTile(x - 1, y, (int)Dir.Left));
            // 右
            if (!Utils.IsBorder(field, x + 1, y) && Utils.IsWall(field, x + 1, y)) openlist.Add(new RectangularTile(x + 1, y, (int)Dir.Right));
        }
    }
}