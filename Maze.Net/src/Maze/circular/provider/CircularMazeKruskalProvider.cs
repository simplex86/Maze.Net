using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于Kruskal算法生成随机迷宫
    /// 设计B正宗做法：打通格子之间的墙
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
        /// 墙信息
        /// </summary>
        private struct Wall
        {
            public WallType type;
            public int ring;
            public int sector;

            public Wall(WallType t, int r, int s)
            {
                type = t;
                ring = r;
                sector = s;
            }
        }

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

            // 收集所有墙
            var walls = new List<Wall>();

            // 收集径向墙（同圈相邻扇形之间的墙）
            for (int r = 0; r < field.rings; r++)
            {
                int sectorsInRing = field.GetSectorsInRing(r);
                for (int s = 0; s < sectorsInRing; s++)
                {
                    walls.Add(new Wall(WallType.Radial, r, s));
                }
            }

            // 收集内圈墙（相邻圈之间的墙）
            for (int r = 0; r < field.rings - 1; r++)
            {
                int sectorsInRing = field.GetSectorsInRing(r);
                for (int s = 0; s < sectorsInRing; s++)
                {
                    walls.Add(new Wall(WallType.Inner, r, s));
                }
            }

            // 随机打乱墙的顺序
            Shuffle(walls);

            // 使用并查集管理连通性
            var parent = new Dictionary<CircularTile, CircularTile>();
            for (int r = 0; r < field.rings; r++)
            {
                int sectorsInRing = field.GetSectorsInRing(r);
                for (int s = 0; s < sectorsInRing; s++)
                {
                    var tile = new CircularTile(r, s);
                    parent[tile] = tile;
                }
            }

            // Kruskal主循环：逐个尝试打通墙
            foreach (var wall in walls)
            {
                CircularTile tile1, tile2;

                if (wall.type == WallType.Radial)
                {
                    // 径向墙：连接同圈的相邻扇形
                    tile1 = new CircularTile(wall.ring, wall.sector);
                    int sectorsInRing = field.GetSectorsInRing(wall.ring);
                    int nextSector = (wall.sector + 1) % sectorsInRing;
                    tile2 = new CircularTile(wall.ring, nextSector);
                }
                else
                {
                    // 内圈墙：连接内圈和外圈
                    tile1 = new CircularTile(wall.ring, wall.sector);
                    int outerRing = wall.ring + 1;
                    int outerSector = field.MapSector(wall.ring, wall.sector, outerRing);
                    tile2 = new CircularTile(outerRing, outerSector);
                }

                var root1 = Find(parent, tile1);
                var root2 = Find(parent, tile2);

                // 如果不在同一集合，则打通墙
                if (!root1.Equals(root2))
                {
                    parent[root1] = root2;

                    // 打通对应的墙
                    if (wall.type == WallType.Radial)
                    {
                        field.SetRadialWall(wall.ring, wall.sector, false);
                    }
                    else
                    {
                        field.SetInnerWall(wall.ring, wall.sector, false);
                    }
                }
            }

            return field;
        }

        /// <summary>
        /// 查找根节点（带路径压缩）
        /// </summary>
        private CircularTile Find(Dictionary<CircularTile, CircularTile> parent, CircularTile tile)
        {
            if (!parent.ContainsKey(tile))
            {
                parent[tile] = tile;
            }

            if (!parent[tile].Equals(tile))
            {
                parent[tile] = Find(parent, parent[tile]);
            }
            return parent[tile];
        }

        /// <summary>
        /// Fisher-Yates 洗牌算法
        /// </summary>
        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
