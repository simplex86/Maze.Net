using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于Aldous-Broder算法生成随机迷宫：最简单的随机迷宫算法，使用纯粹的随机游走
    /// 设计B正宗做法：打通格子之间的墙
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
        /// 创建迷宫（向后兼容）
        /// </summary>
        public CircularField Create(int rings, int sectors)
        {
            return Create(rings, sectors, SectorStrategy.Each);
        }

        /// <summary>
        /// 创建迷宫
        /// </summary>
        /// <param name="rings">圈数</param>
        /// <param name="sectors">最大扇形数（最外圈）</param>
        /// <param name="strategy">扇形分割策略（可选）</param>
        public CircularField Create(int rings, int sectors, SectorStrategy strategy)
        {
            var field = new CircularField(rings, sectors, strategy);

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

            // 随机选择起点
            int currentRing = random.Next(field.rings);
            int currentSector = random.Next(field.GetSectorsInRing(currentRing));
            visited[currentRing][currentSector] = true;
            int visitedCount = 1;

            // Aldous-Broder算法主循环
            CircularTile currentTile = new CircularTile(currentRing, currentSector);
            while (visitedCount < totalCells)
            {
                // 获取随机邻居
                var neighbors = GetNeighbors(field, currentTile.ring, currentTile.sector);
                int idx = random.Next(neighbors.Count);
                CircularTile next = neighbors[idx];

                // 如果邻居未访问，则打通墙并标记为已访问
                if (!visited[next.ring][next.sector])
                {
                    RemoveWall(field, currentTile, next);
                    visited[next.ring][next.sector] = true;
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
        private List<CircularTile> GetNeighbors(CircularField field, int ring, int sector)
        {
            var neighbors = new List<CircularTile>();

            // 内圈邻居
            if (ring > 0)
            {
                int innerSector = field.MapSector(ring, sector, ring - 1);
                neighbors.Add(new CircularTile(ring - 1, innerSector));
            }

            // 外圈邻居
            if (ring < field.rings - 1)
            {
                int outerSector = field.MapSector(ring, sector, ring + 1);
                neighbors.Add(new CircularTile(ring + 1, outerSector));
            }

            // 逆时针邻居
            int sectorsInRing = field.GetSectorsInRing(ring);
            int prevSector = (sector - 1 + sectorsInRing) % sectorsInRing;
            neighbors.Add(new CircularTile(ring, prevSector));

            // 顺时针邻居
            int nextSector = (sector + 1) % sectorsInRing;
            neighbors.Add(new CircularTile(ring, nextSector));

            return neighbors;
        }

        /// <summary>
        /// 移除两个相邻格子之间的墙
        /// </summary>
        private void RemoveWall(CircularField field, CircularTile tile1, CircularTile tile2)
        {
            if (tile1.ring == tile2.ring)
            {
                // 同圈：移除径向墙
                int r = tile1.ring;
                int sectorsInRing = field.GetSectorsInRing(r);
                int s1 = Math.Min(tile1.sector, tile2.sector);
                int s2 = Math.Max(tile1.sector, tile2.sector);
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
                int innerRing = Math.Min(tile1.ring, tile2.ring);
                int innerSector = tile1.ring == innerRing ? tile1.sector : tile2.sector;
                field.SetInnerWall(innerRing, innerSector, false);
            }
        }
    }
}
