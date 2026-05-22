using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于Aldous-Broder算法生成随机迷宫：最简单的随机迷宫算法，使用纯粹的随机游走
    /// </summary>
    public class CircularMazeAldousBroderProvider : ICircularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.AldousBroder;

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
            var totalCells = 0;
            for (var r = 0; r < field.rings; r++)
            {
                totalCells += field.GetSectorsInRing(r);
            }

            // 已访问标记
            bool[][] visited = new bool[field.rings][];
            for (var r = 0; r < field.rings; r++)
            {
                visited[r] = new bool[field.GetSectorsInRing(r)];
            }

            // 随机选择起点
            var currentRing = random.Next(field.rings);
            var currentSector = random.Next(field.GetSectorsInRing(currentRing));
            visited[currentRing][currentSector] = true;
            var visitedCount = 1;

            // Aldous-Broder算法主循环
            var currentTile = new Tile(currentRing, currentSector);
            while (visitedCount < totalCells)
            {
                // 获取随机邻居
                var neighbors = GetNeighbors(field, currentTile.lateral, currentTile.radial);
                var idx = random.Next(neighbors.Count);
                var next = neighbors[idx];

                // 如果邻居未访问，则打通墙并标记为已访问
                if (!visited[next.lateral][next.radial])
                {
                    RemoveWall(field, currentTile, next);
                    visited[next.lateral][next.radial] = true;
                    visitedCount++;
                }

                // 移动到邻居
                currentTile = next;
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
                var innerSector = field.MapSector(ring, sector, ring - 1);
                neighbors.Add(new Tile(ring - 1, innerSector));
            }

            // 外圈邻居
            if (ring < field.rings - 1)
            {
                var outerSector = field.MapSector(ring, sector, ring + 1);
                neighbors.Add(new Tile(ring + 1, outerSector));
            }

            // 逆时针邻居
            var sectorsInRing = field.GetSectorsInRing(ring);
            var prevSector = (sector - 1 + sectorsInRing) % sectorsInRing;
            neighbors.Add(new Tile(ring, prevSector));

            // 顺时针邻居
            var nextSector = (sector + 1) % sectorsInRing;
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
                var r = tile1.lateral;
                var sectorsInRing = field.GetSectorsInRing(r);
                var s1 = Math.Min(tile1.radial, tile2.radial);
                var s2 = Math.Max(tile1.radial, tile2.radial);
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
                // 不同圈：移除内圈墙
                var innerRing = Math.Min(tile1.lateral, tile2.lateral);
                var innerSector = tile1.lateral == innerRing ? tile1.radial : tile2.radial;
                field.SetInnerWall(innerRing, innerSector, false);
            }
        }
    }
}
