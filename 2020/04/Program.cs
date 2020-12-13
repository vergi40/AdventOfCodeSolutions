using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace _04
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "04";
            var content = FileSystem.GetInput(fileName);

            var result1 = SolveA(content);
            Console.WriteLine($"Result a: " + result1);

            var result2 = SolveB(content);
            Console.WriteLine($"Result b: " + result2);
            Console.Read();
        }

        private static int SolveA(List<string> content)
        {
            var list = new List<Passport>();
            var current = new Passport();
            foreach (var line in content)
            {
                if (string.IsNullOrEmpty(line))
                {
                    list.Add(current);
                    current = new Passport();
                    continue;
                }

                var entries = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                current.AddPairs(entries);
            }
            list.Add(current);
            return list.Count(p => p.IsValid());
        }

        class Passport
        {
            public Dictionary<string, string> Data { get; } = new Dictionary<string, string>();

            public void AddPairs(string[] entries)
            {
                foreach (var entry in entries)
                {
                    var pair = entry.Split(':');
                    Data.Add(pair[0], pair[1]);
                }
            }

            public bool IsValid()
            {
                var required = new List<string>{ "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
                foreach (var field in required)
                {
                    if (!Data.ContainsKey(field)) return false;
                }

                return true;
            }
        }

        private static int SolveB(List<string> content)
        {
            return 0;
        }
    }
}
