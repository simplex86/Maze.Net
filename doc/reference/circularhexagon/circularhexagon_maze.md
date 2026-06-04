# 圆三角格迷宫生成器

``` csharp
public class CircularHexagonMazeGenerator
{
    // 同步函数
    public CircularHexagonMazeField Generate(int size, 
                                             MazeAlgorithm algorithm = MazeAlgorithm.Prim);
    // 异步函数
    public await Task<CircularHexagonMazeField> GenerateAsync(int size, 
                                                              MazeAlgorithm algorithm = MazeAlgorithm.Prim);
}
```

## 参数

- **size** 边长
- **algorithm** 生成算法
    - DFS
    - BFS
    - Prim
    - Kruskal
    - Wilson
    - Eller（不支持，会退化为 DFS）
    - Aldous-Broder
    - Hunt and Kill

> [!WARNING]  
> Eller 算法是基于行扫描的，不支持圆三角格。当选择用 Eller 算法生成圆三角格迷宫时，会退化为 DFS 算法。

## 示例

``` csharp
var generator = new CircularHexagonMazeGenerator();
var field = generator.Generate(size, MazeAlgorithm.Kruskal);
```

![](./images/maze.png)