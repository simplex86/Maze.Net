# 阶梯形迷宫生成器

``` csharp
public class StairwayMazeGenerator
{
    // 同步函数
    public StairwayMazeField Generate(int steps,
                                      EMazeAlgorithm algorithm = EMazeAlgorithm.Prim);
    // 异步函数
    public async Task<StairwayMazeField> GenerateAsync(int steps,
                                                       EMazeAlgorithm algorithm = EMazeAlgorithm.Prim);
}
```

## 参数

- **steps** 边长
- **algorithm** 生成算法
    - DFS
    - BFS
    - Prim
    - Kruskal
    - Wilson
    - Eller
    - Aldous-Broder
    - Hunt and Kill

## 示例

``` csharp
var generator = new StairwayMazeGenerator();
var field = generator.Generate(length, MazeAlgorithm.Kruskal);
```

![](./images/maze.png)