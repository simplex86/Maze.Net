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

        /// <summary>
        /// 判断两个元素是否在同一集合中
        /// </summary>
        /// <param name="x">第一个元素</param>
        /// <param name="y">第二个元素</param>
        /// <returns>如果在同一集合返回true，否则返回false</returns>
        public bool IsConnected(int x, int y)
        {
            return Find(x) == Find(y);
        }
    }

    /// <summary>
    /// 带权重的并查集 (Weighted Disjoint Set)
    /// 扩展普通并查集，支持记录元素到根节点的权重（相对关系）
    /// 主要应用：处理带权关系的动态连通性问题、差分约束系统、图论中的相对距离问题
    /// </summary>
    public class WeightedDisjointSet
    {
        /// <summary>
        /// 父节点数组，parent[i]表示元素i的父节点
        /// </summary>
        private int[] parent;

        /// <summary>
        /// 权重数组，weight[i]表示元素i到其父节点parent[i]的权重
        /// 可用于表示元素间的相对关系（如距离、偏移量等）
        /// </summary>
        private int[] weight;

        /// <summary>
        /// 秩数组，用于按秩合并优化
        /// </summary>
        private int[] rank;

        /// <summary>
        /// 集合数量
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 初始化带权重的并查集
        /// </summary>
        /// <param name="size">元素数量</param>
        public WeightedDisjointSet(int size)
        {
            parent = new int[size];
            weight = new int[size];
            rank = new int[size];
            Count = size;

            for (int i = 0; i < size; i++)
            {
                parent[i] = i;   // 每个元素的父节点是自己
                weight[i] = 0;   // 自己到自己的权重为0
                rank[i] = 0;     // 初始秩为0
            }
        }

        /// <summary>
        /// 查找元素x所在集合的根节点，并进行路径压缩
        /// 同时更新权重数组，维护元素到根节点的正确权重
        /// </summary>
        /// <param name="x">要查找的元素</param>
        /// <returns>元素x所在集合的根节点</returns>
        public int Find(int x)
        {
            if (parent[x] != x)
            {
                // 递归查找父节点的根
                int root = Find(parent[x]);
                // 更新权重：x到根的权重 = x到父节点的权重 + 父节点到根的权重
                weight[x] += weight[parent[x]];
                // 路径压缩：直接将x的父节点指向根
                parent[x] = root;
            }
            return parent[x];
        }

        /// <summary>
        /// 合并两个元素所在的集合，并维护权重关系
        /// 假设权重关系为：weight[y] = weight[x] + value
        /// 即 y 的权重 = x 的权重 + value
        /// </summary>
        /// <param name="x">第一个元素</param>
        /// <param name="y">第二个元素</param>
        /// <param name="value">x和y之间的权重关系值</param>
        /// <returns>如果两个元素原本不在同一集合则返回true，否则返回false</returns>
        public bool Union(int x, int y, int value)
        {
            // 查找两个元素的根节点
            int rootX = Find(x);
            int rootY = Find(y);

            // 如果已经在同一集合，检查权重关系是否一致
            if (rootX == rootY)
            {
                // 验证权重关系：weight[y] 应该等于 weight[x] + value
                // 如果不等，说明存在矛盾
                return weight[y] == weight[x] + value;
            }

            // 按秩合并：将秩较小的树合并到秩较大的树下
            if (rank[rootX] < rank[rootY])
            {
                // 将rootX合并到rootY下
                parent[rootX] = rootY;
                // 更新rootX的权重：
                // weight[rootX] = weight[x] + value - weight[y]
                // 推导：weight[y] = weight[x] + value + weight[rootX]
                weight[rootX] = weight[x] + value - weight[y];
            }
            else
            {
                // 将rootY合并到rootX下
                parent[rootY] = rootX;
                // 更新rootY的权重：
                // weight[rootY] = weight[y] - value - weight[x]
                // 推导：weight[y] = weight[x] + value + weight[rootY]
                weight[rootY] = weight[y] - value - weight[x];

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

        /// <summary>
        /// 获取元素x到根节点的权重
        /// </summary>
        /// <param name="x">元素</param>
        /// <returns>元素x到根节点的权重</returns>
        public int GetWeight(int x)
        {
            // 先执行Find确保路径压缩和权重更新
            Find(x);
            return weight[x];
        }

        /// <summary>
        /// 获取两个元素之间的相对权重
        /// 即 weight[y] - weight[x]
        /// </summary>
        /// <param name="x">第一个元素</param>
        /// <param name="y">第二个元素</param>
        /// <returns>y相对于x的权重差，如果不在同一集合返回null</returns>
        public int? GetRelativeWeight(int x, int y)
        {
            if (!IsConnected(x, y))
                return null;

            Find(x);
            Find(y);
            return weight[y] - weight[x];
        }

        /// <summary>
        /// 判断两个元素是否在同一集合中
        /// </summary>
        /// <param name="x">第一个元素</param>
        /// <param name="y">第二个元素</param>
        /// <returns>如果在同一集合返回true，否则返回false</returns>
        public bool IsConnected(int x, int y)
        {
            return Find(x) == Find(y);
        }
    }
}