using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day06 : DayBase
    {
        public Day06() : base("06")
        {
        }

        public override long SolveA()
        {
            var dict = new Dictionary<int, long>();
            foreach (var entry in Content.First().Split(","))
            {
                var age = int.Parse(entry);
                if (!dict.TryAdd(age, 1))
                {
                    dict[age]++;
                }
            }

            return RunSimulations(80, dict);
        }

        public override long SolveB()
        {
            var dict = new Dictionary<int, long>();
            foreach (var entry in Content.First().Split(","))
            {
                var age = int.Parse(entry);
                if (!dict.TryAdd(age, 1))
                {
                    dict[age]++;
                }
            }

            return RunSimulations(256, dict);
        }

        private long RunSimulations(int amount, Dictionary<int,long> fishes)
        {
            for(int i = 0; i < amount; i++)
            {
                var nextGeneration = new Dictionary<int, long>();

                foreach(var fish in fishes)
                {
                    if(fish.Key == 0)
                    {
                        nextGeneration.Add(8, fish.Value);

                        if(!nextGeneration.TryAdd(6, fish.Value))
                        {
                            nextGeneration[6] += fish.Value;
                        }
                    }
                    else if(fish.Key == 7)
                    {
                        // Would maybe be easier with ordered tupe list?
                        if (!nextGeneration.TryAdd(6, fish.Value))
                        {
                            nextGeneration[6] += fish.Value;
                        }
                    }
                    else
                    {
                        nextGeneration.Add(fish.Key - 1, fish.Value);
                    }
                }

                fishes = nextGeneration;
            }
            return fishes.Sum(f => f.Value);
        }
    }
}
