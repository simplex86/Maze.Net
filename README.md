# 随机迷宫生成器

随机迷宫生成器包括三个模块

- [迷宫生成器](#迷宫生成器)
- [出入口生成器](#出入口生成器)
- [解法生成器](#解法生成器)

## 迷宫生成器

提供多种形状的随机迷宫生成器

- [矩形迷宫生成器](./doc/retangular/retangular_maze.md)
- [圆形迷宫生成器](./doc/circular/circular_maze.md)
- [蜂窝迷宫生成器](./doc/honeycomb/honeycomb_maze.md)
- [三角形迷宫生成器](./doc/triangular/triangular_maze.md)
- [六边形迷宫生成器](./doc/hexagonal/hexagonal_maze.md)
- [圆三角格迷宫生成器](./doc/circularhexagon/circularhexagon_maze.md)

![](./doc/images/maze.png)

每种形状都有多种算法

| 算法 | 随机偏差 | 死胡同特征 | 速度 | 空间 |
|------|---------|-----------|-----|------|
| **DFS** | 强（方向偏好）| 少而深 | ⚡快 | 低 |
| **BFS** | 轻（径向偏好）| 多而浅 | ⚡快 | 中 |
| **Prim** | 轻（扩散偏好）| 多而浅 | ⚡快 | 较高 |
| **Kruskal** | 无 | 均匀 | ⚡快 |	较高 |
| **Wilson** | 无（均匀）| 均匀 | 🐢不可预测 | 中 |
| **Eller** | 强（水平偏好）| 中等 | ⚡快 | 极低 | 
| **Aldous-Broder** | 无（均匀）| 均匀 | 🐢🐢极慢 | 低 |

## 出入口生成器

为各种形状的迷宫生成随机出入口

- [矩形迷宫出入口生成器](./doc/retangular/retangular_gate.md)
- [圆形迷宫出入口生成器](./doc/circular/circular_gate.md)
- [蜂窝迷宫出入口生成器](./doc/honeycomb/honeycomb_gate.md)
- [三角形迷宫出入口生成器](./doc/triangular/triangular_gate.md)
- [六边形迷宫出入口生成器](./doc/hexagonal/hexagonal_gate.md)
- [圆三角格迷宫出入口生成器](./doc/circularhexagon/circularhexagon_gate.md)

![](./doc/images/gate.png)

> [!NOTE]  
> 无论哪种形状的迷宫，出入口都只会出现在处于迷宫边缘的格子上。但不同形状的迷宫又略微有一些差异
> | 迷宫形状 | 出入口位置 |
> |---------|-----------|
> | 矩形迷宫 | 矩形的对边 |
> | 圆形迷宫 | 同直径的两端 |
> | 蜂窝迷宫 | 蜂窝（六边形）的对边 |
> | 三角形迷宫 | 无论正三角还是倒三角，出口都在顶点，入口随机出现在底边 |
> | 六边形迷宫 | 六边形的对边 |
> | 圆三角格迷宫 | 同直径的两端 |

## 解法生成器

在各种形状的迷宫的出入口之间，计算出有效路径

``` csharp
public class MazeSolutionGenerator
{
    // 根据迷宫和出入口数据，计算迷宫解法
    public MazeSolution Generate(MazeField field, MazeGate gate);
}
```

### 示例

``` csharp
var generator = new MazeSolutionGenerator();
var solution = generator.Generate(field, gate);
```

![](./doc/images/solution.png)