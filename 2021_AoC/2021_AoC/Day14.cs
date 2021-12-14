using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    // Problems:
    // OutOfMemory
    // Large numbers
    // Exponential growth
    // Needs efficient algorithm
    internal class Day14:DayBase
    {
        public Day14() : base("14")
        {
        }

        public override long SolveA()
        {
            //return Solve(Content.ToList(), 10);
            return Solve(Content.ToList(), 10);
        }

        public override long SolveB()
        {
            return Solve(Content.ToList(), 40);
        }

        private long Solve(List<string> content, int steps)
        {
            var polymer = content.First();
            var rules = content.GetRange(2, content.Count - 2);
            var dict = new Dictionary<string, char>();
            foreach (var line in rules)
            {
                var input = line.Split(" -> ");
                dict.Add(input[0], input[1].Single());
            }

            var pairCounter = new Dictionary<string, long>();

            // Init
            foreach (var key in dict.Keys)
            {
                pairCounter.Add(key, 0);
            }

            for (int i = 0; i < polymer.Length - 1; i++)
            {
                var pair = polymer.Substring(i, 2);
                pairCounter[pair]++;
            }

            // 
            for (int step = 0; step < steps; step++)
            {
                var newPairs = new Dictionary<string, long>();
                foreach (var (pair, count) in pairCounter)
                {
                    var middle = dict[pair].ToString();
                    var pair1 = pair[0] + middle;
                    var pair2 = middle + pair[1];

                    if (!newPairs.TryAdd(pair1, count))
                    {
                        newPairs[pair1] += count;
                    }
                    if (!newPairs.TryAdd(pair2, count))
                    {
                        newPairs[pair2] += count;
                    }
                }

                pairCounter = newPairs;
            }

            // Count chars
            // A A B C
            // AA AB BC
            // A = 2
            // B = 1
            // C = 1
            //
            // A B B A A A C A C
            // AB BB BA AA AA AC CA AC
            // BB = 1
            // AA = 2
            // A = AA + AA + (AB + BA + AC + CA + AC) = 9 / 2 = 5
            // B = BB + (AB + BA) = 4 / 2 = 2
            // C = AC + CA + AC = 3 / 2 = 2

            // First count how many times each char is found in pairs
            var result = new Dictionary<char, long>();
            foreach (var (pair, count) in pairCounter)
            {
                if (!result.TryAdd(pair[0], count))
                {
                    result[pair[0]] += count;
                }
                if (!result.TryAdd(pair[1], count))
                {
                    result[pair[1]] += count;
                }
            }

            // Half down (because of pairs)
            var result2 = new Dictionary<char, long>();
            foreach (var (c, count) in result)
            {
                var count2 = (long)Math.Ceiling(count * 0.5);
                if (!result2.TryAdd(c, count2))
                {
                    result2[c] += count2;
                }
            }

            var max = result2.Values.Max();
            var min = result2.Values.Min();
            return max - min;
        }

        private static List<string> example = new()
        {
            "NNCB",
            "",
            "CH -> B",
            "HH -> N",
            "CB -> H",
            "NH -> C",
            "HB -> C",
            "HC -> B",
            "HN -> C",
            "NN -> C",
            "BH -> H",
            "NC -> B",
            "NB -> B",
            "BN -> B",
            "BB -> N",
            "BC -> B",
            "CC -> N",
            "CN -> C"
        };
    }
}
