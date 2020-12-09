using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using Common;

namespace _01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var filePath = FileSystem.GetSolutionPath() + @"\01\input.txt";

            // Traditional
            //var content = File.ReadAllLines(filePath).Select(l => int.Parse(l)).ToList();

            // Method groups in linq
            var content = File.ReadAllLines(filePath).Select(int.Parse).ToList();

            var result1 = SolveA(content);
            Console.WriteLine($"Result 01a: " + result1);

            var result2 = SolveB(content);
            Console.WriteLine($"Result 01b: " + result2);
        }

        private static int SolveA(List<int> content)
        {
            const int target = 2020;
            for (int i = 0; i < content.Count; i++)
            {
                var a = content[i];
                for (int j = i + 1; j < content.Count; j++)
                {
                    var b = content[j];
                    if (a + b == target) return a * b;
                }
            }

            throw new ArgumentException();
        }

        private static int SolveB(List<int> content)
        {
            const int target = 2020;
            for (int i = 0; i < content.Count - 2; i++)
            {
                var a = content[i];
                for (int j = i + 1; j < content.Count - 1; j++)
                {
                    var b = content[j];
                    for (int k = i + 2; k < content.Count; k++)
                    {
                        var c = content[k];
                        if (a + b + c == target) return a * b * c;
                    }
                }
            }

            throw new ArgumentException();
        }
    }
}
