using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    class Day24 : DayBase
    {

        private List<List<string>> Instructions { get; } = new();
        public Day24() : base("24")
        {
            foreach (var line in Content)
            {
                // Split input to direction strings
                var split = new List<string>();
                for (int i = 0; i < line.Length; i++)
                {
                    if(i < line.Length - 1)
                    {
                        var sub = line.Substring(i, 2);
                        if(sub == "nw" || sub == "ne" || sub == "sw" || sub == "se")
                        {
                            split.Add(sub);
                            i++;
                            continue;
                        }
                    }
                    split.Add(line[i].ToString());
                }
                Instructions.Add(split);
            }
        }

        public override long SolveA()
        {
            var dict = BuildDict();
            return dict.Values.Where(t => t).ToList().Count;
        }

        private Dictionary<(int x, int y), bool> BuildDict()
        {
            //   nw     ne
            //   .-'  '-.
            //
            // |          |
            //
            //   '-.  .-'
            //   sw     se

            // ----> x
            // Lets decide coordinate system
            // East and west means stepping 2 units x
            // Nw ne etc means stepping 1 unit x, 1 unit y

            // Let's keep results in dictionary
            var dict = new Dictionary<(int x, int y), bool>();

            foreach (var instruction in Instructions)
            {
                var tile = CalculatePath(instruction);
                if (dict.ContainsKey(tile)) dict[tile] = !dict[tile];
                else dict.Add(tile, true);
            }

            return dict;
        }

        private (int x, int y) CalculatePath(List<string> instruction)
        {
            var x = 0;
            var y = 0;
            foreach (var dir in instruction)
            {
                switch (dir)
                {
                    case "e": 
                        x += 2;
                        break;
                    case "w":
                        x -= 2;
                        break;
                    case "se":
                        x += 1;
                        y -= 1;
                        break;
                    case "sw":
                        x -= 1;
                        y -= 1;
                        break;
                    case "ne":
                        x += 1;
                        y += 1;
                        break;
                    case "nw":
                        x -= 1;
                        y += 1;
                        break;
                }
            }

            return (x, y);
        }

        public override long SolveB()
        {
            // Build array that is looped through in each cycle
            var dict = BuildDict();
            var width = 2000;
            var height = 2000;

            // Should not be a problem to do 1000000 cells
            var floor = new bool[width, height];
            foreach (var pair in dict)
            {
                if (pair.Value)
                {
                    var x = pair.Key.x + 1000;
                    var y = pair.Key.y + 1000;
                    floor[x, y] = true;
                }
            }

            for (int i = 0; i < 100; i++)
            {
                // Cycles
                var newFloor = new bool[width, height];

                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < height; k++)
                    {
                        var tile = floor[j, k];
                        var count = AdjacentBlackTiles(floor, j, k, width, height);
                        if (tile)
                        {
                            // black
                            if (count == 0 || count > 2) newFloor[j, k] = false;//Unnecessary operation
                            else newFloor[j, k] = true;
                        }
                        else
                        {
                            if (count == 2) newFloor[j, k] = true;
                            else newFloor[j, k] = false;//Unnecessary operation
                        }
                    }
                }

                floor = newFloor;

                // Testing
                var blackTemp = 0;
                for (int i1 = 0; i1 < width; i1++)
                {
                    for (int j1 = 0; j1 < height; j1++)
                    {
                        if (floor[i1, j1]) blackTemp++;
                    }
                }
                Console.WriteLine($"Cycle {i+1}, blacks {blackTemp}");
            }


            var black = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (floor[i, j]) black++;
                }
            }

            return black;
        }

        private int AdjacentBlackTiles(bool[,] floor, int x, int y, int width, int height)
        {
            var count = 0;
            if (IsInsideAndBlack(floor, x + 1, y + 1, width, height)) count++;
            if (IsInsideAndBlack(floor, x + 1, y - 1, width, height)) count++;
            if (IsInsideAndBlack(floor, x - 1, y + 1, width, height)) count++;
            if (IsInsideAndBlack(floor, x - 1, y - 1, width, height)) count++;
            if (IsInsideAndBlack(floor, x + 2, y, width, height)) count++;
            if (IsInsideAndBlack(floor, x - 2, y, width, height)) count++;
            return count;
        }

        private bool IsInsideAndBlack(bool[,] floor, int x, int y, int width, int height)
        {
            if (x < 0 || x > width - 1) return false;
            if (y < 0 || y > height - 1) return false;
            return floor[x, y];
        }
    }
}
