using System.Diagnostics;

namespace _2021_AoC
{
    internal class Day24 : DayBase
    {
        public Day24() : base("24")
        {
            Console.WriteLine($"Split ALU commands by each input. Find all possible outcomes for 1st digit.");
            Console.WriteLine("Use them combined to output z values for 2nd digit. Repeat");
        }

        protected override void InitializeChild()
        {
            Phases = ParsePhases(Content);
        }

        public Dictionary<int, Phase> Phases { get; set; }

        /// <summary>
        /// Do memoization. If digit with certain depth is already inspected, don't repeat
        /// </summary>
        private HashSet<(int digit, int depth, int z)> Mem { get; } = new();


        public override long SolveA()
        {
            Console.WriteLine($"A: Find largest possible number");
            Mem.Clear();
            var totalWatch = new Stopwatch();
            totalWatch.Start();

            var digit1List = Phases[1].GetAllOutputsForZ(0, true).OrderByDescending(x => x.digit);
            var startList = new List<(int, int)>() { digit1List.First()};

            var result = DepthFirstRec(2, startList, true);
            var resultString = string.Join("", result.Select(pair => pair.digit));
            Console.WriteLine($"Search time: {totalWatch.Elapsed.ToString()}");

            return long.Parse(resultString);
        }

        public override long SolveB()
        {
            Console.WriteLine($"B: Find lowest possible number");
            Mem.Clear();
            var totalWatch = new Stopwatch();
            totalWatch.Start();

            var digit1List = Phases[1].GetAllOutputsForZ(0, false).OrderBy(x => x.digit);
            foreach (var digit1Values in digit1List)
            {
                var startList = new List<(int, int)>() { digit1Values };
                var result = DepthFirstRec(2, startList, false);

                if(result.Any())
                {
                    var resultString = string.Join("", result.Select(pair => pair.digit));
                    Console.WriteLine($"Search time: {totalWatch.Elapsed.ToString()}");

                    return long.Parse(resultString);
                }
            }

            return 0;
        }

        private List<(int digit, int outputZ)> DepthFirstRec(int depth, List<(int digit, int outputZ)> newList, bool findHighest)
        {
            var current = Phases[depth].GetAllOutputsForZ(newList.Last().outputZ, findHighest).ToList();
            current = current.Where(x => x.outputZ >= 0).ToList();
            
            foreach (var valueTuple in current)
            {
                if (depth == 14)
                {
                    if (valueTuple.outputZ == 0)
                    {
                        newList.Add(valueTuple);
                        return newList;
                    }
                }
                else if (Mem.Contains((valueTuple.digit, depth, valueTuple.outputZ)))
                {
                    continue;
                }
                else
                {
                    Mem.Add((valueTuple.digit, depth, valueTuple.outputZ));
                    var nextList = new List<(int digit, int outputZ)>(newList) { valueTuple };
                    var next = DepthFirstRec(depth + 1, nextList, findHighest);
                    if (next.Any() && next.Count == 14)
                    {
                        return next;
                    }
                }
            }

            return new List<(int, int)>();
        }

        // Handle each input phase --------------------------------

        private Dictionary<int, Phase> ParsePhases(IReadOnlyList<string> content)
        {
            var phases = new Dictionary<int,Phase>();

            var acc = new List<string>();
            var counter = 1;
            foreach (var line in content)
            {
                if (line.Contains("inp"))
                {
                    if (acc.Any())
                    {
                        var phase = new Phase(acc);
                        phases.Add(counter, phase);
                        counter++;

                    }

                    acc = new List<string>();
                }

                acc.Add(line);
            }
            phases.Add(counter, new Phase(acc));
            return phases;
        }

        /// <summary>
        /// Operations phase for single digit
        /// </summary>
        internal class Phase
        {
            private readonly List<string> _content;

            public Phase(List<string> content)
            {
                _content = content;
            }

            public IEnumerable<(int digit, int outputZ)> GetAllOutputsForZ(int inputZ, bool ascending)
            {
                if(ascending)
                {
                    for (int i = 9; i > 0; i--)
                    {
                        var z = CalculateOutputZ(i, inputZ);
                        yield return (i, z);
                    }
                }
                else
                {
                    for (int i = 1; i <= 9; i++)
                    {
                        var z = CalculateOutputZ(i, inputZ);
                        yield return (i, z);
                    }
                }
            }
            
            private int CalculateOutputZ(int inputDigit, int inputZ)
            {
                Dictionary<char, int> dict = new()
                {
                    { 'w', 0 },
                    { 'x', 0 },
                    { 'y', 0 },
                    { 'z', 0 },
                };
                dict['z'] = inputZ;
                foreach (var line in _content)
                {
                    OperateSet(line, inputDigit, dict);
                }

                return dict['z'];
            }

            private void OperateSet(string line, int input, Dictionary<char, int> dict)
            {
                var split = line.Split(' ');

                var a = new Variable(split[1]);
                Variable b = new Variable("0");
                if (split.Length > 2)
                {
                    b = new Variable(split[2]);
                }

                switch (split[0])
                {
                    case "inp":
                        dict[a.Name] = input;
                        break;
                    case "add":
                        if (b.IsDigit) dict[a.Name] += b.Digit;
                        else dict[a.Name] += dict[b.Name];
                        break;
                    case "mul":
                        if (b.IsDigit) dict[a.Name] *= b.Digit;
                        else dict[a.Name] *= dict[b.Name];
                        break;
                    case "div":
                        if (b.IsDigit)
                        {
                            if (b.Digit == 0) throw new DivideByZeroException();
                            dict[a.Name] /= b.Digit;
                        }
                        else
                        {
                            if (dict[b.Name] == 0) throw new DivideByZeroException();
                            dict[a.Name] /= dict[b.Name];
                        }
                        break;
                    case "mod":
                        if (b.IsDigit)
                        {
                            if (dict[a.Name] < 0 || b.Digit <= 0) throw new DivideByZeroException();
                            dict[a.Name] %= b.Digit;
                        }
                        else
                        {
                            if (dict[a.Name] < 0 || dict[b.Name] <= 0) throw new DivideByZeroException();
                            dict[a.Name] %= dict[b.Name];
                        }
                        break;
                    case "eql":
                        if (b.IsDigit)
                        {
                            if (dict[a.Name] == b.Digit) dict[a.Name] = 1;
                            else dict[a.Name] = 0;
                        }
                        else
                        {
                            if (dict[a.Name] == dict[b.Name]) dict[a.Name] = 1;
                            else dict[a.Name] = 0;
                        }
                        break;
                    default: throw new ArgumentException("Wrong command", nameof(line));
                }
            }
        }

        class Variable
        {
            public bool IsDigit { get; }
            public int Digit { get; }
            public char Name { get; }

            public Variable(string s)
            {
                if (int.TryParse(s, out var result))
                {
                    IsDigit = true;
                    Digit = result;
                }
                else
                {
                    Name = s[0];

                }
            }
        }
    }
}
