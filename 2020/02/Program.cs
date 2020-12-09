using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace _02
{
    class Program
    {
        static void Main(string[] args)
        {

            var filePath = FileSystem.GetSolutionPath() + @"\02\input.txt";
            var content = File.ReadAllLines(filePath).ToList();

            var result1 = SolveA(content);
            Console.WriteLine($"Result a: " + result1);

            var result2 = SolveB(content);
            Console.WriteLine($"Result b: " + result2);
        }

        private static (int, int, char target, string code) Limits(string input)
        {
            // Example:
            // 13-17 s: ssssssssssssgsssj
            var min = "";
            var max = "";
            char target = ' ';

            bool digits = false;
            int index = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    digits = true;
                    min += input[i];
                }
                else if (digits)
                {
                    // Digit stream ended
                    index = i;
                    break;
                }
            }

            for (int i = index + 1; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    digits = true;
                    max += input[i];
                }
                else if (digits && !char.IsWhiteSpace(input[i]))
                {
                    // Digit stream ended
                    target = input[i];
                    index = i;
                    break;
                }
            }

            return (int.Parse(min), int.Parse(max), target, input.Substring(index + 3));
        }

        private static int SolveA(List<string> content)
        {
            var valid = 0;
            foreach (var line in content)
            {
                var limits = Limits(line);
                if(limits.target == ' ') throw new ArgumentException();
                if(limits.code.Length == 0) throw new ArgumentException();

                var count = limits.code.Where(c => c == limits.target).ToList().Count;

                if (count >= limits.Item1 && count <= limits.Item2)
                {
                    valid++;
                }
            }

            return valid;
        }
        private static int SolveB(List<string> content)
        {
            var valid = 0;
            foreach (var line in content)
            {
                var limits = Limits(line);
                var first = false;
                var second = false;
                var code = limits.code;

                if (limits.Item1 <= code.Length && code[limits.Item1 - 1] == limits.target)
                {
                    first = true;
                }

                if (limits.Item2 <= code.Length && code[limits.Item2 - 1] == limits.target)
                {
                    second = true;
                }

                if ((first && !second) || (!first && second))
                {
                    valid++;
                }
            }

            return valid;
        }
    }
}
