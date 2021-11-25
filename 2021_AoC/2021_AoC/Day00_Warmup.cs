using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day00_Warmup : DayBase
    {
        public Day00_Warmup() : base("00")
        {
        }

        public override long SolveA()
        {
            Utils.Say.hello("F# call");

            var filePath = Common.FileSystem.GetInputFilePath("00");
            Utils.Day00.solve(filePath);
            Utils.Day00.solveA(filePath);

            var exampleInput = new List<int> { 1721, 979, 366, 299, 675, 1456 };
            var exampleResult = 0;

            for(var i = 0; i < exampleInput.Count - 1; i++)
            {
                for(var j = i + 1; j < exampleInput.Count; j++)
                {
                    if (exampleInput[i] + exampleInput[j] == 2020) exampleResult = exampleInput[i] * exampleInput[j];
                }
            }

            var input = GetInputAsIntList();
            for (var i = 0; i < input.Count - 1; i++)
            {
                for (var j = i + 1; j < input.Count; j++)
                {
                    if (input[i] + input[j] == 2020) return input[i] * input[j];
                }
            }

            throw new NotImplementedException();
        }

        public override long SolveB()
        {
            var input = GetInputAsLongList();

            for (var i = 0; i < input.Count - 2; i++)
            {
                for (var j = i + 1; j < input.Count - 1; j++)
                {
                    for (var k = j + 1; k < input.Count; k++)
                    {
                        if (input[i] + input[j] + input[k] == 2020) return input[i] * input[j] * input[k];
                    }
                }
            }
            throw new NotImplementedException();
        }
    }
}
