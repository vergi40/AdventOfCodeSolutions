using _2021_AoC;

namespace _2022_AoC
{
    internal class Day05 : DayBase
    {
        public Day05() : base("05") { }

        public override long SolveA()
        {
            var initLines = Content.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).SkipLast(1);
            var grid = new Layout();
            grid.Init(initLines);

            foreach (var line in Content.Where(l => l.StartsWith("move")))
            {
                grid.ExecuteSingleMove(line);
            }

            Console.WriteLine($"Result A: {grid.PrintResult()}");
            return 0;
        }

        public override long SolveB()
        {
            var initLines = Content.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).SkipLast(1);
            var grid = new Layout();
            grid.Init(initLines);

            foreach (var line in Content.Where(l => l.StartsWith("move")))
            {
                grid.ExecuteStackMove(line);
            }

            Console.WriteLine($"Result B: {grid.PrintResult()}");
            return 0;
        }

        class Layout
        {
            public Dictionary<int, Stack<char>> Grid { get; } = new();

            public void Init(IEnumerable<string> lines)
            {
                // [G] [G] [C] [J] [P] [P] [Z] [R] [H]
                //  1   2   3   4   5   6   7   8   9
                const int size = 4;
                foreach (var line in lines.Reverse())
                {
                    var lineFixed = line + ' ';
                    for (int i = 0; i < lineFixed.Length / size; i++)
                    {
                        var block = lineFixed.Substring(i * size, size);
                        if (string.IsNullOrWhiteSpace(block)) continue;

                        // On purpose use C#11 list patterns instead just split()
                        if (block is ['[', char c, ']', ..])
                        {
                            if(Grid.TryGetValue(i + 1, out var stack))
                            {
                                stack.Push(c);
                            }
                            else
                            {
                                Grid.Add(i + 1, new Stack<char>(new List<char>{c}));
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"Unknown syntax {block}");
                        }
                    }
                }
            }

            public void ExecuteSingleMove(string line)
            {
                // https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching#list-patterns
                string[] array = line.Split(' ');

                // move 3 from 4 to 3
                if (array is ["move", var amount, "from", var from, "to", var to])
                {
                    ExecuteSingleMove(int.Parse(amount), int.Parse(from), int.Parse(to));
                }
                else
                {
                    throw new ArgumentException($"Unknown syntax {line}");
                }
            }

            private void ExecuteSingleMove(int amount, int from, int to)
            {
                for (int i = 0; i < amount; i++)
                {
                    var target = Grid[from].Pop();
                    Grid[to].Push(target);
                }
            }

            public string PrintResult()
            {
                var result = "";
                for (int i = 0; i < Grid.Count; i++)
                {
                    result += Grid[i + 1].Peek();
                }

                return result;
            }

            public void ExecuteStackMove(string line)
            {
                // https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching#list-patterns
                string[] array = line.Split(' ');

                // move 3 from 4 to 3
                if (array is ["move", var amount, "from", var from, "to", var to])
                {
                    ExecuteStackMove(int.Parse(amount), int.Parse(from), int.Parse(to));
                }
                else
                {
                    throw new ArgumentException($"Unknown syntax {line}");
                }
            }

            private void ExecuteStackMove(int amount, int from, int to)
            {
                var modStack = new Stack<char>();

                for (int i = 0; i < amount; i++)
                {
                    modStack.Push(Grid[from].Pop());
                }

                for (int i = 0; i < amount; i++)
                {
                    Grid[to].Push(modStack.Pop());
                }
            }
        }
    }
}