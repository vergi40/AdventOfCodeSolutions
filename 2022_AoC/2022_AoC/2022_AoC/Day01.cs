using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2021_AoC;

namespace _2022_AoC
{
    internal class Day01 : DayBase
    {
        public Day01() : base("01") { }
        
        public override long SolveA()
        {
            return Sums().Max();
        }

        public override long SolveB()
        {
            var ordered = Sums().ToList().OrderByDescending(s => s).ToList();
            return ordered[0] + ordered[1] + ordered[2];
        }

        private List<int> Sums()
        {
            var sums = new List<int>();
            var sum = 0;
            foreach (var line in Content)
            {
                if (string.IsNullOrEmpty(line))
                {
                    sums.Add(sum);
                    sum = 0;
                }
                else
                {
                    sum += int.Parse(line);
                }
            }

            if (sum > 0)
            {
                sums.Add(sum);
            }

            return sums;
        }

    }
}
