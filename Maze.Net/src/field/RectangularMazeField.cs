using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 矩形迷宫场地（邻接表方案）
    /// </summary>
    public class RectangularMazeField : IMazeField
    {
        private readonly List<List<Edge>> _graph;

        public int width { get; }
        public int height { get; }
        public int count => width * height;
        public List<List<Edge>> graph => _graph;
        public int rows => height;

        public RectangularMazeField(int width, int height)
        {
            this.width = Math.Max(1, width);
            this.height = Math.Max(1, height);
            _graph = BuildGraph();
        }

        private List<List<Edge>> BuildGraph()
        {
            var g = new List<List<Edge>>(count);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var edges = new List<Edge>();

                    // 右邻居
                    if (x < width - 1)
                    {
                        int neighbor = y * width + (x + 1);
                        edges.Add(new Edge(neighbor, new LineBorder(x + 1, y, x + 1, y + 1)));
                    }
                    // 左邻居
                    if (x > 0)
                    {
                        int neighbor = y * width + (x - 1);
                        edges.Add(new Edge(neighbor, new LineBorder(x, y, x, y + 1)));
                    }
                    // 下邻居
                    if (y < height - 1)
                    {
                        int neighbor = (y + 1) * width + x;
                        edges.Add(new Edge(neighbor, new LineBorder(x, y + 1, x + 1, y + 1)));
                    }
                    // 上邻居
                    if (y > 0)
                    {
                        int neighbor = (y - 1) * width + x;
                        edges.Add(new Edge(neighbor, new LineBorder(x, y, x + 1, y)));
                    }

                    // 边界边
                    if (x == 0)
                        edges.Add(new Edge(-1, new LineBorder(0, y, 0, y + 1)));
                    if (x == width - 1)
                        edges.Add(new Edge(-1, new LineBorder(width, y, width, y + 1)));
                    if (y == 0)
                        edges.Add(new Edge(-1, new LineBorder(x, 0, x + 1, 0)));
                    if (y == height - 1)
                        edges.Add(new Edge(-1, new LineBorder(x, height, x + 1, height)));

                    g.Add(edges);
                }
            }

            return g;
        }

        public void RemoveBorders(List<(int, int)> spanningTree)
        {
            foreach (var (u, v) in spanningTree)
            {
                for (int i = 0; i < _graph[u].Count; i++)
                {
                    if (_graph[u][i].Neighbor == v)
                    {
                        _graph[u][i].Border = null;
                        break;
                    }
                }
                for (int i = 0; i < _graph[v].Count; i++)
                {
                    if (_graph[v][i].Neighbor == u)
                    {
                        _graph[v][i].Border = null;
                        break;
                    }
                }
            }
        }

        public int GetTileIndex(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                throw new ArgumentOutOfRangeException();
            return y * width + x;
        }

        public int GetTileIndex(Tile tile)
        {
            return GetTileIndex(tile.lateral, tile.radial);
        }

        public Tile GetTileByIndex(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new Tile(index % width, index / width);
        }

        public int GetRow(Tile tile)
        {
            return tile.radial;
        }

        public List<Tile> GetTilesInRow(int row)
        {
            if (row < 0 || row >= height)
                throw new ArgumentOutOfRangeException(nameof(row));
            var tiles = new List<Tile>(width);
            for (int x = 0; x < width; x++)
                tiles.Add(new Tile(x, row));
            return tiles;
        }
    }
}
