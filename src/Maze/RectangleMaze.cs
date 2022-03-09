using System;
using System.Collections.Generic;

namespace SimpleX.Maze
{
    public class RectangleMaze
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
        // 
        private List<Tile> openlist = new List<Tile>();

        // 创建迷宫
        public RectangleMazeField Create(int width, int height)
        {
            width  = (width  / 2) * 2 + 1;
            height = (height / 2) * 2 + 1;

            RectangleMazeField field = new RectangleMazeField(width, height);

            // 随机起点
            int x = random.Next(1, field.width - 1); 
            int y = random.Next(1, field.height - 1);
            field[x, y] = TileType.Path;

            // string text = "";
            // text = AppendFrame(text, field);

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
                // text = AppendFrameDelta(text, cur.x, cur.y, cur.d, x, y);

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
                // text = AppendFrame(text, field);
            }
            // PrintFrames(text);

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

        // 是否为墙
        private bool IsWall(RectangleMazeField field, int x, int y)
        {
            return field[x, y] == TileType.Wall;
        }

        // 是否为迷宫的边界
        private bool IsBorder(RectangleMazeField field, int x, int y)
        {
            return (x <= 0 || x >= field.width-1 || y <= 0 || y >= field.height-1);
        }

        private string AppendFrame(string text, RectangleMazeField field)
        {
            for (int y=0; y<field.height; y++)
            {
                for (int x=0; x<field.width; x++)
                {
                    text += $"{field[x, y]} ";
                }
                text += "\n";
            }

            return text;
        }

        private string AppendFrameDelta(string text, int cx, int cy, int cd, int nx, int ny)
        {
            text += $"\n{cx}, {cy}, {cd} | {nx}, {ny}\n\n";
            return text;
        }

        private void PrintFrames(string text)
        {
            System.IO.File.WriteAllText("D:/maze.txt", text);
        }
    }
}