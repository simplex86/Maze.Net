using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫场地接口（邻接表方案）
    /// </summary>
    public interface IMazeField
    {
        /// <summary>
        /// 顶点总数
        /// </summary>
        int count { get; }

        /// <summary>
        /// 邻接表（图）
        /// </summary>
        List<List<Edge>> graph { get; }

        /// <summary>
        /// 根据生成树边集移除边界
        /// </summary>
        void RemoveBorders(List<(int, int)> spanningTree);

        /// <summary>
        /// 坐标范围，描述迷宫所有边界的几何包围盒
        /// </summary>
        CoordinateBounds Bounds { get; }

        /// <summary>
        /// Y轴是否需要翻转（true表示场地的Y轴朝上，渲染时需翻转为屏幕Y朝下）
        /// </summary>
        bool FlipY { get; }
    }

    /// <summary>
    /// 迷宫场地基类
    /// </summary>
    public class MazeField : IMazeField
    {
        /// <summary>
        /// 顶点总数
        /// </summary>
        public int count { get; protected set; }

        /// <summary>
        /// 邻接表（图）
        /// </summary>
        public List<List<Edge>> graph { get; protected set; }

        private CoordinateBounds? _bounds;

        /// <summary>
        /// 坐标范围（延迟计算）
        /// </summary>
        public CoordinateBounds Bounds
        {
            get
            {
                if (!_bounds.HasValue)
                    _bounds = ComputeBounds();
                return _bounds.Value;
            }
        }

        /// <summary>
        /// Y轴是否需要翻转（默认false，Y朝下的坐标系）
        /// </summary>
        public virtual bool FlipY => false;

        /// <summary>
        /// 根据生成树边集移除边界
        /// </summary>
        public void RemoveBorders(List<(int, int)> spanningTree)
        {
            foreach (var (u, v) in spanningTree)
            {
                for (int i = 0; i < graph[u].Count; i++)
                {
                    if (graph[u][i].Neighbor == v)
                    {
                        graph[u][i].Border = null;
                        break;
                    }
                }
                for (int i = 0; i < graph[v].Count; i++)
                {
                    if (graph[v][i].Neighbor == u)
                    {
                        graph[v][i].Border = null;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 遍历邻接表中所有边界，计算几何包围盒
        /// </summary>
        private CoordinateBounds ComputeBounds()
        {
            double minX = double.MaxValue, minY = double.MaxValue;
            double maxX = double.MinValue, maxY = double.MinValue;

            foreach (var edges in graph)
            {
                foreach (var edge in edges)
                {
                    if (edge.Border is LineBorder line)
                    {
                        minX = Math.Min(minX, Math.Min(line.X1, line.X2));
                        maxX = Math.Max(maxX, Math.Max(line.X1, line.X2));
                        minY = Math.Min(minY, Math.Min(line.Y1, line.Y2));
                        maxY = Math.Max(maxY, Math.Max(line.Y1, line.Y2));
                    }
                    else if (edge.Border is ArcBorder arc)
                    {
                        minX = Math.Min(minX, arc.CenterX - arc.Radius);
                        maxX = Math.Max(maxX, arc.CenterX + arc.Radius);
                        minY = Math.Min(minY, arc.CenterY - arc.Radius);
                        maxY = Math.Max(maxY, arc.CenterY + arc.Radius);
                    }
                }
            }

            return new CoordinateBounds(minX, minY, maxX, maxY);
        }
    }
}
