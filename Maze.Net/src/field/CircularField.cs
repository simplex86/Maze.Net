using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫场地
    /// 使用极坐标系统：ring（圈数）和 sector（扇形数）
    /// 存储相邻格子之间的墙的状态（设计B正宗做法）
    /// </summary>
    public struct CircularField
    {
        /// <summary>
        /// 内圈墙（径向墙的内半径）- 分隔相邻圈的圆弧墙
        /// [ring][sector]：表示圈 ring 的外半径（与圈 ring+1 之间）的墙是否存在
        /// </summary>
        private bool[][] innerWalls = null;

        /// <summary>
        /// 径向墙（放射状墙）- 分隔同一圈相邻扇形的直线墙
        /// [ring][sector]：表示圈 ring 的 sector 与 sector+1 之间的墙是否存在
        /// </summary>
        private bool[][] radialWalls = null;

        /// <summary>
        /// 每圈的扇形数量缓存
        /// </summary>
        private int[] sectorsPerRing = null;

        /// <summary>
        /// 圈数（从中心向外的层数）
        /// </summary>
        public int rings { get; private set; } = 3;

        /// <summary>
        /// 最大扇形数（最外圈的扇形数量）
        /// </summary>
        public int maxSectors { get; private set; } = 8;

        /// <summary>
        /// 扇形分割策略
        /// </summary>
        public SectorStrategy strategy { get; private set; } = SectorStrategy.Each;

        /// <summary>
        /// 初始化圆形迷宫场地（向后兼容）
        /// </summary>
        public CircularField(int rings, int sectors)
        {
            // 确保最小尺寸
            this.rings = Math.Max(1, rings);
            this.maxSectors = Math.Max(3, sectors);
            this.strategy = SectorStrategy.Each;

            // 计算每圈的扇形数量
            this.sectorsPerRing = new int[this.rings];
            for (int r = 0; r < this.rings; r++)
            {
                this.sectorsPerRing[r] = CalculateSectors(r);
            }

            // 创建墙数组
            innerWalls = new bool[this.rings][];
            radialWalls = new bool[this.rings][];

            // 初始化所有墙为"存在"状态
            for (int r = 0; r < this.rings; r++)
            {
                int sectorsInRing = this.sectorsPerRing[r];
                innerWalls[r] = new bool[sectorsInRing];
                radialWalls[r] = new bool[sectorsInRing];

                for (int s = 0; s < sectorsInRing; s++)
                {
                    innerWalls[r][s] = true;  // 内圈墙存在
                    radialWalls[r][s] = true;  // 径向墙存在
                }
            }
        }

        /// <summary>
        /// 初始化圆形迷宫场地
        /// </summary>
        /// <param name="rings">圈数</param>
        /// <param name="maxSectors">最大扇形数（最外圈）</param>
        public CircularField(int rings, int maxSectors, SectorStrategy strategy)
        {
            // 确保最小尺寸
            this.rings = Math.Max(1, rings);
            this.maxSectors = Math.Max(3, maxSectors);
            this.strategy = strategy;

            // 计算每圈的扇形数量
            this.sectorsPerRing = new int[this.rings];
            for (int r = 0; r < this.rings; r++)
            {
                this.sectorsPerRing[r] = CalculateSectors(r);
            }

            // 创建墙数组
            innerWalls = new bool[this.rings][];
            radialWalls = new bool[this.rings][];

            // 初始化所有墙为"存在"状态
            for (int r = 0; r < this.rings; r++)
            {
                int sectorsInRing = this.sectorsPerRing[r];
                innerWalls[r] = new bool[sectorsInRing];
                radialWalls[r] = new bool[sectorsInRing];

                for (int s = 0; s < sectorsInRing; s++)
                {
                    innerWalls[r][s] = true;  // 内圈墙存在
                    radialWalls[r][s] = true;  // 径向墙存在
                }
            }
        }

        /// <summary>
        /// 根据策略计算某圈的扇形数量
        /// </summary>
        private int CalculateSectors(int ring)
        {
            switch (strategy)
            {
                case SectorStrategy.Each:
                    return maxSectors;

                case SectorStrategy.Arc:
                    // 弧长均匀策略：扇形数与半径成正比
                    // 最内圈最少3个扇形
                    double radiusArc = ring + 1;
                    double minRadiusArc = 1;
                    int sectorsArc = (int)(3 * radiusArc / minRadiusArc);
                    return Math.Max(3, Math.Min(maxSectors, sectorsArc));

                case SectorStrategy.Area:
                    // 面积均匀策略：扇形面积大致相等
                    // 面积 = π((R+1)² - R²)/N = π(2R+1)/N
                    // 保持 N 与 (2R+1) 成正比
                    double radiusArea = ring + 1;
                    double factorArea = 2 * radiusArea + 1;
                    double minFactorArea = 2 * 1 + 1;
                    int sectorsArea = (int)(3 * factorArea / minFactorArea);
                    return Math.Max(3, Math.Min(maxSectors, sectorsArea));

                default:
                    return maxSectors;
            }
        }

        /// <summary>
        /// 获取指定圈的扇形数量
        /// </summary>
        public int GetSectorsInRing(int ring)
        {
            if (ring < 0 || ring >= rings)
                return 0;
            return sectorsPerRing[ring];
        }

        /// <summary>
        /// 将某圈的扇形映射到内圈/外圈的对应扇形
        /// </summary>
        public int MapSector(int fromRing, int fromSector, int toRing)
        {
            int fromSectors = GetSectorsInRing(fromRing);
            int toSectors = GetSectorsInRing(toRing);

            // 比例映射
            return (fromSector * toSectors) / fromSectors;
        }

        /// <summary>
        /// 获取某个格子的下一个扇形（顺时针方向）
        /// </summary>
        public int GetNextSector(int ring, int sector)
        {
            int sectorsInRing = GetSectorsInRing(ring);
            return (sector + 1) % sectorsInRing;
        }

        /// <summary>
        /// 获取某个格子的上一个扇形（逆时针方向）
        /// </summary>
        public int GetPrevSector(int ring, int sector)
        {
            int sectorsInRing = GetSectorsInRing(ring);
            return (sector - 1 + sectorsInRing) % sectorsInRing;
        }

        /// <summary>
        /// 判断是否为最内圈
        /// </summary>
        public bool IsInnermostRing(int ring)
        {
            return ring == 0;
        }

        /// <summary>
        /// 判断是否为最外圈
        /// </summary>
        public bool IsOutermostRing(int ring)
        {
            return ring == rings - 1;
        }

        /// <summary>
        /// 获取格子总数
        /// </summary>
        public int GetTotalCells()
        {
            int total = 0;
            for (int r = 0; r < rings; r++)
            {
                total += GetSectorsInRing(r);
            }
            return total;
        }

        /// <summary>
        /// 获取或设置指定位置的格子类型（保持向后兼容，始终返回Path）
        /// </summary>
        /// <param name="ring">圈数</param>
        /// <param name="sector">扇形索引</param>
        /// <returns>始终返回Path</returns>
        public int this[int ring, int sector]
        {
            get
            {
                return TileType.Path;
            }
            internal set
            {
                // 保持向后兼容，但忽略设置
            }
        }

        /// <summary>
        /// 获取内圈墙状态（分隔圈 ring 和 ring+1 的墙）
        /// </summary>
        public bool GetInnerWall(int ring, int sector)
        {
            if (ring < 0 || ring >= rings - 1 || sector < 0 || sector >= GetSectorsInRing(ring))
                return true;
            return innerWalls[ring][sector];
        }

        /// <summary>
        /// 设置内圈墙状态（分隔圈 ring 和 ring+1 的墙）
        /// </summary>
        public void SetInnerWall(int ring, int sector, bool exists)
        {
            if (ring < 0 || ring >= rings - 1 || sector < 0 || sector >= GetSectorsInRing(ring))
                return;
            innerWalls[ring][sector] = exists;
        }

        /// <summary>
        /// 获取径向墙状态（分隔圈 ring 内 sector 和 sector+1 的墙）
        /// </summary>
        public bool GetRadialWall(int ring, int sector)
        {
            if (ring < 0 || ring >= rings || sector < 0 || sector >= GetSectorsInRing(ring))
                return true;
            return radialWalls[ring][sector];
        }

        /// <summary>
        /// 设置径向墙状态（分隔圈 ring 内 sector 和 sector+1 的墙）
        /// </summary>
        public void SetRadialWall(int ring, int sector, bool exists)
        {
            if (ring < 0 || ring >= rings || sector < 0 || sector >= GetSectorsInRing(ring))
                return;
            radialWalls[ring][sector] = exists;
        }
    }
}
