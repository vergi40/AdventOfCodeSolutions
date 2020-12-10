using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace _03
{
    class Program
    {
        static void Main(string[] args)
        {
            var content = FileSystem.GetInput("03");

            var result1 = SolveA(content);
            Console.WriteLine($"Result a: " + result1);

            var example = FileSystem.GetInput("03-example");
            var asd = SolveA(example);
            var exampleResult = SolveB(example);
            // 90209024
            Console.WriteLine($"Result example: " + exampleResult);

            var result2 = SolveB(content);
            // 90209024
            Console.WriteLine($"Result b: " + result2);
        }

        private static int SolveA(List<string> content)
        {
            const char tree = '#';
            var treeCount = 0;
            var x = 0;

            foreach (var line in content.Skip(1))
            {
                x += 3;
                
                // Goes over
                if (x > line.Length - 1)
                {
                    x -= line.Length;
                }

                if (line[x] == tree)
                {
                    treeCount++;
                }
            }

            return treeCount;
        }

        private static int SolveB(List<string> content)
        {
            const char tree = '#';
            const int a = 0;
            const int b = 1;
            const int c = 2;
            const int d = 3;
            const int e = 4;

            var horizontalIndex = new List<int> {0, 0, 0, 0, 0};
            var treeCounts = new List<int> {0, 0, 0, 0, 0};
            

            for(int i = 1; i < content.Count; i++)
            {
                var line = content[i];
                horizontalIndex[a] += 1;
                horizontalIndex[b] += 3;
                horizontalIndex[c] += 5;
                horizontalIndex[d] += 7;

                if(i % 2 == 0)
                {
                    horizontalIndex[e] += 1;
                }

                // Goes over
                for(int j = 0; j < horizontalIndex.Count; j++)
                {
                    if (horizontalIndex[j] > line.Length - 1)
                    {
                        horizontalIndex[j] -= line.Length;
                    }
                }

                for (int j = 0; j < horizontalIndex.Count - 1; j++)
                {
                    var columnIndex = horizontalIndex[j];
                    if (line[columnIndex] == tree)
                    {
                        treeCounts[j]++;
                    }
                }

                if (i % 2 == 0 && line[horizontalIndex[e]] == tree)
                {
                    treeCounts[e]++;
                }
            }

            // 82 173 84 80 46
            // 90209024
            return treeCounts[a] * treeCounts[b] * treeCounts[c] * treeCounts[d] * treeCounts[e];
        }
    }
}
