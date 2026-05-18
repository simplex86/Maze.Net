# 随机迷宫和地牢生成器

基于 .Net 纯 C# 实现的随机迷宫和地牢生成器库

- [迷宫生成器](./doc/maze.md)

    ``` csharp
    var maze = new RectangleMaze();
    maze.Create(width, height, RectangleMazeAlgorithm.Kruskal);
    ```

    ![地牢](./doc/imgs/maze.png)

- [地牢生成器](./doc/dungeon.md)

    ``` csharp
    var dungeon = new RectangleDungeon();
    dungeon.Create(width,
                   height,
                   roomMinWidth,
                   roomMaxWidth,
                   roomMinHeight,
                   roomMaxHeight,
                   roomCount,
                   mulconnector,
                   tortuosity,
                   RectangleDungeonAlgorithm.Nystroms);
    ```

    ![地牢](./doc/imgs/dungeon.png)

> [TIP]  
> 详细用法和示例，请见 [测试工程](https://github.com/simplex86/Maze.Net-Test)