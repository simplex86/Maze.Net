using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 圆形迷宫生成器
    /// 基于Eller算法生成随机迷宫：逐圈处理，内存效率高
    /// 设计B正宗做法：打通格子之间的墙
    /// </summary>
    public class CircularMazeEllerProvider : ICircularMazeProvider
    {
        private Random random = new Random();

        /// <summary>
        /// 当前算法类型
        /// </summary>
        public MazeAlgorithm algorithm { get; } = MazeAlgorithm.Eller;

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

            // 并查集字典（键：扇形编号，值：集合根）
            Dictionary<int, int> parent = new Dictionary<int, int>();
            int nextSetId = 0;

            // 处理每一圈（从内到外）
            for (int r = 0; r < field.rings; r++)
            {
                int sectorsInRing = field.GetSectorsInRing(r);

                // 初始化当前圈的集合（未连接到内圈的扇形分配新集合）
                for (int s = 0; s < sectorsInRing; s++)
                {
                    if (!parent.ContainsKey(s))
                    {
                        parent[s] = nextSetId++;
                    }
                }

                // ========== 水平连接阶段（同圈连接）==========
                for (int s = 0; s < sectorsInRing; s++)
                {
                    int nextS = (s + 1) % sectorsInRing;
                    int root1 = Find(parent, s);
                    int root2 = Find(parent, nextS);

                    // 随机决定是否连接（最后一圈强制不闭合环形，否则保证每个集合至少有一个垂直连接）
                    if (root1 != root2 && random.Next(2) == 0)
                    {
                        // 打通径向墙
                        field.SetRadialWall(r, s, false);
                        parent[root2] = root1;
                    }
                }

                // ========== 垂直连接阶段（内圈到外圈）==========
                if (r < field.rings - 1)
                {
                    int nextSectors = field.GetSectorsInRing(r + 1);

                    // 确保每个集合至少有一个垂直连接
                    Dictionary<int, List<int>> sets = new Dictionary<int, List<int>>();
                    for (int s = 0; s < sectorsInRing; s++)
                    {
                        int root = Find(parent, s);
                        if (!sets.ContainsKey(root))
                        {
                            sets[root] = new List<int>();
                        }
                        sets[root].Add(s);
                    }

                    // 为每个集合选择至少一个垂直连接
                    bool[] hasVertical = new bool[sectorsInRing];
                    foreach (var set in sets.Values)
                    {
                        int selected = set[random.Next(set.Count)];
                        hasVertical[selected] = true;
                    }

                    // 随机添加额外的垂直连接
                    for (int s = 0; s < sectorsInRing; s++)
                    {
                        if (!hasVertical[s] && random.Next(2) == 0)
                        {
                            hasVertical[s] = true;
                        }
                    }

                    // 执行垂直连接：打通内圈墙
                    Dictionary<int, int> nextParent = new Dictionary<int, int>();
                    for (int s = 0; s < sectorsInRing; s++)
                    {
                        if (hasVertical[s])
                        {
                            field.SetInnerWall(r, s, false);

                            // 计算对应的外圈扇形
                            int outerS = field.MapSector(r, s, r + 1);

                            // 继承集合关系
                            int root = Find(parent, s);
                            nextParent[outerS] = root;
                        }
                    }

                    // 为下一圈准备新的parent，未连接的扇形分配新集合
                    for (int s = 0; s < nextSectors; s++)
                    {
                        if (!nextParent.ContainsKey(s))
                        {
                            nextParent[s] = nextSetId++;
                        }
                    }
                    parent = nextParent;
                }
            }

            return field;
        }

        /// <summary>
        /// 查找根节点（带路径压缩）
        /// </summary>
        private int Find(Dictionary<int, int> parent, int s)
        {
            if (!parent.ContainsKey(s))
            {
                return s;
            }
            if (parent[s] != s)
            {
                parent[s] = Find(parent, parent[s]);
            }
            return parent[s];
        }
    }
}
