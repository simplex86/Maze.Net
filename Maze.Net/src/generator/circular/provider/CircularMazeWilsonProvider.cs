using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于Wilson算法生成随机迷宫：使用随机游走，生成的迷宫具有均匀的随机性
    /// </summary>
    public class CircularMazeWilsonProvider : ICircularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Wilson;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        public CircularMazeField Create(int rings, int sectors)
        {
            return Create(rings, sectors, SectorStrategy.Each);
        }

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="rings">圈数</param>
        /// <param name="sectors">最大扇形数（最外圈）</param>
        /// <param name="strategy">扇形分割策略（可选）</param>
        public CircularMazeField Create(int rings, int sectors, SectorStrategy strategy)
        {
            var field = new CircularMazeField(rings, sectors, strategy);

            // 计算总格子数
            int totalCells = 0;
            for (int r = 0; r < field.rings; r++)
            {
                totalCells += field.GetSectorsInRing(r);
            }

            // 已访问标记
            bool[][] visited = new bool[field.rings][];
            for (int r = 0; r < field.rings; r++)
            {
                visited[r] = new bool[field.GetSectorsInRing(r)];
            }

            // 选择起点（最内圈的第一个扇形）
            int startRing = 0;
            int startSector = 0;
            visited[startRing][startSector] = true;
            int visitedCount = 1;

            // Wilson算法主循环
            while (visitedCount < totalCells)
            {
                // 随机选择一个未访问的格子开始随机游走
                int currentRing, currentSector;
                do
                {
                    currentRing = random.Next(field.rings);
                    currentSector = random.Next(field.GetSectorsInRing(currentRing));
                } while (visited[currentRing][currentSector]);

                // 开始随机游走，记录路径
                var path = new Dictionary<Tile, Tile>();
                var current = new Tile(currentRing, currentSector);
                path[current] = current; // 起点指向自己

                while (!visited[current.lateral][current.radial])
                {
                    // 获取随机邻居
                    var neighbors = GetNeighbors(field, current.lateral, current.radial);
                    int idx = random.Next(neighbors.Count);
                    var next = neighbors[idx];

                    // 如果路径中已经包含这个邻居，移除环路
                    if (path.ContainsKey(next))
                    {
                        // 从路径中移除环路
                        var temp = current;
                        while (!temp.Equals(next))
                        {
                            var prev = path[temp];
                            path.Remove(temp);
                            temp = prev;
                        }
                    }
                    else
                    {
                        path[next] = current;
                    }

                    current = next;
                }

                // 将路径打通并标记为已访问
                var pos = path[current];
                while (!pos.Equals(current))
                {
                    // 打通pos和next之间的墙
                    RemoveWall(field, pos, current);

                    // 标记pos为已访问
                    visited[pos.lateral][pos.radial] = true;
                    visitedCount++;

                    // 继续处理下一个
                    current = pos;
                    pos = path[current];
                }
            }

            return field;
        }

        /// <summary>
        /// 获取指定格子的所有邻居
        /// </summary>
        private List<Tile> GetNeighbors(CircularMazeField field, int ring, int sector)
        {
            var neighbors = new List<Tile>();

            // 内圈邻居
            if (ring > 0)
            {
                int innerSector = field.MapSector(ring, sector, ring - 1);
                neighbors.Add(new Tile(ring - 1, innerSector));
            }

            // 外圈邻居
            if (ring < field.rings - 1)
            {
                int outerRing = ring + 1;
                int innerSectors = field.GetSectorsInRing(ring);
                int outerSectors = field.GetSectorsInRing(outerRing);
                int firstOuter = (sector * outerSectors) / innerSectors;
                int lastOuter = ((sector + 1) * outerSectors) / innerSectors;
                for (int os = firstOuter; os < lastOuter; os++)
                {
                    neighbors.Add(new Tile(outerRing, os));
                }
            }

            // 逆时针邻居
            int sectorsInRing = field.GetSectorsInRing(ring);
            int prevSector = (sector - 1 + sectorsInRing) % sectorsInRing;
            neighbors.Add(new Tile(ring, prevSector));

            // 顺时针邻居
            int nextSector = (sector + 1) % sectorsInRing;
            neighbors.Add(new Tile(ring, nextSector));

            return neighbors;
        }

        /// <summary>
        /// 移除两个相邻格子之间的墙
        /// </summary>
        private void RemoveWall(CircularMazeField field, Tile tile1, Tile tile2)
        {
            if (tile1.lateral == tile2.lateral)
            {
                // 同圈：移除径向墙
                int r = tile1.lateral;
                int sectorsInRing = field.GetSectorsInRing(r);
                // 找到较小的扇形编号作为墙的位置
                int s1 = Math.Min(tile1.radial, tile2.radial);
                int s2 = Math.Max(tile1.radial, tile2.radial);
                if (s2 - s1 == 1 || (s1 == 0 && s2 == sectorsInRing - 1))
                {
                    if (s2 == sectorsInRing - 1 && s1 == 0)
                    {
                        field.SetRadialWall(r, s2, false);
                    }
                    else
                    {
                        field.SetRadialWall(r, s1, false);
                    }
                }
            }
            else
            {
                // 不同圈：移除内圈墙（内圈的那个扇形的墙）
                int innerRing = Math.Min(tile1.lateral, tile2.lateral);
                int outerSector = tile1.lateral != innerRing ? tile1.radial : tile2.radial;
                field.SetInnerWall(innerRing, outerSector, false);
            }
        }
    }
}
