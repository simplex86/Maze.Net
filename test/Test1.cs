using System;
using SimpleX.Maze;

class Test1
{
    public void Run()
    {
        var field = MazeProvider.CreateRectangleMaze(15, 15);
        Show(field);
    }

    private void Show(RectangleMazeField field)
    {
        var text = "";
        for (int y = 0; y < field.height; y++)
        {
            for (int x = 0; x < field.width; x++)
            {
                var t = field[x, y];
                if (t == TileType.Path)
                {
                    text += " ";
                }
                else
                {
                    text += "*";
                }
            }
            text += "\n";
        }

        Console.Write(text);
    }
}
