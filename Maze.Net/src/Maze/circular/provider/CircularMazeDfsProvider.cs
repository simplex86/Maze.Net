using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于深度优先搜索算法生成随机迷宫
    /// </summary>
    public class CircularMazeDfsProvider : ICircularMazeProvider
    {
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.DFS;

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
            var visited = new bool[field.rings][];
            
            for (int r = 0; r < field.rings; r++)
            {
                visited[r] = new bool[field.GetSectorsInRing(r)];
            }

            // 随机选择起点
            int startRing = random.Next(field.rings);
            int startSector = random.Next(field.GetSectorsInRing(startRing));
            visited[startRing][startSector] = true;

            // 使用栈实现深度优先搜索
            var stack = new Stack<Tile>();
            stack.Push(new Tile(startRing, startSector));

            while (stack.Count > 0)
            {
                var current = stack.Peek();
                int cr = current.lateral;
                int cs = current.radial;

                var neighbors = GetUnvisitedNeighbors(field, visited, cr, cs);

                if (neighbors.Count > 0)
                {
                    // 随机选择一个邻居
                    int idx = random.Next(neighbors.Count);
                    var neighbor = neighbors[idx];
                    int nr = neighbor.lateral;
                    int ns = neighbor.radial;

                    // 打通相邻格子之间的墙
                    RemoveWall(field, cr, cs, nr, ns);

                    // 标记为已访问
                    visited[nr][ns] = true;
                    stack.Push(new Tile(nr, ns));
                }
                else
                {
                    // 回溯
                    stack.Pop();
                }
            }

            return field;
        }

        /// <summary>
        /// 获取未访问的邻居列表
        /// </summary>
        private List<Tile> GetUnvisitedNeighbors(CircularMazeField field, bool[][] visited, int ring, int sector)
        {
            var neighbors = new List<Tile>();
            int sectorsInRing = field.GetSectorsInRing(ring);

            // 内圈邻居
            if (ring > 0)
            {
                int innerRing = ring - 1;
                int innerSector = field.MapSector(ring, sector, innerRing);
                if (!visited[innerRing][innerSector])
                {
                    neighbors.Add(new Tile(innerRing, innerSector));
                }
            }

            // 外圈邻居
            if (ring < field.rings - 1)
            {
                int outerRing = ring + 1;
                int outerSector = field.MapSector(ring, sector, outerRing);
                if (!visited[outerRing][outerSector])
                {
                    neighbors.Add(new Tile(outerRing, outerSector));
                }
            }

            // 左邻居（逆时针）
            int leftSector = field.GetPrevSector(ring, sector);
            if (!visited[ring][leftSector])
            {
                neighbors.Add(new Tile(ring, leftSector));
            }

            // 右邻居（顺时针）
            int rightSector = field.GetNextSector(ring, sector);
            if (!visited[ring][rightSector])
            {
                neighbors.Add(new Tile(ring, rightSector));
            }

            return neighbors;
        }

        /// <summary>
        /// 移除相邻格子之间的墙
        /// </summary>
        private void RemoveWall(CircularMazeField field, int r1, int s1, int r2, int s2)
        {
            if (r1 == r2)
            {
                // 同一圈：移除径向墙
                //int sectorsInRing = field.GetSectorsInRing(r1);
                int wallSector = Math.Min(s1, s2);
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
                int wallRing = Math.Min(r1, r2);
                //int fromRing = r1 < r2 ? r1 : r2;
                int fromSector = r1 < r2 ? s1 : s2;
                field.SetInnerWall(wallRing, fromSector, false);
            }
        }
    }
}
