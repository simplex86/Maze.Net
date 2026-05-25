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
        /// 获取格子的顶点索引
        /// </summary>
        int GetTileIndex(Tile tile);

        /// <summary>
        /// 根据顶点索引获取格子
        /// </summary>
        Tile GetTileByIndex(int index);

        /// <summary>
        /// 邻接表（图）
        /// </summary>
        List<List<Edge>> graph { get; }

        /// <summary>
        /// 根据生成树边集移除边界
        /// </summary>
        void RemoveBorders(List<(int, int)> spanningTree);

        /// <summary>
        /// 行数
        /// </summary>
        int rows { get; }

        /// <summary>
        /// 获取格子所在行
        /// </summary>
        int GetRow(Tile tile);

        /// <summary>
        /// 获取指定行的所有格子
        /// </summary>
        List<Tile> GetTilesInRow(int row);
    }
}
