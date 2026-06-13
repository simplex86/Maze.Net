using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexLab.Maze
{
    /// <summary>
    /// 迷宫质量评估器，基于图论度量计算8项指标得分
    /// </summary>
    public class MazeScoreEvaluator
    {
        /// <summary>
        /// 评估迷宫质量
        /// </summary>
        public static MazeScore Evaluate(MazeField field, MazeGate gate, MazeSolution solution)
        {
            var score = new MazeScore();

            // 步骤1: 可解性检查
            if (gate.Entrance < 0 || gate.Exit < 0 || solution.Count == 0)
            {
                score.IsSolvable = false;
                score.TotalScore = 0;
                score.Difficulty = MazeDifficulty.NeedsOptimization;
                return score;
            }

            score.IsSolvable = true;
            var graph = field.Graph;
            int vertexCount = field.VertexCount;
            int shortestPathLength = solution.Count - 1;

            // 起点与终点相同，退化为0分
            if (shortestPathLength == 0)
            {
                score.TotalScore = 0;
                score.Difficulty = MazeDifficulty.NeedsOptimization;
                return score;
            }

            // 将解路径缓存为列表和集合，避免多次枚举
            var pathList = new List<int>(solution);
            var pathSet = new HashSet<int>(pathList);

            // 步骤2: 计算基础图论量
            int graphTheoreticDistance = ComputeGraphTheoreticDistance(graph, gate.Entrance, gate.Exit, vertexCount);
            int graphDiameter = ComputeGraphDiameter(graph, vertexCount);
            var (branchPoints, deadEnds) = ClassifyVertices(graph, gate, vertexCount);
            var deadEndLengths = ComputeDeadEndLengths(graph, deadEnds, branchPoints, gate);
            double avgDeadEndLength = deadEndLengths.Count > 0 ? deadEndLengths.Average() : 0;
            double avgBFSDepth = ComputeAverageBFSDepth(graph, gate.Entrance, vertexCount);
            int branchPointsOnPath = CountBranchPointsOnPath(pathList, branchPoints);
            double detourLength = ComputeDetourLength(graph, pathList, pathSet, branchPoints);

            // 死路长度变异系数
            double deadEndCV = 0;
            if (deadEndLengths.Count > 1 && avgDeadEndLength > 0)
            {
                double sumSqDiff = deadEndLengths.Sum(l => (l - avgDeadEndLength) * (l - avgDeadEndLength));
                double variance = sumSqDiff / deadEndLengths.Count;
                deadEndCV = Math.Sqrt(variance) / avgDeadEndLength;
            }

            // 步骤3: 计算各指标原始比值
            score.PathEfficiencyRaw = graphTheoreticDistance > 0
                ? (double)shortestPathLength / graphTheoreticDistance : 1.0;
            score.StructuralComplexityRaw = vertexCount > 0
                ? (double)(branchPoints.Count + deadEnds.Count) / vertexCount : 0;
            score.ExplorationDepthRaw = graphDiameter > 0
                ? avgBFSDepth / graphDiameter : 0;
            score.DecisionDensityRaw = branchPointsOnPath > 0
                ? (double)shortestPathLength / branchPointsOnPath : double.MaxValue;
            score.DeadEndReasonabilityRaw = graphDiameter > 0
                ? avgDeadEndLength / graphDiameter : 0;
            // 解的隐蔽性：最短路径上分支点的岔路子树总边数 / 最短路径长度
            score.SolutionConcealmentRaw = shortestPathLength > 0
                ? detourLength / shortestPathLength : 0;
            // 岔路均衡度：分支点数 / (分支点数 + 死路数)
            int branchPlusDead = branchPoints.Count + deadEnds.Count;
            score.BranchBalanceRaw = branchPlusDead > 0
                ? (double)branchPoints.Count / branchPlusDead : 0;
            // 死路多样性：变异系数
            score.DeadEndDiversityRaw = deadEndCV;

            // 步骤4: 代入评分函数
            score.PathEfficiencyScore = ScorePathEfficiency(score.PathEfficiencyRaw);
            score.StructuralComplexityScore = ScoreStructuralComplexity(score.StructuralComplexityRaw);
            score.ExplorationDepthScore = ScoreExplorationDepth(score.ExplorationDepthRaw);
            score.DecisionDensityScore = ScoreDecisionDensity(score.DecisionDensityRaw);
            score.DeadEndReasonabilityScore = ScoreDeadEndReasonability(score.DeadEndReasonabilityRaw);
            score.SolutionConcealmentScore = ScoreSolutionConcealment(score.SolutionConcealmentRaw);
            score.BranchBalanceScore = ScoreBranchBalance(score.BranchBalanceRaw);
            score.DeadEndDiversityScore = ScoreDeadEndDiversity(score.DeadEndDiversityRaw);

            // 步骤5: 按权重计算总分
            score.TotalScore =
                  score.PathEfficiencyScore * 2.0
                + score.StructuralComplexityScore * 1.5
                + score.ExplorationDepthScore * 1.5
                + score.DecisionDensityScore * 1.5
                + score.DeadEndReasonabilityScore * 1.0
                + score.SolutionConcealmentScore * 1.0
                + score.BranchBalanceScore * 1.0
                + score.DeadEndDiversityScore * 0.5;

            // 步骤6: 确定难度等级
            score.Difficulty = ClassifyDifficulty(score.TotalScore);

            return score;
        }

        /// <summary>
        /// 异步评估迷宫质量
        /// </summary>
        public static async Task<MazeScore> EvaluateAsync(MazeField field, MazeGate gate, MazeSolution solution)
        {
            return await Task.Run(() => Evaluate(field, gate, solution));
        }

        #region 图论基础量计算

        /// <summary>
        /// 计算图论最短距离（移除所有墙壁后的BFS距离）
        /// </summary>
        private static int ComputeGraphTheoreticDistance(List<List<Adjacency>> graph, int entrance, int exit, int vertexCount)
        {
            var distance = new int[vertexCount];
            for (int i = 0; i < vertexCount; i++)
                distance[i] = -1;

            distance[entrance] = 0;
            var queue = new Queue<int>();
            queue.Enqueue(entrance);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                if (v == exit)
                    break;

                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor < 0 || distance[edge.Neighbor] >= 0)
                        continue;
                    // 忽略 IsOpen，视为所有墙壁已移除
                    distance[edge.Neighbor] = distance[v] + 1;
                    queue.Enqueue(edge.Neighbor);
                }
            }

            return distance[exit] >= 0 ? distance[exit] : 0;
        }

        /// <summary>
        /// 计算图直径（仅考虑开放边，使用2次BFS法）
        /// </summary>
        private static int ComputeGraphDiameter(List<List<Adjacency>> graph, int vertexCount)
        {
            if (vertexCount <= 1)
                return 0;

            // 第一次BFS：从顶点0出发找最远顶点A
            int farthest = BFSToFarthest(graph, 0, vertexCount, out _);
            // 第二次BFS：从A出发找最远顶点B，距离即为直径
            BFSToFarthest(graph, farthest, vertexCount, out int diameter);

            return diameter;
        }

        /// <summary>
        /// 从指定顶点做BFS，返回最远顶点及其距离（仅沿开放边）
        /// </summary>
        private static int BFSToFarthest(List<List<Adjacency>> graph, int start, int vertexCount, out int maxDistance)
        {
            var distance = new int[vertexCount];
            for (int i = 0; i < vertexCount; i++)
                distance[i] = -1;

            distance[start] = 0;
            var queue = new Queue<int>();
            queue.Enqueue(start);

            int farthest = start;
            maxDistance = 0;

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();

                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor < 0 || !edge.IsOpen || distance[edge.Neighbor] >= 0)
                        continue;

                    distance[edge.Neighbor] = distance[v] + 1;
                    queue.Enqueue(edge.Neighbor);

                    if (distance[edge.Neighbor] > maxDistance)
                    {
                        maxDistance = distance[edge.Neighbor];
                        farthest = edge.Neighbor;
                    }
                }
            }

            return farthest;
        }

        /// <summary>
        /// 分类顶点：分支点（开放邻居≥3）和死路（开放邻居=1，排除入口/出口）
        /// </summary>
        private static (HashSet<int> branchPoints, HashSet<int> deadEnds) ClassifyVertices(
            List<List<Adjacency>> graph, MazeGate gate, int vertexCount)
        {
            var branchPoints = new HashSet<int>();
            var deadEnds = new HashSet<int>();

            for (int v = 0; v < vertexCount; v++)
            {
                int openNeighbors = 0;
                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor >= 0 && edge.IsOpen)
                        openNeighbors++;
                }

                if (openNeighbors >= 3)
                    branchPoints.Add(v);
                else if (openNeighbors == 1 && v != gate.Entrance && v != gate.Exit)
                    deadEnds.Add(v);
            }

            return (branchPoints, deadEnds);
        }

        /// <summary>
        /// 计算每条死路的长度（从死路端点到最近分支点/入口/出口的边数）
        /// </summary>
        private static List<double> ComputeDeadEndLengths(
            List<List<Adjacency>> graph, HashSet<int> deadEnds, HashSet<int> branchPoints, MazeGate gate)
        {
            var stopSet = new HashSet<int>(branchPoints);
            stopSet.Add(gate.Entrance);
            stopSet.Add(gate.Exit);

            var lengths = new List<double>();

            foreach (int deadEnd in deadEnds)
            {
                int length = 0;
                int current = deadEnd;
                int prev = -1;

                while (true)
                {
                    int next = -1;
                    foreach (var edge in graph[current])
                    {
                        if (edge.Neighbor >= 0 && edge.IsOpen && edge.Neighbor != prev)
                        {
                            next = edge.Neighbor;
                            break;
                        }
                    }

                    if (next == -1)
                        break;

                    length++;

                    if (stopSet.Contains(next))
                        break;

                    prev = current;
                    current = next;
                }

                lengths.Add(length);
            }

            return lengths;
        }

        /// <summary>
        /// 计算从入口出发的平均BFS深度（仅沿开放边）
        /// </summary>
        private static double ComputeAverageBFSDepth(List<List<Adjacency>> graph, int entrance, int vertexCount)
        {
            var distance = new int[vertexCount];
            for (int i = 0; i < vertexCount; i++)
                distance[i] = -1;

            distance[entrance] = 0;
            var queue = new Queue<int>();
            queue.Enqueue(entrance);

            long totalDepth = 0;
            int reachableCount = 0;

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                totalDepth += distance[v];
                reachableCount++;

                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor < 0 || !edge.IsOpen || distance[edge.Neighbor] >= 0)
                        continue;

                    distance[edge.Neighbor] = distance[v] + 1;
                    queue.Enqueue(edge.Neighbor);
                }
            }

            return reachableCount > 0 ? (double)totalDepth / reachableCount : 0;
        }

        /// <summary>
        /// 统计最短路径上的分支点数
        /// </summary>
        private static int CountBranchPointsOnPath(List<int> pathList, HashSet<int> branchPoints)
        {
            int count = 0;
            foreach (int v in pathList)
            {
                if (branchPoints.Contains(v))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 计算最短路径岔路总长度：最短路径上每个分支点引出的非路径方向子树的边数总和
        /// </summary>
        private static double ComputeDetourLength(
            List<List<Adjacency>> graph, List<int> pathList, HashSet<int> pathSet, HashSet<int> branchPoints)
        {
            double totalDetour = 0;

            for (int i = 0; i < pathList.Count; i++)
            {
                int v = pathList[i];
                if (!branchPoints.Contains(v))
                    continue;

                // 确定路径上的前后邻居
                var pathNeighbors = new HashSet<int>();
                if (i > 0) pathNeighbors.Add(pathList[i - 1]);
                if (i < pathList.Count - 1) pathNeighbors.Add(pathList[i + 1]);

                // 对每个非路径方向的开放邻居，计算子树边数
                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor < 0 || !edge.IsOpen)
                        continue;
                    if (pathNeighbors.Contains(edge.Neighbor))
                        continue;

                    totalDetour += CountSubtreeEdges(graph, edge.Neighbor, v);
                }
            }

            return totalDetour;
        }

        /// <summary>
        /// 计算从start出发（不经过parent）的子树边数
        /// </summary>
        private static int CountSubtreeEdges(List<List<Adjacency>> graph, int start, int parent)
        {
            int count = 0;
            var stack = new Stack<(int vertex, int par)>();
            stack.Push((start, parent));

            while (stack.Count > 0)
            {
                var (vertex, par) = stack.Pop();

                foreach (var edge in graph[vertex])
                {
                    if (edge.Neighbor < 0 || !edge.IsOpen || edge.Neighbor == par)
                        continue;

                    count++;
                    stack.Push((edge.Neighbor, vertex));
                }
            }

            return count;
        }

        #endregion

        #region 评分函数

        /// <summary>
        /// 路径效率评分：ratio = 最短路径长度 / 图论最短距离
        /// 使用 10/sqrt(ratio)，比 10/ratio 更温和
        /// ratio=1.0→10, ratio=1.5→8.2, ratio=2.0→7.1, ratio=3.0→5.8, ratio=4.0→5.0
        /// </summary>
        private static double ScorePathEfficiency(double ratio)
        {
            if (ratio < 1.0) ratio = 1.0;
            return Clamp(10.0 / Math.Sqrt(ratio), 0, 10);
        }

        /// <summary>
        /// 结构复杂度评分：ratio = (分支点+死路)/顶点数，0.4-0.6最佳
        /// score = clamp(10 - 40 * (ratio - 0.5)², 0, 10)
        /// </summary>
        private static double ScoreStructuralComplexity(double ratio)
        {
            return Clamp(10 - 40 * (ratio - 0.5) * (ratio - 0.5), 0, 10);
        }

        /// <summary>
        /// 探索深度评分：ratio = 平均BFS深度/图直径，0.35-0.45最佳
        /// ratio ≤ 0.4: score = 10 * ratio / 0.4
        /// ratio > 0.4: score = 10 - 10 * (ratio - 0.4)
        /// </summary>
        private static double ScoreExplorationDepth(double ratio)
        {
            if (ratio <= 0.4)
                return Clamp(10 * ratio / 0.4, 0, 10);
            else
                return Clamp(10 - 10 * (ratio - 0.4), 0, 10);
        }

        /// <summary>
        /// 决策密度评分：steps = 最短路径长度/路径上分支点数，5-8步最佳
        /// score = clamp(10 - 0.18 * (steps - 6.5)², 0, 10)
        /// </summary>
        private static double ScoreDecisionDensity(double steps)
        {
            if (double.IsInfinity(steps) || steps <= 0)
                return 0;
            return Clamp(10 - 0.18 * (steps - 6.5) * (steps - 6.5), 0, 10);
        }

        /// <summary>
        /// 死路合理性评分：ratio = 死路平均长度/图直径，0.2-0.4最佳
        /// score = clamp(10 - 80 * (ratio - 0.3)², 0, 10)
        /// </summary>
        private static double ScoreDeadEndReasonability(double ratio)
        {
            return Clamp(10 - 80 * (ratio - 0.3) * (ratio - 0.3), 0, 10);
        }

        /// <summary>
        /// 解的隐蔽性评分：ratio = 最短路径岔路子树总边数 / 最短路径长度
        /// 使用对数评分：score = 10 * log(1 + ratio) / log(4)
        /// ratio=0→0, ratio=0.5→3.7, ratio=1→5.0, ratio=2→6.3, ratio=3→10
        /// </summary>
        private static double ScoreSolutionConcealment(double ratio)
        {
            if (ratio <= 0)
                return 0;
            return Clamp(10.0 * Math.Log(1 + ratio) / Math.Log(4), 0, 10);
        }

        /// <summary>
        /// 岔路均衡度评分：ratio = 分支点数/(分支点数+死路数)，0.25-0.45最佳
        /// score = clamp(10 - 40 * (ratio - 0.35)², 0, 10)
        /// </summary>
        private static double ScoreBranchBalance(double ratio)
        {
            return Clamp(10 - 40 * (ratio - 0.35) * (ratio - 0.35), 0, 10);
        }

        /// <summary>
        /// 死路多样性评分：cv = 死路长度的变异系数，0.3-0.7最佳
        /// score = clamp(10 - 25 * (cv - 0.5)², 0, 10)
        /// cv=0.5→10, cv=0.3或0.7→9.0, cv=0.0或1.0→6.25
        /// </summary>
        private static double ScoreDeadEndDiversity(double cv)
        {
            return Clamp(10 - 25 * (cv - 0.5) * (cv - 0.5), 0, 10);
        }

        private static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        #endregion

        #region 难度分级

        private static MazeDifficulty ClassifyDifficulty(double totalScore)
        {
            if (totalScore >= 90) return MazeDifficulty.Expert;
            if (totalScore >= 80) return MazeDifficulty.Hard;
            if (totalScore >= 70) return MazeDifficulty.Medium;
            if (totalScore >= 60) return MazeDifficulty.Easy;
            return MazeDifficulty.NeedsOptimization;
        }

        #endregion
    }
}
