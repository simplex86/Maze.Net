using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫场地
    /// 使用极坐标系统：ring（圈数）和 sector（扇形数）
    /// 存储相邻格子之间的墙的状态
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
        public int rings { get; } = 20;

        /// <summary>
        /// 最大扇形数（最外圈的扇形数量）
        /// </summary>
        public int sectors { get; } = 100;

        /// <summary>
        /// 扇形分割策略
        /// </summary>
        public SectorStrategy strategy { get; } = SectorStrategy.Arc;

        /// <summary>
        /// 初始化圆形迷宫场地
        /// </summary>
        public CircularField(int rings, int sectors)
            : this(rings, sectors, SectorStrategy.Arc)
        {

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
            this.sectors = Math.Max(3, maxSectors);
            this.strategy = strategy;

            // 计算每圈的扇形数量
            this.sectorsPerRing = new int[this.rings];
            if (strategy == SectorStrategy.Each)
            {
                for (int r = 0; r < this.rings; r++)
                {
                    this.sectorsPerRing[r] = maxSectors;
                }
            }
            else
            {
                // 1. 从内圈开始，扇形数从较小的值开始
                // 2. 逐圈向外，根据弧长判断是否需要翻倍
                // 3. 保证不超过最大扇形数
                
                // 先找到最大的 2 的幂次，不超过 maxSectors
                var normalizedMaxSectors = 3;
                while (normalizedMaxSectors * 2 <= this.sectors)
                {
                    normalizedMaxSectors *= 2;
                }

                // 从内圈开始计算
                this.sectorsPerRing[0] = 3;  // 最内圈至少 3 个扇形
                for (var r = 1; r < this.rings; r++)
                {
                    this.sectorsPerRing[r] = this.sectorsPerRing[r - 1];
                    // 计算弧长（半径 = r + 1，因为我们从 ring 0 开始）
                    double arcLength = (2 * Math.PI * (r + 1)) / this.sectorsPerRing[r - 1];
                    if (arcLength > 2.0 && this.sectorsPerRing[r] * 2 <= normalizedMaxSectors)
                    {
                        this.sectorsPerRing[r] *= 2;
                    }
                }

                // 确保最外圈至少达到 normalizedMaxSectors
                if (this.sectorsPerRing[this.rings - 1] < normalizedMaxSectors)
                {
                    this.sectorsPerRing[this.rings - 1] = normalizedMaxSectors;
                }
            }

            // 创建墙数组
            innerWalls = new bool[this.rings][];
            radialWalls = new bool[this.rings][];

            // 初始化所有墙为"存在"状态
            for (var r = 0; r < this.rings; r++)
            {
                var sectorsInRing = this.sectorsPerRing[r];
                innerWalls[r] = new bool[sectorsInRing];
                radialWalls[r] = new bool[sectorsInRing];

                for (var s = 0; s < sectorsInRing; s++)
                {
                    innerWalls[r][s] = true;  // 内圈墙存在
                    radialWalls[r][s] = true;  // 径向墙存在
                }
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
            var fromSectors = GetSectorsInRing(fromRing);
            var toSectors = GetSectorsInRing(toRing);

            // 比例映射
            return (fromSector * toSectors) / fromSectors;
        }

        /// <summary>
        /// 获取某个格子的下一个扇形（顺时针方向）
        /// </summary>
        public int GetNextSector(int ring, int sector)
        {
            var sectorsInRing = GetSectorsInRing(ring);
            return (sector + 1) % sectorsInRing;
        }

        /// <summary>
        /// 获取某个格子的上一个扇形（逆时针方向）
        /// </summary>
        public int GetPrevSector(int ring, int sector)
        {
            var sectorsInRing = GetSectorsInRing(ring);
            return (sector - 1 + sectorsInRing) % sectorsInRing;
        }

        /// <summary>
        /// 获取格子总数
        /// </summary>
        public int GetTotalCells()
        {
            var total = 0;
            for (var r = 0; r < rings; r++)
            {
                total += GetSectorsInRing(r);
            }
            return total;
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

        /// <summary>
        /// 将指定的(ring, sector)映射到唯一的整数索引
        /// 用于DisjointSet等需要整数索引的场景
        /// </summary>
        public int GetTileIndex(int ring, int sector)
        {
            if (ring < 0 || ring >= rings || sector < 0 || sector >= GetSectorsInRing(ring))
                throw new ArgumentOutOfRangeException();

            var index = 0;
            for (var r = 0; r < ring; r++)
            {
                index += sectorsPerRing[r];
            }
            return index + sector;
        }
    }
}
