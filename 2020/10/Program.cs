using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Common;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "10";
            var content = FileSystem.GetInput(fileName);

            var result1 = SolveA(content);
            Console.WriteLine($"Result a: " + result1);

            //var example = FileSystem.GetInput($"{fileName}-example");
            //var example1Result = SolveA(example);
            //var example2Result = SolveB(example);

            // 32 396 521 357 312
            // 32 trillion
            var result2 = SolveB(content);
            Console.WriteLine($"Result b: " + result2);
            Console.Read();
        }
        
        private static int SolveA(List<string> content)
        {
            var list = content.Select(s => int.Parse(s)).ToList();
            list.Insert(0, 0);

            list.Sort();
            list.Add(list.Last() + 3);

            var numberOf1Jolt = 0;
            var numberOf3Jolt = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {

                var diff = list[i + 1] - list[i];
                if (diff == 1) numberOf1Jolt++;
                else if (diff == 3) numberOf3Jolt++;
            }

            return numberOf1Jolt * numberOf3Jolt;
        }

        private static long SolveB(List<string> content)
        {
            var list = content.Select(s => int.Parse(s)).ToList();
            list.Insert(0, 0);

            list.Sort();
            list.Add(list.Last() + 3);

            var adapters = list.Select(l => new Adapter(l)).ToList();

            // Find all connections
            foreach (var parent in adapters)
            {
                var childList = adapters.Where(a => a != parent).ToList();
                foreach (var child in childList)
                {
                    if (parent.CanConnectNext(child))
                    {
                        parent.Children.Add(child);
                    }
                }
            }

            long distincts = TraverseIterate(adapters.First());
            return distincts;
        }


        private static long TraverseIterate(Adapter adapter)
        {
            // At the end
            if(adapter.Children.Count == 0) return 1;

            long count = 0;
            foreach (var child in adapter.Children)
            {
                // Already visited
                if (child.ChildrenResult > 0)
                {
                    count += child.ChildrenResult;
                    continue;
                }

                // Save result
                var result = TraverseIterate(child);
                child.ChildrenResult = result;
                count += result;
            }

            return count;
        }

        class Adapter
        {
            public int Jolts { get; }
            public long ChildrenResult { get; set; } = 0;

            public List<Adapter> Children { get; }

            public Adapter(int jolts)
            {
                Jolts = jolts;
                Children = new List<Adapter>();
            }

            public bool CanConnectPrevious(Adapter previous)
            {
                if (previous.Jolts + 1 == Jolts) return true;
                if (previous.Jolts + 2 == Jolts) return true;
                if (previous.Jolts + 3 == Jolts) return true;
                return false;
            }

            public bool CanConnectNext(Adapter next)
            {
                if (next.Jolts - 1 == Jolts) return true;
                if (next.Jolts - 2 == Jolts) return true;
                if (next.Jolts - 3 == Jolts) return true;
                return false;
            }
        }

    }
}
