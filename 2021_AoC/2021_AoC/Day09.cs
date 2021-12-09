namespace _2021_AoC
{
    internal class Day09 : DayBase
    {
        public Day09() : base("09")
        {
        }

        public override long SolveA()
        {
            var grid = ParseStringListToInt2D(Content.ToList());

            var riskSum = 0;
            var checker = new NeighbourChecker(grid);
            
            
            // Width (x-axis) search
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (checker.IsLower(i, j, out var riskLevel))
                    {
                        riskSum += riskLevel;
                    }
                }
            }

            return riskSum;
        }

        public override long SolveB()
        {
            // Collect coordinates with accumulator
            var grid = ParseStringListToInt2D(Content.ToList());
            var checker = new BasinChecker(grid);

            var allAreas = new HashSet<(int,int)>();
            var sizes = new List<long>();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (allAreas.Contains((i, j))) continue;

                    var area = checker.TravelEachDirectionRec((i, j), new HashSet<(int, int)>());
                    if(area.Any())
                    {
                        sizes.Add(area.Count);
                        allAreas.UnionWith(area);
                    }
                }
            }

            //sizes.Sort();
            sizes = sizes.OrderByDescending(s => s).ToList();
            return sizes[0] * sizes[1] * sizes[2];

        }

        class BasinChecker
        {
            private readonly int _width;
            private readonly int _height;
            private readonly int[,] _grid;

            public BasinChecker(int[,] grid)
            {
                _width = grid.GetLength(0);
                _height = grid.GetLength(1);
                _grid = grid;
            }

            public HashSet<(int, int)> TravelEachDirectionRec((int, int) coordinates, HashSet<(int, int)> accumulated)
            {
                (int x, int y) = coordinates;
                if (x < 0 || y < 0 || x >= _width || y >= _height)
                {
                    return accumulated;
                }

                var value = _grid[x, y];
                if (value == 9)
                {
                    return accumulated;
                }

                accumulated.Add(coordinates);

                // Up, right, down, left
                if (!accumulated.Contains((x, y + 1)))
                {
                    accumulated = TravelEachDirectionRec((x, y + 1), accumulated);
                }
                if (!accumulated.Contains((x+1, y)))
                {
                    accumulated = TravelEachDirectionRec((x + 1, y), accumulated);
                }
                if (!accumulated.Contains((x, y - 1)))
                {
                    accumulated = TravelEachDirectionRec((x, y - 1), accumulated);
                }
                if (!accumulated.Contains((x - 1, y)))
                {
                    accumulated = TravelEachDirectionRec((x - 1, y), accumulated);
                }

                return accumulated;
            }
        }

        

        class NeighbourChecker
        {
            private readonly int _width;
            private readonly int _height;
            private readonly int[,] _grid;

            public NeighbourChecker(int[,] grid)
            {
                _width = grid.GetLength(0);
                _height = grid.GetLength(1);
                _grid = grid;
            }

            public bool IsLower(int x, int y, out int riskLevel)
            {
                riskLevel = 0;
                var value = _grid[x, y];

                var prevX = 9;
                var nextX = 9;
                var prevY = 9;
                var nextY = 9;

                if (x > 0) prevX = _grid[x - 1, y];
                if (x < _width - 1) nextX = _grid[x + 1, y];
                if (y > 0) prevY = _grid[x, y - 1];
                if (y < _height - 1) nextY = _grid[x, y + 1];

                if (value < prevX && value < nextX && value < prevY && value < nextY)
                {
                    riskLevel = _grid[x, y] + 1;
                    return true;
                }

                return false;
            }
        }
    }
}
