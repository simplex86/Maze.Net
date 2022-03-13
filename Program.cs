using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleX.Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                T2();

                Console.WriteLine("\n任意键刷新迷宫，ESC退出！");

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) break;

                Console.Clear();
            }
        }

        static void T1()
        {
            var test = new Test1();
            test.Run();
        }

        static void T2()
        {
            var test = new Test2();
            test.Run();
        }
    }
}
