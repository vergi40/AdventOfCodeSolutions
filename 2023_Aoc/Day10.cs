namespace _2023_Aoc
{
    public class Day10 : DayBase
    {
        private enum Dir{Right, Down, Left, Up};

        protected override string DayNumber { get; set; } = "10";

        private char[,] _grid;
        
        [Test]
        public void Test1()
        {
            var answer = 0;

            _grid = CreateGridWithExtraBoundaries();
            var sPos = GetSPos();

            // Start dirs: right, down
            var path1 = TravelNext(new Node(sPos, (1, 0)));
            var path2 = TravelNext(new Node(sPos, (0, -1)));
            for (int i = 1; i < 1000000; i++)
            {
                if (path1.pos == path2.pos)
                {
                    // win
                    answer = i;
                    break;
                }

                path1 = TravelNext(path1);
                path2 = TravelNext(path2);
            }

            Assert.That(answer, Is.EqualTo(6931));
        }

        private record Node((int, int) pos, (int, int) heading)
        {
            public (int, int) Next()
            {
                return (pos.Item1 + heading.Item1, pos.Item2 + heading.Item2);
            }
        }

        private Node TravelNext(Node node)
        {
            var nextPos = node.Next();
            var negHeading = (node.heading.Item1 * -1, node.heading.Item2 * -1);

            var newDirs = Dirs(nextPos);
            newDirs.Remove(negHeading);
            var nextHeading = newDirs.Single();

            return new Node(nextPos, nextHeading);
        }

        /// <summary>
        /// X,Y. '.' as extra boundary around
        /// </summary>
        /// <returns></returns>
        private char[,] CreateGridWithExtraBoundaries()
        {
            var lineArrays = Input.Reverse().ToList();
            var height = lineArrays.Count;
            var width = lineArrays[0].Length;

            var emptyLine = string.Concat(Enumerable.Repeat('.', width));
            lineArrays.Insert(0, emptyLine);
            lineArrays.Add(emptyLine);

            var grid = new char[width + 2, height + 2];
            for (int y = 0; y < height + 2; y++)
            {
                // Iterate rows. 0 is bottom row in input
                var line = $".{lineArrays[y]}.";
                for (int x = 0; x < width + 2; x++)
                {
                    // Iterate columns left to right
                    grid[x, y] = line[x];
                }
            }

            return grid;
        }

        private (int x, int y) GetSPos()
        {
            var width = 142;
            var s = (-1, -1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (_grid[i, j] == 'S')
                    {
                        s = (i, j);
                        break;
                    }
                }
            }

            return s;
        }

        private List<(int x, int y)> Dirs((int, int) pos)
        {
            var symbol = _grid[pos.Item1, pos.Item2];
            return symbol switch
            {
                '.' => new List<(int x, int y)>(),
                '-' => [(-1, 0), (1, 0)],
                '|' => [(0, 1), (0, -1)],
                'L' => [(0, 1), (1, 0)],
                'F' => [(1, 0), (0, -1)],
                '7' => [(-1, 0), (0, -1)],
                'J' => [(-1, 0), (0, 1)],
                _ => throw new ArgumentException()
            };
        }

        private List<Dir> DirsEnum((int, int) pos)
        {
            var symbol = _grid[pos.Item1, pos.Item2];
            return symbol switch
            {
                '.' => new List<Dir>(),
                '-' => [Dir.Left, Dir.Right],
                '|' => [Dir.Down, Dir.Up],
                'L' => [Dir.Up, Dir.Left],
                'F' => [Dir.Down, Dir.Right],
                '7' => [Dir.Down, Dir.Left],
                'J' => [Dir.Left, Dir.Up],
                _ => throw new ArgumentException()
            };
        }
    }
}