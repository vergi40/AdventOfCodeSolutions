using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    class Day03 : DayBase
    {
        public Day03() : base("03")
        {

        }

        public override long SolveA()
        {
            const char tree = '#';
            var treeCount = 0;
            var x = 0;

            foreach (var line in Content.Skip(1))
            {
                x += 3;

                // Goes over
                if (x > line.Length - 1)
                {
                    x -= line.Length;
                }

                if (line[x] == tree)
                {
                    treeCount++;
                }
            }

            return treeCount;
        }

        public override long SolveB()
        {
            const char tree = '#';
            const int a = 0;
            const int b = 1;
            const int c = 2;
            const int d = 3;
            const int e = 4;

            var horizontalIndex = new List<int> { 0, 0, 0, 0, 0 };
            var treeCounts = new List<int> { 0, 0, 0, 0, 0 };


            for (int i = 1; i < Content.Count; i++)
            {
                var line = Content[i];
                horizontalIndex[a] += 1;
                horizontalIndex[b] += 3;
                horizontalIndex[c] += 5;
                horizontalIndex[d] += 7;

                if (i % 2 == 0)
                {
                    horizontalIndex[e] += 1;
                }

                // Goes over
                for (int j = 0; j < horizontalIndex.Count; j++)
                {
                    if (horizontalIndex[j] > line.Length - 1)
                    {
                        horizontalIndex[j] -= line.Length;
                    }
                }

                for (int j = 0; j < horizontalIndex.Count - 1; j++)
                {
                    var columnIndex = horizontalIndex[j];
                    if (line[columnIndex] == tree)
                    {
                        treeCounts[j]++;
                    }
                }

                if (i % 2 == 0 && line[horizontalIndex[e]] == tree)
                {
                    treeCounts[e]++;
                }
            }

            // 82 173 84 80 46
            // 90209024
            return treeCounts[a] * treeCounts[b] * treeCounts[c] * treeCounts[d] * treeCounts[e];
        }
    }
}
