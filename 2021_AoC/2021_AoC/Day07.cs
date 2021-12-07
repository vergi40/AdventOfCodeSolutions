using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day07 : DayBase
    {
        public Day07() : base("07")
        {
        }

        public override long SolveA()
        {
            //var positions = "16,1,2,0,4,2,7,1,2,14".Split(",").Select(x => int.Parse(x)).ToList();
            var positions = Content.First().Split(",").Select(x => int.Parse(x));

            var length = positions.Max();

            var smallest = long.MaxValue;
            for(int i = 0; i < length; i++)
            {
                long fuel = 0;
                foreach(var position in positions)
                {
                    fuel += Math.Abs(position - i);
                }

                if (fuel < smallest) smallest = fuel;
            }

            return smallest;
        }

        public override long SolveB()
        {
            var positions = Content.First().Split(",").Select(x => int.Parse(x));

            var length = positions.Max();

            // Precalculate to save time
            var fuelDict = new FuelDict(length);

            var smallest = long.MaxValue;
            for (int i = 0; i < length; i++)
            {
                long fuel = 0;
                foreach (var position in positions)
                {
                    fuel += fuelDict.Values[Math.Abs(position - i)];
                }

                if (fuel < smallest) smallest = fuel;
            }

            return smallest;
        }

        class FuelDict
        {
            public Dictionary<int,int> Values { get; }

            public FuelDict(int length)
            {
                Values = new Dictionary<int,int>();

                var previous = 0;
                for(int i = 0; i <= length; i++)
                {
                    previous += i;
                    Values.Add(i, previous);
                }
            }
        }
    }
}
