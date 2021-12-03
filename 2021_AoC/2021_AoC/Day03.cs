using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day03 : DayBase
    {
        public Day03() : base("03")
        {
        }

        public override long SolveA()
        {
            var lineCount = Content.Count;
            

            var lineLength = Content.First().Length;

            var ones = new int[lineLength];
            var gamma = new int[lineLength];


            foreach (var line in Content)
            {
                for(var i = 0; i < lineLength; i++)
                {
                    if(line[i] == '1')
                    {
                        ones[i]++;
                    }
                }
            }
            var epsilon = new int[lineLength];

            // If ones is larger than median at index, it's most common
            var mostCommon = "";
            var leastCommon = "";
            var median = lineCount * 0.5;
            for(var i = 0; i < lineLength; i++)
            {
                var sum = ones[i];
                if(sum > median)
                {
                    gamma[i]++;
                    mostCommon += '1';
                    leastCommon += '0';
                }
                else
                {
                    epsilon[i]++;
                    mostCommon += '0';
                    leastCommon += '1';
                }
            }


            Console.WriteLine($"Most common: {mostCommon}");
            Console.WriteLine($"Least common: {leastCommon}");

            // 3895776
            return 0;
        }


        public override long SolveB()
        {
            var content = Content.ToList();
            var lineLength = content.First().Length;

            // oxygen generator
            var oxygen = "";
            for(int i = 0; i < lineLength; i++)
            {
                content = EliminateNext(i, true, content);

                if(content.Count == 1)
                {
                    oxygen = content.Single().ToString();
                    break;
                }
            }

            // co2 scrubber
            var co2 = "";
            content = Content.ToList();
            for (int i = 0; i < lineLength; i++)
            {
                content = EliminateNext(i, false, content);

                if (content.Count == 1)
                {
                    co2 = content.Single().ToString();
                    break;
                }
            }

            // 20470 -> too low
            // 8025721 -> too high
            // 7928162
            return 0;
        }

        private List<string> EliminateNext(int index, bool findMostCommon, List<string> content)
        {
            

            var result = new List<string>();
            if (findMostCommon)
            {
                var mostCommon = MostCommonAtIndex(index, content, true);
                foreach (var line in content)
                {
                    if(int.Parse(line[index].ToString()) == mostCommon)
                    {
                        result.Add(line);
                    }
                }
            }
            else
            {
                var leastCommon = LeastCommonAtIndex(index, content);
                foreach (var line in content)
                {
                    if (int.Parse(line[index].ToString()) == leastCommon)
                    {
                        result.Add(line);
                    }
                }
            }

            return result;
        }


        public int MostCommonAtIndex(int index, List<string> content, bool preferUpper)
        {
            var lineCount = content.Count;
            var sum = 0;

            foreach (var line in content)
            {
                if (line[index] == '1')
                {
                    sum++;
                }
            }

            var median = lineCount * 0.5;
            if (sum > median) return 1;
            if (sum < median) return 0;

            if (preferUpper) return 1;
            return 0;
        }
        public int LeastCommonAtIndex(int index, List<string> content)
        {
            var lineCount = content.Count;
            var onesSum = 0;

            foreach (var line in content)
            {
                if (line[index] == '1')
                {
                    onesSum++;
                }
            }

            var median = lineCount * 0.5;
            if (onesSum > median) return 0;
            if (onesSum < median) return 1;

            return 0;
        }

        public int LeastCommon(int mostCommon)
        {
            if (mostCommon == 0) return 1;
            return 0;
        }

    }
}
