using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FSharp.Core;

namespace _2021_AoC
{
    internal class Day08 : DayBase
    {
        public Day08() : base("08")
        {
            // 4 digits. e.g. "1234"
            // 7 segments for each digit - abcdefg
        }

        public override long SolveA()
        {
            // beacf afbd bcead cgefa ecdbga efb gbfdeac ecgfbd acbdfe fb | bf efb bgecdfa egcfa
            // ------------------signal patterns-------------------------   ----output value----
            var times = 0;
            var targetLength = new[] { 2, 3, 4, 7 };
            foreach(var line in Content)
            {
                var outputValues = line.Split(" | ")[1].Split(" ");
                foreach (var value in outputValues)
                {
                    foreach (var length in targetLength)
                    {
                        if (value.Length == length) times++;
                    }
                }
            }

            return times;
        }

        public override long SolveB()
        {
            var times = 0;
            var targetLength = new[] { 2, 3, 4, 7 };
            var connections = new List<Connection>();

            foreach (var line in Content)
            {
                var output = line.Split(" | ");
                var signalValues = output[0].Split(" ").ToList();
                var outputValues = output[1].Split(" ").ToList();

                var connection = new Connection(signalValues, outputValues);
                connections.Add(connection);
            }

            return times;
        }

        class Connection
        {
            public Dictionary<string, int> Signals { get; set; } = new();

            public Connection(List<string> start, List<string> end)
            {
                var output = end;
                var merge = start.Concat(end).ToList();

                foreach (var key in merge)
                {
                    var sorted = SortString(key);
                    if (!Signals.TryAdd(sorted, 1))
                    {
                        Signals[sorted]++;
                    }
                }


            }

            private static string SortString(string input)
            {
                var sortedArray = input.ToArray();
                Array.Sort(sortedArray);
                return new string(sortedArray);
            }
        }
        
    }
}
