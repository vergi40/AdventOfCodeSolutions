using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FSharp.Collections;

namespace _2021_AoC
{
    internal class Day15 : DayBase
    {
        /// <summary>
        /// [x,y]
        /// </summary>
        private int[,] map;
        private int width;
        private int height;

        private long evaluated = 0;
        private long evaluatedPaths = 0;

        public Day15() : base("15")
        {
            
        }

        public override long SolveA()
        {
            //var temp = ParseStringListToInt2D(example.ToList());
            var temp = ParseStringListToInt2D(Content.ToList());
            map = Int2DToWrappedInt2D(temp);
            (width, height) = Get2DMeasures(map);

            var path = IterateToFinish(
                new List<(int x, int y)>(){(1,11)}, 0, (1, 10));

            Console.WriteLine($"Travelled: {evaluated}");
            Console.WriteLine($"Paths evaluated: {evaluatedPaths}");
            if (path.Item1 != null) return path.sum - map[1,10];
            return 0;
        }

        private int GetSum(IEnumerable<(int, int)> path)
        {
            var sum = path.Skip(2).Sum(pair => map[pair.Item1, pair.Item2]);
            return sum;
        }

        public override long SolveB()
        {
            return 0;
        }

        /// <summary>
        /// Null: dead end
        /// </summary>
        /// <returns></returns>
        private (List<(int x, int y)>?, int sum) IterateToFinish(List<(int x, int y)> previousPath, int previousSum, (int x, int y) current)
        {
            if (current == (width - 2, 1))
            {
                // End
                evaluatedPaths++;
                previousPath.Add(current);
                return (previousPath, previousSum + map[current.x, current.y]);
            }

            // Current position ok
            var nextList = SortNextDirections(previousPath, current).ToList();

            var resultList = new List<(List<(int x, int y)>?,int sum)>();
            foreach (var next in nextList)
            {
                evaluated++;
                var currentPath = new List<(int x, int y)>(previousPath){current};
                var currentSum = previousSum + map[current.x, current.y];
                var result = IterateToFinish(currentPath, currentSum, next);
                if (result.Item1 == null)
                {
                    // There was some obstacle
                }
                else
                {
                    resultList.Add(result);
                }
            }

            if (resultList.Any())
            {
                resultList = resultList.OrderBy(item => item.sum).ToList();
                return resultList.First();
            }

            return (null,0);
        }

        private IEnumerable<(int x, int y)> SortNextDirections(List<(int x, int y)> behind, (int,int) current)
        {
            // Default directions: right and down
            var (x, y) = current;
            var right = map[x + 1, y];
            var down = map[x, y - 1];

            if (right < 0)
            {
                yield return (x, y - 1);
                yield break;
            }
            if (down < 0)
            {
                yield return (x + 1, y);
                yield break;
            }

            // Prefer the smaller risk
            if (right < down)
            {
                yield return (x + 1, y);
                yield return (x, y - 1);
            }
            else
            {
                yield return (x, y - 1);
                yield return (x + 1, y);
            }

        }

        private static List<string>  example = new()
        {
            "1163751742",
            "1381373672",
            "2136511328",
            "3694931569",
            "7463417111",
            "1319128137",
            "1359912421",
            "3125421639",
            "1293138521",
            "2311944581",
        };
    }
}
