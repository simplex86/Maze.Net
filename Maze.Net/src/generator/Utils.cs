using System.Collections.Generic;
using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Fisher-Yates 洗牌算法
        /// </summary>
        public static void Shuffle<T>(this List<T> list, Random random)
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
