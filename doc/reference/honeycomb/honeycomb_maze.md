# 蜂窝迷宫生成器

``` csharp
public class HoneycombMazeGenerator
{
    // 同步函数
    public HoneycombMazeField Generate(int size, 
                                       EMazeAlgorithm algorithm = EMazeAlgorithm.Prim);
    // 异步函数
    public async Task<HoneycombMazeField> GenerateAsync(int size, 
                                                        EMazeAlgorithm algorithm = EMazeAlgorithm.Prim);
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
    - Eller
    - Aldous-Broder
    - Hunt and Kill

## 示例

``` csharp
var generator = new HoneycombMazeGenerator();
generator.Generate(11, MazeAlgorithm.Kruskal);
```

![](./images/maze.png)