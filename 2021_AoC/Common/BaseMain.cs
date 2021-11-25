using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    class BaseMain
    {
        static void Main(string[] args)
        {
            const string fileName = "04";
            var content = FileSystem.GetInputData(fileName);

            var result1 = SolveA(content);
            Console.WriteLine($"Result a: " + result1);

            var result2 = SolveB(content);
            Console.WriteLine($"Result b: " + result2);
            Console.Read();
        }

        private static int SolveA(List<string> content)
        {
            return 0;
        }

        private static int SolveB(List<string> content)
        {
            return 0;
        }
    }
}
