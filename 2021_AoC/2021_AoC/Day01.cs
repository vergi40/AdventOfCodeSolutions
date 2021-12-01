using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day01 : DayBase
    {
        public Day01() : base("01"){ }

        public override long SolveA()
        {
            var input = GetInputAsIntList();

            var counter = 0;
            for(var i = 0; i < input.Count - 1; i++)
            {
                var current = input[i];
                var next = input[i+1];

                if (next > current) counter++;
            }
            return counter;
        }

        public override long SolveB()
        {
            var input = GetInputAsIntList();

            // Build sums
            var sums = new List<int>();
            for (var i = 1; i < input.Count - 1; i++)
            {
                var sum = input[i - 1] + input[i] + input[i + 1];
                sums.Add(sum);
            }

            // Compare
            var counter = 0;
            for (var i = 0; i < sums.Count - 1; i++)
            {
                var current = sums[i];
                var next = sums[i + 1];

                if (next > current) counter++;
            }

            return counter;
        }
    }
}
