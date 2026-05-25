namespace SimplexLab.Maze
{
    /// <summary>
    /// 并查集 (Disjoint Set Union, DSU) 实现
    /// 支持路径压缩和按秩合并优化，用于高效管理和合并不相交的集合
    /// </summary>
    public class DisjointSet
    {
        /// <summary>
        /// 父节点数组，parent[i]表示元素i的父节点
        /// 如果parent[i] == i，则i是该集合的根节点
        /// </summary>
        private int[] parent;

        /// <summary>
        /// 秩数组，rank[i]表示以i为根的树的高度（秩）
        /// 用于按秩合并优化，保持树的平衡
        /// </summary>
        private int[] rank;

        /// <summary>
        /// 集合数量，初始时每个元素都是一个独立的集合
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 初始化并查集
        /// </summary>
        /// <param name="size">元素数量</param>
        public DisjointSet(int size)
        {
            // 初始化父节点数组，每个元素的父节点是自己
            parent = new int[size];
            // 初始化秩数组，初始秩为0（每个元素都是高度为0的树）
            rank = new int[size];
            // 初始集合数量等于元素数量
            Count = size;

            for (int i = 0; i < size; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }
        }

        /// <summary>
        /// 查找元素x所在集合的根节点
        /// 使用路径压缩优化，使后续查询更快
        /// </summary>
        /// <param name="x">要查找的元素</param>
        /// <returns>元素x所在集合的根节点</returns>
        public int Find(int x)
        {
            // 如果x不是根节点，递归查找其父节点的根，并进行路径压缩
            if (parent[x] != x)
            {
                parent[x] = Find(parent[x]); // 路径压缩：直接将x的父节点指向根
            }
            return parent[x];
        }

        /// <summary>
        /// 合并两个元素所在的集合
        /// 使用按秩合并优化，保持树的高度尽可能小
        /// </summary>
        /// <param name="x">第一个元素</param>
        /// <param name="y">第二个元素</param>
        /// <returns>如果两个元素原本不在同一集合则返回true，否则返回false</returns>
        public bool Union(int x, int y)
        {
            // 查找两个元素的根节点
            int rootX = Find(x);
            int rootY = Find(y);

            // 如果已经在同一集合，无需合并
            if (rootX == rootY)
                return false;

            // 按秩合并：将秩较小的树合并到秩较大的树下
            if (rank[rootX] < rank[rootY])
            {
                parent[rootX] = rootY;
            }
            else
            {
                parent[rootY] = rootX;
                // 如果两个树的秩相等，合并后秩增加1
                if (rank[rootX] == rank[rootY])
                {
                    rank[rootX]++;
                }
            }

            // 合并后集合数量减少1
            Count--;
            return true;
        }

    }
}