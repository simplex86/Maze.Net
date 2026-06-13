using System;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫质量评分结果
    /// </summary>
    public struct MazeScore
    {
        /// <summary>
        /// 迷宫是否可解
        /// </summary>
        public bool IsSolvable;

        /// <summary>
        /// 路径效率原始值：最短路径长度 / 图论最短距离
        /// </summary>
        public double PathEfficiencyRaw;

        /// <summary>
        /// 结构复杂度原始值：(分支点数 + 死路数) / 顶点总数
        /// </summary>
        public double StructuralComplexityRaw;

        /// <summary>
        /// 探索深度原始值：平均BFS深度 / 图直径
        /// </summary>
        public double ExplorationDepthRaw;

        /// <summary>
        /// 决策密度原始值：最短路径长度 / 最短路径上的分支点数（每多少步一个决策点）
        /// </summary>
        public double DecisionDensityRaw;

        /// <summary>
        /// 死路合理性原始值：死路平均长度 / 图直径
        /// </summary>
        public double DeadEndReasonabilityRaw;

        /// <summary>
        /// 解的隐蔽性原始值：最短路径上分支点的岔路子树总边数 / 最短路径长度
        /// </summary>
        public double SolutionConcealmentRaw;

        /// <summary>
        /// 岔路均衡度原始值：分支点数 / (分支点数 + 死路数)
        /// </summary>
        public double BranchBalanceRaw;

        /// <summary>
        /// 死路多样性原始值：死路长度的变异系数（标准差/均值）
        /// </summary>
        public double DeadEndDiversityRaw;

        /// <summary>
        /// 路径效率得分 (0-10)
        /// </summary>
        public double PathEfficiencyScore;

        /// <summary>
        /// 结构复杂度得分 (0-10)
        /// </summary>
        public double StructuralComplexityScore;

        /// <summary>
        /// 探索深度得分 (0-10)
        /// </summary>
        public double ExplorationDepthScore;

        /// <summary>
        /// 决策密度得分 (0-10)
        /// </summary>
        public double DecisionDensityScore;

        /// <summary>
        /// 死路合理性得分 (0-10)
        /// </summary>
        public double DeadEndReasonabilityScore;

        /// <summary>
        /// 解的隐蔽性得分 (0-10)
        /// </summary>
        public double SolutionConcealmentScore;

        /// <summary>
        /// 岔路均衡度得分 (0-10)
        /// </summary>
        public double BranchBalanceScore;

        /// <summary>
        /// 死路多样性得分 (0-10)
        /// </summary>
        public double DeadEndDiversityScore;

        /// <summary>
        /// 总分 (0-100)
        /// </summary>
        public double TotalScore;

        /// <summary>
        /// 难度等级
        /// </summary>
        public MazeDifficulty Difficulty;
    }

    /// <summary>
    /// 迷宫难度等级
    /// </summary>
    public enum MazeDifficulty
    {
        /// <summary>
        /// 需优化 (&lt;60分)
        /// </summary>
        NeedsOptimization = 0,

        /// <summary>
        /// 简单 (60-69分)
        /// </summary>
        Easy = 1,

        /// <summary>
        /// 中等 (70-79分)
        /// </summary>
        Medium = 2,

        /// <summary>
        /// 困难 (80-89分)
        /// </summary>
        Hard = 3,

        /// <summary>
        /// 专家级 (90-100分)
        /// </summary>
        Expert = 4,
    }
}
