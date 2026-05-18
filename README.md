# 随机迷宫和地牢生成器

纯C#实现创建随机迷宫和地牢

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