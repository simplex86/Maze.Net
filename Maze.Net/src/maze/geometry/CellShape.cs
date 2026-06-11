namespace SimplexLab.Maze
{
    /// <summary>
    /// 格子的几何形状，用于渲染出入口标记。
    /// 支持两种形状：多边形（由顶点列表定义）和环形扇区（由弧线参数定义）。
    /// </summary>
    internal struct CellShape
    {
        /// <summary>
        /// 形状类型
        /// </summary>
        public CellShapeType Type;

        /// <summary>
        /// 多边形顶点（Type == Polygon 时有效）
        /// </summary>
        public Vertex[] Vertices;

        /// <summary>
        /// 环形扇区参数（Type == AnnularSector 时有效）
        /// </summary>
        public AnnularSector Sector;

        /// <summary>
        /// 带弧形边的三角形参数（Type == CurvedTriangle 时有效）
        /// </summary>
        public CurvedTriangle CurvedTriangle;

        /// <summary>
        /// 创建多边形形状
        /// </summary>
        public static CellShape Polygon(Vertex[] vertices)
        {
            return new CellShape { Type = CellShapeType.Polygon, Vertices = vertices };
        }

        /// <summary>
        /// 创建环形扇区形状
        /// </summary>
        public static CellShape AnnularSectorShape(AnnularSector sector)
        {
            return new CellShape { Type = CellShapeType.AnnularSector, Sector = sector };
        }

        /// <summary>
        /// 创建带弧形边的三角形形状
        /// </summary>
        public static CellShape CurvedTriangleShape(CurvedTriangle curvedTriangle)
        {
            return new CellShape { Type = CellShapeType.CurvedTriangle, CurvedTriangle = curvedTriangle };
        }
    }

    /// <summary>
    /// 格子形状类型
    /// </summary>
    public enum CellShapeType
    {
        /// <summary>多边形</summary>
        Polygon,
        /// <summary>环形扇区</summary>
        AnnularSector,
        /// <summary>带弧形边的三角形</summary>
        CurvedTriangle,
    }
}
