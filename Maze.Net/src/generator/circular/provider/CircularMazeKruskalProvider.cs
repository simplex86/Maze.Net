using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于Kruskal算法生成随机迷宫
    /// </summary>
    public class CircularMazeKruskalProvider : ICircularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Kruskal;

        /// <summary>
        /// 墙类型
        /// </summary>
        private enum WallType
        {
            Radial,  // 径向墙
            Inner    // 内圈墙
        }

        /// <summary>
        /// 墙
        /// </summary>
        private struct Wall
        {
            public WallType type;
            public int ring;
            public int sector;
            public int tile1Index;
            public int tile2Index;

            public Wall(WallType t, int r, int s, int idx1, int idx2)
            {
                type = t;
                ring = r;
                sector = s;
                tile1Index = idx1;
                tile2Index = idx2;
            }
        }

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
            var cells = field.GetTotalCells();

            // 收集所有墙，预先计算索引
            var walls = new List<Wall>();

            // 收集径向墙（同圈相邻扇形之间的墙）
            for (var r = 0; r < field.rings; r++)
            {
                var sectorsInRing = field.GetSectorsInRing(r);
                for (var s = 0; s < sectorsInRing; s++)
                {
                    var idx1 = field.GetTileIndex(r, s);
                    var idx2 = field.GetTileIndex(r, (s + 1) % sectorsInRing);
                    walls.Add(new Wall(WallType.Radial, r, s, idx1, idx2));
                }
            }

            // 收集内圈墙（相邻圈之间的墙）
            for (var r = 0; r < field.rings - 1; r++)
            {
                // 关键修复：使用外圈的扇形数，而不是内圈的
                var sectorsInOuterRing = field.GetSectorsInRing(r + 1);
                for (var s = 0; s < sectorsInOuterRing; s++)
                {
                    var outerRing = r + 1;
                    var outerSector = s;
                    var innerRing = r;
                    var innerSector = field.MapSector(outerRing, outerSector, innerRing);
                    
                    var idx1 = field.GetTileIndex(innerRing, innerSector);
                    var idx2 = field.GetTileIndex(outerRing, outerSector);
                    
                    walls.Add(new Wall(WallType.Inner, r, s, idx1, idx2));
                }
            }

            // 打乱墙的顺序
            walls.Shuffle(random);

            // 管理连通性
            var dsu = new DisjointSet(cells);

            // 逐个尝试打通墙
            foreach (var wall in walls)
            {
                // 如果两个格子不在同一连通分量，则打通墙
                if (dsu.Union(wall.tile1Index, wall.tile2Index))
                {
                    // 打通对应的墙
                    if (wall.type == WallType.Radial)
                    {
                        field.SetRadialWall(wall.ring, wall.sector, false);
                    }
                    else
                    {
                        field.SetInnerWall(wall.ring, wall.sector, false);
                    }

                    // 所有格子已连通时提前退出
                    if (dsu.Count == 1) break;
                }
            }

            return field;
        }
    }
}
