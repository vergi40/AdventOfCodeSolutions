using NUnit.Framework;

namespace CSharp
{
    public class Day06 : DayBase
    {
        protected override string DayNumber { get; set; } = "06";

        [Test]
        public void Test1()
        {
            var sum = 0;

            var map = new Map2D(Input);
            var startPos = map.FindElements("<>^v").First();
            var dir = GetStartDir(startPos.element);

            var traveller = new MapTraveller(map);
            traveller.SetPosition((startPos.x, startPos.y));

            var path = new HashSet<(int, int)>();
            path.Add((startPos.x, startPos.y));
            while (true)
            {
                var next = traveller.Peek(dir);
                if (next == '.')
                {
                    traveller.Move(dir);
                    path.Add(traveller.CurrentPos);
                }
                else if(next == MapTraveller.OutOfBounds)
                {
                    // Calculate sum
                    sum = path.Count;
                    break;
                }
                else if (next == '#')
                {
                    dir = Rotate90(dir);
                }
            }

            Assert.That(sum, Is.EqualTo(5551));
        }

        private static (int xDir, int yDir) GetStartDir(char arrow)
        {
            return arrow switch
            {
                '<' => (-1, 0),
                '^' => (0, 1),
                '>' => (1, 0),
                'v' => (0, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(arrow))
            };
        }

        private static (int xDir, int yDir) Rotate90((int, int) previous)
        {
            return previous switch
            {
                (0, -1) => (-1, 0),
                (-1, 0) => (0, 1),
                (0, 1) => (1, 0),
                (1, 0) => (0, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(previous))
            };
        }

    }

    /// <summary>
    /// 2D map with X and Y. Origin is in bottom left corner
    /// Y
    /// ^
    /// |
    /// |
    /// 0 -----> X
    /// </summary>
    public class Map2D
    {
        public int Height { get; }
        public int Width { get; }

        /// <summary>
        /// X,Y
        /// </summary>
        public char[,] Grid { get; }
        public Map2D(IReadOnlyList<string> rows)
        {
            Height = rows.Count;
            Width = rows[0].Length;
            Grid = new char[Width, Height];

            // Set last row to be i=Y=0
            var rowsReverse = rows.Reverse().ToList();
            for (int i = 0; i < Height; i++)
            {
                var row = rowsReverse[i];
                for (int j = 0; j < Width; j++)
                {
                    Grid[j, i] = row[j];
                }
;           }
        }

        public IReadOnlyList<(int x, int y)> FindElement(char element)
        {
            var result = new List<(int, int)>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Grid[x, y] == element)
                    {
                        result.Add((x,y));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Give list of chars to find
        /// </summary>
        /// <param name="elements">E.g. asdf</param>
        /// <returns>E.g. (0,0,a),(0,5,f)</returns>
        public IReadOnlyList<(int x, int y, char element)> FindElements(string elements)
        {
            var result = new List<(int, int, char)>();
            foreach (var element in elements)
            {
                foreach (var (x, y) in FindElement(element))
                {
                    result.Add((x,y,element));
                }
            }

            return result;
        }
    }

    public class MapTraveller
    {
        public const char OutOfBounds = 'ä';

        private readonly Map2D _map;
        public (int x, int y) CurrentPos { get; private set; }

        public MapTraveller(Map2D map)
        {
            _map = map;
            CurrentPos = (0, 0);
        }

        public void SetPosition((int x, int y) pos)
        {
            CurrentPos = pos;
        }

        public char Peek((int xDir, int yDir) dir)
        {
            var (x,y) = (CurrentPos.x + dir.xDir, CurrentPos.y + dir.yDir);
            if (CheckOutOfBounds((x,y)))
            {
                return OutOfBounds;
            }

            return _map.Grid[x, y];
        }

        private bool CheckOutOfBounds((int x, int y) pos)
        {
            var (x, y) = pos;
            if (x < 0 || x + 1 > _map.Width)
            {
                return true;
            }

            if (y < 0 || y + 1 > _map.Height)
            {
                return true;
            }

            return false;
        }


        public void Move((int xDir, int yDir) dir)
        {
            CurrentPos = (CurrentPos.x + dir.xDir, CurrentPos.y + dir.yDir);
        }
    }
}