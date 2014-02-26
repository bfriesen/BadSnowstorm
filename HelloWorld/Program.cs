using BadSnowstorm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureConsole();

            Application.Run(new ApplicationContext<HelloWorldController>());
        }

        private static void ConfigureConsole()
        {
            Console.Title = "Hello, world!";

            Console.CursorVisible = false;

            Console.OutputEncoding = Encoding.GetEncoding(1252);
            Console.SetWindowSize(99, 31);
            Console.BufferWidth = 99;
            Console.BufferHeight = 31;
        }
    }
}
