using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solutions
{
    class Day15 : DayBase
    {
        public Day15() : base("15")
        {
        }

        public override long SolveA()
        {
            var line = Content.First();
            //line = "0,3,6";
            //line = "3,1,2";

            var initialNumbers = line.Split(',').Select(int.Parse).ToList();
            var numbers = new List<int>();
            numbers.AddRange(initialNumbers);

            //var dict = new HashSet<int>();
            //dict.UnionWith(numbers.SkipLast(1));

            // 9,12,1,4,17,0,18
            for (int i = initialNumbers.Count - 1; i < 2020 - 1; i++)
            {
                var previous = numbers[i];
                if (numbers.SkipLast(1).Contains(previous))
                {
                    var index = numbers.SkipLast(1).ToList().LastIndexOf(previous);
                    var diff = numbers.Count - 1 - index;

                    numbers.Add(diff);
                }
                else
                {
                    // Not spoken
                    numbers.Add(0);
                }
            }

            return numbers.Last();
        }

        public override long SolveB()
        {
            var line = Content.First();
            //line = "0,3,6";
            //line = "3,1,2";

            var initialNumbers = line.Split(',').Select(int.Parse).ToList();
            var numbers = new List<int>();
            numbers.AddRange(initialNumbers);
            
            // Maintain found numbers and last indexes for fast retrieval
            // Key = spoken number
            // Value = index when last seen
            var dict = new Dictionary<int, int>();
            for(int i = 0; i < numbers.Count - 1; i++)
            {
                var number = numbers[i];
                if (dict.ContainsKey(number))
                {
                    dict[number] = i;
                }
                else
                {
                    dict.Add(number, i);
                }
            }

            var previouslyAdded = numbers.Last();

            // 9,12,1,4,17,0,18
            for (int i = initialNumbers.Count - 1; i < 30000000 - 1; i++)
            {
                var previous = numbers[i];
                if (dict.ContainsKey(previous))
                {
                    var index = dict[previous];
                    var diff = numbers.Count - 1 - index;

                    // Add or update previous round
                    if (dict.ContainsKey(previouslyAdded))
                    {
                        dict[previouslyAdded] = i;
                    }
                    else
                    {
                        dict.Add(previouslyAdded, i);
                    }

                    numbers.Add(diff);
                    previouslyAdded = diff;
                }
                else
                {
                    // Not spoken
                    // Add or update previous round
                    if (dict.ContainsKey(previouslyAdded))
                    {
                        dict[previouslyAdded] = i;
                    }
                    else
                    {
                        dict.Add(previouslyAdded, i);
                    }

                    numbers.Add(0);
                    previouslyAdded = 0;
                }
            }

            return numbers.Last();
        }
    }
}
