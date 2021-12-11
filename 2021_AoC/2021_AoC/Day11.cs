using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day11 : DayBase
    {
        public Day11() : base("11")
        {
        }

        public override long SolveA()
        {
            var tempGrid = ParseStringListToInt2D(Content.ToList());
            var grid = Int2DToWrappedInt2D(tempGrid, int.MinValue);

            var (width, height) = Get2DMeasures(grid);
            var flashCounter = 0;

            // Steps
            for (int step = 0; step < 100; step++)
            {
                // Increase all
                for (int i = 1; i < width - 1; i++)
                {
                    for (int j = 1; j < height - 1; j++)
                    {
                        grid[i, j]++;
                    }
                }

                // Flashers
                var allFlashers = new HashSet<(int x, int y)>();
                while (true)
                {
                    // Collect all > 9
                    var stack = new Stack<(int x, int y)>();

                    for (int i = 1; i < width - 1; i++)
                    {
                        for (int j = 1; j < height - 1; j++)
                        {
                            if (grid[i, j] > 9 && !allFlashers.Contains((i,j)))
                            {
                                stack.Push((i, j));
                                allFlashers.Add((i, j));
                            }
                        }
                    }

                    if (!stack.Any())
                    {
                        break;
                    }

                    // Flash
                    while (stack.Any())
                    {
                        var flasher = stack.Pop();
                        grid[flasher.x - 1, flasher.y]++;
                        grid[flasher.x + 1, flasher.y]++;
                        grid[flasher.x, flasher.y - 1]++;
                        grid[flasher.x, flasher.y + 1]++;

                        grid[flasher.x + 1, flasher.y + 1]++;
                        grid[flasher.x + 1, flasher.y - 1]++;
                        grid[flasher.x - 1, flasher.y + 1]++;
                        grid[flasher.x - 1, flasher.y - 1]++;
                    }
                }

                // Set 0
                foreach (var (x,y) in allFlashers)
                {
                    grid[x,y] = 0;
                    flashCounter++;
                }
            }

            return flashCounter;
        }

        // Pretty lazy copy-paste

        public override long SolveB()
        {
            var tempGrid = ParseStringListToInt2D(Content.ToList());
            var grid = Int2DToWrappedInt2D(tempGrid, int.MinValue);

            var (width, height) = Get2DMeasures(grid);
            var flashCounter = 0;

            // Steps
            for (int step = 0; step < int.MaxValue; step++)
            {
                // Increase all
                for (int i = 1; i < width - 1; i++)
                {
                    for (int j = 1; j < height - 1; j++)
                    {
                        grid[i, j]++;
                    }
                }

                // Flashers
                var allFlashers = new HashSet<(int x, int y)>();
                while (true)
                {
                    // Collect all > 9
                    var stack = new Stack<(int x, int y)>();

                    for (int i = 1; i < width - 1; i++)
                    {
                        for (int j = 1; j < height - 1; j++)
                        {
                            if (grid[i, j] > 9 && !allFlashers.Contains((i, j)))
                            {
                                stack.Push((i, j));
                                allFlashers.Add((i, j));
                            }
                        }
                    }

                    if (!stack.Any())
                    {
                        break;
                    }

                    // Flash
                    while (stack.Any())
                    {
                        var flasher = stack.Pop();
                        grid[flasher.x - 1, flasher.y]++;
                        grid[flasher.x + 1, flasher.y]++;
                        grid[flasher.x, flasher.y - 1]++;
                        grid[flasher.x, flasher.y + 1]++;

                        grid[flasher.x + 1, flasher.y + 1]++;
                        grid[flasher.x + 1, flasher.y - 1]++;
                        grid[flasher.x - 1, flasher.y + 1]++;
                        grid[flasher.x - 1, flasher.y - 1]++;
                    }
                }

                if (allFlashers.Count == 100)
                    return step + 1;

                // Set 0
                foreach (var (x, y) in allFlashers)
                {
                    grid[x, y] = 0;
                    flashCounter++;
                }
            }

            return flashCounter;
        }
    }
}
