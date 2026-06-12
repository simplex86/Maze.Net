using System;
using System.Collections.Generic;
using System.IO;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫场地基类
    /// </summary>
    public class MazeField
    {
        /// <summary>
        /// 迷宫的形状
        /// </summary>
        public EMazeShape Shape { get; internal protected set; }

        /// <summary>
        /// 顶点总数
        /// </summary>
        public int VertexCount { get; internal protected set; }

        /// <summary>
        /// 邻接表（图）
        /// </summary>
        internal List<List<Adjacency>> Graph { get; set; }

        private CoordinateBounds? bounds;

        /// <summary>
        /// 坐标范围（延迟计算）
        /// </summary>
        public CoordinateBounds Bounds
        {
            get
            {
                if (!bounds.HasValue) bounds = ComputeBounds();
                return bounds.Value;
            }
        }

        /// <summary>
        /// Y轴是否需要翻转（默认false，Y朝下的坐标系）
        /// </summary>
        public virtual bool FlipY => false;

        protected MazeField()
        {

        }

        /// <summary>
        /// 获取指定顶点对应格子的几何形状，用于渲染出入口标记。
        /// 子类应覆写此方法以提供特定形状的格子几何信息。
        /// </summary>
        /// <param name="vertex">顶点索引</param>
        /// <returns>格子的几何形状</returns>
        internal virtual CellShape GetCellShape(int vertex)
        {
            return CellShape.Polygon(new Vertex[0]);
        }

        /// <summary>
        /// 遍历邻接表中所有边界，计算几何包围盒
        /// </summary>
        private CoordinateBounds ComputeBounds()
        {
            double minx = double.MaxValue, miny = double.MaxValue;
            double maxx = double.MinValue, maxy = double.MinValue;

            foreach (var edges in Graph)
            {
                foreach (var edge in edges)
                {
                    if (edge.Border is LineBorder line)
                    {
                        minx = Math.Min(minx, Math.Min(line.X1, line.X2));
                        maxx = Math.Max(maxx, Math.Max(line.X1, line.X2));
                        miny = Math.Min(miny, Math.Min(line.Y1, line.Y2));
                        maxy = Math.Max(maxy, Math.Max(line.Y1, line.Y2));
                    }
                    else if (edge.Border is ArcBorder arc)
                    {
                        minx = Math.Min(minx, arc.CenterX - arc.Radius);
                        maxx = Math.Max(maxx, arc.CenterX + arc.Radius);
                        miny = Math.Min(miny, arc.CenterY - arc.Radius);
                        maxy = Math.Max(maxy, arc.CenterY + arc.Radius);
                    }
                }
            }

            return new CoordinateBounds(minx, miny, maxx, maxy);
        }
    }
}
