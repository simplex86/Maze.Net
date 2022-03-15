using System;
using System.Collections.Generic;

namespace SimpleX.Maze
{
    public class RectangleMaze : IRectangleMaze
    {
        // 格子
        private class Tile
        {
            public int x = 0;
            public int y = 0;
            public int d = 0;

            public Tile(int x, int y, int d)
            {
                this.x = x;
                this.y = y;
                this.d = d;
            }
        }
        // 方向
        private static class Dir
        {
            public const int Up    = 1; //上
            public const int Down  = 2; //下
            public const int Left  = 4; //左
            public const int Right = 8; //右
        }

        // 随机数
        private Random random = new Random();
        // 开链表
        private List<Tile> openlist = new List<Tile>();

        public int width { get; set; } = 25;
        public int height { get; set; } = 25;

        public RectangleMaze(int width, int height)
        {
            this.width = Odd(width);
            this.height = Odd(height);
        }

        // 创建迷宫
        public override RectangleMazeField Create()
        {
            RectangleMazeField field = new RectangleMazeField(width, height);

            // 随机起点
            int x = random.Next(1, field.width - 1); 
            int y = random.Next(1, field.height - 1);
            field[x, y] = TileType.Path;

            // 从起点开始探索
            SearchNeighbours(field, x, y);
            while (openlist.Count > 0)
            {
                int idx = random.Next(0, openlist.Count);
                var cur = openlist[idx];

                x = cur.x;
                y = cur.y;

                switch (cur.d)
                {
                case Dir.Up:
                    y = y - 1;
                    break;
                case Dir.Down:
                    y = y + 1;
                    break;
                case Dir.Left:
                    x = x - 1;
                    break;
                case Dir.Right:
                    x = x + 1;
                    break;
                }

                if (IsWall(field, x, y))
                {
                    field[cur.x, cur.y] = TileType.Path;
                    if (!IsBorder(field, x, y))
                    {
                        field[x, y] = TileType.Path;
                        SearchNeighbours(field, x, y);
                    }
                }
                openlist.RemoveAt(idx);
            }

            return field;
        }

        // 获取邻居
        private void SearchNeighbours(RectangleMazeField field, int x, int y)
        {
            // 上
            if (!IsBorder(field, x, y - 1) && IsWall(field, x, y - 1)) openlist.Add(new Tile(x, y - 1, Dir.Up));
            // 下
            if (!IsBorder(field, x, y + 1) && IsWall(field, x, y + 1)) openlist.Add(new Tile(x, y + 1, Dir.Down));
            // 左
            if (!IsBorder(field, x - 1, y) && IsWall(field, x - 1, y)) openlist.Add(new Tile(x - 1, y, Dir.Left));
            // 右
            if (!IsBorder(field, x + 1, y) && IsWall(field, x + 1, y)) openlist.Add(new Tile(x + 1, y, Dir.Right));
        }
    }
}