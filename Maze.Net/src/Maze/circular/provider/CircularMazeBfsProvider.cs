using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 环形迷宫生成器
    /// 基于广度优先搜索算法生成随机迷宫
    /// 特点：生成的迷宫具有较短的分支，相对均匀的分布
    /// </summary>
    public class CircularMazeBfsProvider : ICircularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.BFS;

        // 用于临时存储邻居信息的结构体
        private struct NeighborInfo
        {
            public int ring;
            public int sector;
            public int parentRing;
            public int parentSector;

            public NeighborInfo(int ring, int sector, int parentRing, int parentSector)
            {
                this.ring = ring;
                this.sector = sector;
                this.parentRing = parentRing;
                this.parentSector = parentSector;
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
        /// <param name="sectors">扇形数</param>
        /// <param name="strategy">策略类型</param>
        /// <returns>生成的迷宫场地</returns>
        public CircularField Create(int rings, int sectors, SectorStrategy strategy)
        {
            var field = new CircularField(rings, sectors, strategy);

            // 初始化visited数组
            var visited = new bool[field.rings][];
            for (int r = 0; r < field.rings; r++)
            {
                visited[r] = new bool[field.GetSectorsInRing(r)];
            }

            // 从最内圈随机选择一个起点
            int startRing = 0;
            int startSector = random.Next(field.GetSectorsInRing(startRing));
            visited[startRing][startSector] = true;

            // 使用队列实现广度优先搜索
            var currentLevel = new Queue<CircularTile>();
            currentLevel.Enqueue(new CircularTile(startRing, startSector));

            while (currentLevel.Count > 0)
            {
                // 收集当前层的所有邻居
                var nextLevel = new List<NeighborInfo>();

                foreach (var tile in currentLevel)
                {
                    var neighbors = GetUnvisitedNeighbors(field, visited, tile.ring, tile.sector);
                    nextLevel.AddRange(neighbors);
                }

                // 随机打乱下一层的顺序，增加迷宫的随机性
                Shuffle(nextLevel);

                // 处理下一层
                var newCurrentLevel = new Queue<CircularTile>();
                foreach (var neighbor in nextLevel)
                {
                    if (!visited[neighbor.ring][neighbor.sector])
                    {
                        // 打通父节点和当前节点之间的墙
                        RemoveWall(field, neighbor.parentRing, neighbor.parentSector, neighbor.ring, neighbor.sector);

                        // 标记为已访问
                        visited[neighbor.ring][neighbor.sector] = true;

                        // 加入新的当前层
                        newCurrentLevel.Enqueue(new CircularTile(neighbor.ring, neighbor.sector));
                    }
                }

                // 更新当前层为新的一层
                currentLevel = newCurrentLevel;
            }

            return field;
        }

        /// <summary>
        /// 获取未访问的邻居列表
        /// </summary>
        private List<NeighborInfo> GetUnvisitedNeighbors(CircularField field, bool[][] visited, int ring, int sector)
        {
            var neighbors = new List<NeighborInfo>();

            int sectorsInRing = field.GetSectorsInRing(ring);

            // 内圈邻居（如果还没有被访问）
            if (ring > 0)
            {
                int innerRing = ring - 1;
                int innerSector = field.MapSector(ring, sector, innerRing);
                if (!visited[innerRing][innerSector])
                {
                    neighbors.Add(new NeighborInfo(innerRing, innerSector, ring, sector));
                }
            }

            // 外圈邻居（如果还没有被访问）
            if (ring < field.rings - 1)
            {
                int outerRing = ring + 1;
                int outerSector = field.MapSector(ring, sector, outerRing);
                if (!visited[outerRing][outerSector])
                {
                    neighbors.Add(new NeighborInfo(outerRing, outerSector, ring, sector));
                }
            }

            // 逆时针邻居（如果还没有被访问）
            int leftSector = field.GetPrevSector(ring, sector);
            if (!visited[ring][leftSector])
            {
                neighbors.Add(new NeighborInfo(ring, leftSector, ring, sector));
            }

            // 顺时针邻居（如果还没有被访问）
            int rightSector = field.GetNextSector(ring, sector);
            if (!visited[ring][rightSector])
            {
                neighbors.Add(new NeighborInfo(ring, rightSector, ring, sector));
            }

            return neighbors;
        }

        /// <summary>
        /// 移除两个相邻格子之间的墙
        /// </summary>
        private void RemoveWall(CircularField field, int ring1, int sector1, int ring2, int sector2)
        {
            // 检查两个格子是否在同一圈
            if (ring1 == ring2)
            {
                // 同一圈的情况，需要移除径向墙
                int ring = ring1;
                int wallSector = Math.Min(sector1, sector2);
                // 处理绕圈的情况（第一个是最后一个的邻居）
                if (Math.Abs(sector1 - sector2) > 1)
                {
                    wallSector = Math.Max(sector1, sector2);
                }
                field.SetRadialWall(ring, wallSector, false);
            }
            // 不同圈的情况，需要移除内圈墙
            else
            {
                int innerRing = Math.Min(ring1, ring2);
                int outerRing = Math.Max(ring1, ring2);
                int innerSector = (ring1 < ring2) ? sector1 : sector2;
                int outerSector = (ring1 < ring2) ? sector2 : sector1;

                // 需要检查内圈的sector是否和外圈的对应
                // 这里我们以较小的圈的sector为基准
                int baseRing = innerRing;
                int baseSector = innerSector;
                field.SetInnerWall(baseRing, baseSector, false);
            }
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
