using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    class Day01 : DayBase
    {
        public Day01() : base("01") { }
        public override long SolveA()
        {
            var content = Content.Select(int.Parse).ToList();
            const int target = 2020;
            for (int i = 0; i < content.Count; i++)
            {
                var a = content[i];
                for (int j = i + 1; j < content.Count; j++)
                {
                    var b = content[j];
                    if (a + b == target) return a * b;
                }
            }

            throw new ArgumentException();
        }

        public override long SolveB()
        {
            var content = Content.Select(int.Parse).ToList();
            const int target = 2020;
            for (int i = 0; i < content.Count - 2; i++)
            {
                var a = content[i];
                for (int j = i + 1; j < content.Count - 1; j++)
                {
                    var b = content[j];
                    for (int k = i + 2; k < content.Count; k++)
                    {
                        var c = content[k];
                        if (a + b + c == target) return a * b * c;
                    }
                }
            }

            throw new ArgumentException();
        }
    }
}
