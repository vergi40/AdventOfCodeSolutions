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
            return 1;
        }

        public override long SolveB()
        {
            var result1 = long.Parse(Content.First());

            var result2 = ParseLineToLong(Content.First());

            var result3 = ParseInputToLong();
            return result1;

            throw new ArgumentException();
        }
    }
}
