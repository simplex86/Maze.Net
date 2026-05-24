using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于Prim算法生成随机迷宫
    /// </summary>
    public class CircularMazePrimProvider : ICircularMazeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Prim;

        /// <summary>
        /// 创建迷宫
        /// </summary>
        public CircularMazeField Create(int rings, int sectors)
        {
            return Create(rings, sectors, SectorStrategy.Arc);
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
            var visited = new bool[field.rings][];
            
            for (var r = 0; r < field.rings; r++)
            {
                visited[r] = new bool[field.GetSectorsInRing(r)];
            }

            // 从最内圈的任意扇形开始
            var startRing = 0;
            var startSector = random.Next(field.GetSectorsInRing(startRing));
            visited[startRing][startSector] = true;

            // 存储边缘（与已访问区域相邻的墙）
            var edges = new List<Tuple<Tile, Tile>>();
            AddEdges(field, visited, edges, startRing, startSector);

            while (edges.Count > 0)
            {
                // 随机选择一条边缘
                var idx = random.Next(edges.Count);
                var edge = edges[idx];
                edges.RemoveAt(idx);

                var tile1 = edge.Item1;
                var tile2 = edge.Item2;

                // 如果另一边未访问，则打通
                if (!visited[tile2.lateral][tile2.radial])
                {
                    // 移除墙
                    RemoveWall(field, tile1.lateral, tile1.radial, tile2.lateral, tile2.radial);
                    
                    // 标记为已访问
                    visited[tile2.lateral][tile2.radial] = true;
                    
                    // 添加新格子的边缘
                    AddEdges(field, visited, edges, tile2.lateral, tile2.radial);
                }
            }

            return field;
        }

        /// <summary>
        /// 添加指定格子的边缘
        /// </summary>
        private void AddEdges(CircularMazeField field, bool[][] visited, List<Tuple<Tile, Tile>> edges, int ring, int sector)
        {
            // 内圈邻居
            if (ring > 0)
            {
                var innerRing = ring - 1;
                var innerSector = field.MapSector(ring, sector, innerRing);
                if (!visited[innerRing][innerSector])
                {
                    edges.Add(Tuple.Create(new Tile(ring, sector), new Tile(innerRing, innerSector)));
                }
            }

            // 外圈邻居
            if (ring < field.rings - 1)
            {
                var outerRing = ring + 1;
                int innerSectors = field.GetSectorsInRing(ring);
                int outerSectors = field.GetSectorsInRing(outerRing);
                int firstOuter = (sector * outerSectors) / innerSectors;
                int lastOuter = ((sector + 1) * outerSectors) / innerSectors;
                for (int os = firstOuter; os < lastOuter; os++)
                {
                    if (!visited[outerRing][os])
                    {
                        edges.Add(Tuple.Create(new Tile(ring, sector), new Tile(outerRing, os)));
                    }
                }
            }

            // 左邻居（逆时针）
            var leftSector = field.GetPrevSector(ring, sector);
            if (!visited[ring][leftSector])
            {
                edges.Add(Tuple.Create(new Tile(ring, sector), new Tile(ring, leftSector)));
            }

            // 右邻居（顺时针）
            var rightSector = field.GetNextSector(ring, sector);
            if (!visited[ring][rightSector])
            {
                edges.Add(Tuple.Create(new Tile(ring, sector), new Tile(ring, rightSector)));
            }
        }

        /// <summary>
        /// 移除相邻格子之间的墙
        /// </summary>
        private void RemoveWall(CircularMazeField field, int r1, int s1, int r2, int s2)
        {
            if (r1 == r2)
            {
                var wallSector = Math.Min(s1, s2);
                // 特殊情况：边界相邻（s1最大，s2最小）
                if (Math.Abs(s1 - s2) > 1)
                {
                    wallSector = Math.Max(s1, s2);
                }
                field.SetRadialWall(r1, wallSector, false);
            }
            else
            {
                // 不同圈：移除内圈墙
                var wallRing = Math.Min(r1, r2);
                var outerSector = r1 > r2 ? s1 : s2;
                field.SetInnerWall(wallRing, outerSector, false);
            }
        }
    }
}
