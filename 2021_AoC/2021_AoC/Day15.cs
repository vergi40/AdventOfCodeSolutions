using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Day15() : base("15")
        {
            
        }

        public override long SolveA()
        {
            var temp = ParseStringListToInt2D(example.ToList());
            //var temp = ParseStringListToInt2D(Content.ToList());
            map = Int2DToWrappedInt2D(temp);
            (width, height) = Get2DMeasures(map);

            var path = IterateToFinish(
                new HashSet<(int x, int y)>(), 
                new List<(int x, int y)>(){(0,1)}, 
                (1, 1));

            return 0;
        }

        public override long SolveB()
        {
            return 0;
        }

        private List<(int x, int y)> IterateToFinish(HashSet<(int x, int y)> visited,
            List<(int x, int y)> behind, (int x, int y) current)
        {
            if (current == (width - 2, height - 2))
            {
                // End
                behind.Add(current);
                return behind;
            }

            if (visited.Contains(current))
            {
                // Ended up to visited path
                return new List<(int x, int y)>();
            }
            else if (map[current.x, current.y] == -1)
            {
                // Edge
                return new List<(int x, int y)>();
            }

            // Current position ok
            var nextList = CalculateNextDirections(behind, current).ToList();
            behind.Add(current);
            visited.Add(current);

            // debug print

            foreach (var next in nextList)
            {
                var result = IterateToFinish(visited, behind.ToList(), next);
                if (result.Count == 0)
                {
                    // There was some obstacle
                }
                else
                {
                    // Found path!
                    // TODO add risk sum calculation
                    return result;
                }
            }

            return new List<(int x, int y)>();

        }

        private static IEnumerable<(int x, int y)> CalculateNextDirections(List<(int x, int y)> behind, (int x, int y) current)
        {
            var previousX = current.x - behind.Last().x;
            var previousY = current.y - behind.Last().y;
            if (previousX == 0)
            {
                // Last movement was vertical
                yield return (current.x, current.y + previousY);
                yield return (current.x + 1, current.y);
                yield return (current.x - 1, current.y);
            }
            else
            {
                yield return (current.x + previousX, current.y);
                yield return (current.x, current.y + 1);
                yield return (current.x, current.y - 1);
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
