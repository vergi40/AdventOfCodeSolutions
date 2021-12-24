using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace _2021_AoC
{
    internal class Day24 : DayBase
    {
        public Day24() : base("24")
        {
        }

        private Dictionary<char, int> Dict { get; set; } = new()
        {
            { 'w', 0 },
            { 'x', 0 },
            { 'y', 0 },
            { 'z', 0 },
        };

        private Queue<int> Input { get; set; } = new Queue<int>();
        internal const int SearchBase = 5_000;


        public override long SolveA()
        {
            // We want z to be 0. Let's backtrack
            // Solve all inputs or 14th digit that result z = 0
            // Use them to solve all inputs for 13th digit
            // etc

            Console.WriteLine($"Starting backwards run to figure out all possible digits. Constants: SearchBase = {SearchBase}");
            var phases = ParsePhases(Content);

            var totalWatch = new Stopwatch();
            totalWatch.Start();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var digit14 = phases[14].GetValidInputsWithAnyZ(0).ToList();
            //PrintPossibilities(digit14, 14, stopWatch);
            Console.WriteLine($"Phase {14} complete. Digit possibilities: {digit14.Count}. Total time {totalWatch.Elapsed.ToString()}");


                // --------------
            var allPossibilities = new List<List<(int inputDigit, int inputZ)>>();
            allPossibilities.Add(digit14);

            List<(int inputDigit, int inputZ)> digitPrevious = digit14;
            for (int phaseLevel = 13; phaseLevel > 0; phaseLevel--)
            {
                List<(int inputDigit, int inputZ)> digitN = new();
                foreach (var (inputDigit, inputZ) in digitPrevious)
                {
                    var temp = phases[phaseLevel].GetValidInputsWithAnyZ(inputZ).ToList();
                    if(temp.Any())
                    {
                        //PrintPossibilities(temp, phaseLevel, inputZ);
                    }
                    digitN.AddRange(temp);
                }
                Console.WriteLine($"Phase {phaseLevel} complete. Digit possibilities: {digitN.Count}. Total time {totalWatch.Elapsed.ToString()}");
                ResetWatch(stopWatch);

                digitN = digitN.Distinct().ToList();
                allPossibilities.Add(digitN);
                digitPrevious = digitN;
            }

            if (!allPossibilities.Last().Any())
            {
                Console.WriteLine("Didn't receive digits for all indexes.");
                return 0;
            }

            // Build full numbers
            var resultList = new List<string>();
            foreach (var first in allPossibilities.Last())
            {
                var full = "";
                var digit = first.inputDigit;
                full += digit.ToString();

                var index = 2;
                while (index <= 14)
                {
                    var next = allPossibilities[index].Find(pair => pair.inputDigit == digit);
                    digit = next.inputDigit;
                    full += digit.ToString();
                    index++;
                }

                Console.WriteLine($"Found result number: {full}");
                resultList.Add(full);
            }

            resultList.Sort();
            foreach (var result in resultList)
            {
                Console.WriteLine($"{result}");
            }
            return 0;


            var start = 11111111111111;
            var iMax = 99999999999999;
            var incr = 1;


            //long first = 13579246899999;
            for (long i = start; i < iMax; i += incr)
            {
                var s = i.ToString();
                if (s.Contains('0')) continue;
                //var reverse = Reverse(i.ToString());
                //var revL = long.Parse(reverse);

                var tail = s.Substring(s.Length - 2);
                if (tail == "79")
                {
                    //Good
                }
                else continue;

                var subTail = s.Substring(s.Length - 4, 2);
                if (subTail[0] - subTail[1] == 1)
                {
                    //Good
                }
                else continue;

                // ----------
                var input = i;
                //if (input % 10000000 == 0)
                //{
                //    Console.WriteLine($"Currently looping: {i}");
                //}
                try
                {
                    //var result = DebugIsValid(i);
                    var result = IsValid(input);
                    if (result)
                    {
                        return input;
                    }

                    // Debugging rules
                    Console.WriteLine($"Input: {input}. w {Dict['w'],-5}, x {Dict['x'],-5}, y {Dict['y'],-5}, z {Dict['z']}");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"{input} threw exception {e.Message}");
                }
            }
            return 0;
        }
        private void PrintPossibilities(List<(int inputDigit, int inputZ)> inputs, int digit, int targetZ)
        {
            Console.WriteLine($"Possibilities for digit on location {digit} - target output Z [{targetZ}]:");
            foreach (var valueTuple in inputs)
            {
                Console.WriteLine($"Digit [{valueTuple.inputDigit}]: with input value z of {valueTuple.inputZ}");
            }

            Console.WriteLine();
        }
        private void PrintPossibilities(List<(int inputDigit, int inputZ)> inputs, int digit, Stopwatch watch)
        {
            Console.WriteLine($"Possibilities for digit on location {digit}:");
            foreach (var valueTuple in inputs)
            {
                Console.WriteLine($"Digit [{valueTuple.inputDigit}]: with input value z of {valueTuple.inputZ}");
            }

            ResetWatch(watch);
            Console.WriteLine();
        }

        private void ResetWatch(Stopwatch watch)
        {
            //Console.WriteLine($"Time took: {watch.Elapsed.Minutes} min, {watch.Elapsed.Seconds} sec, {watch.Elapsed.Milliseconds} ms");
            watch.Restart();
        }

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
            private int SearchWidth
            {
                get
                {
                    if (Mod26) return SearchBase * 2;
                    return SearchBase;
                }
            }

            private readonly List<string> _content;

            private bool Mod26 { get; }


            public Phase(List<string> content)
            {
                _content = content;

                if (_content.Contains("mod x 26") && _content.Contains("div z 26"))
                {
                    // 
                    Mod26 = true;
                }
            }

            public IEnumerable<(int inputDigit, int inputZ)> GetValidInputsWithAnyZ(int outputZ)
            {
                var concurrentCollection = new ConcurrentBag<(int inputDigit, int inputZ)>();

                for (int i = 1; i < 10; i++)
                {
                    //for (int localZ = 0; localZ < SearchWidth; localZ++)
                    //{
                    //    var result = OperateSingleSet(i, localZ, outputZ);
                    //    temp.AddRange(result);
                    //}

                    Parallel.For(0, SearchWidth, localZ =>
                    {
                        var resultList = OperateSingleSet(i, localZ, outputZ);
                        foreach (var valueTuple in resultList)
                        {
                            concurrentCollection.Add(valueTuple);
                        }
                    });
                }

                return concurrentCollection;
            }

            private IEnumerable<(int inputDigit, int inputZ)> OperateSingleSet(int inputDigit, int localZ, int goalOutputZ)
            {
                if (Mod26 && !HiddenFilterOk(localZ)) yield break;

                Dictionary<char, int> dict = new()
                {
                    { 'w', 0 },
                    { 'x', 0 },
                    { 'y', 0 },
                    { 'z', 0 },
                };
                dict['z'] = localZ;
                foreach (var line in _content)
                {
                    OperateSet(line, inputDigit, dict);
                }

                if (dict['z'] == goalOutputZ) yield return (inputDigit, localZ);
            }

            private bool HiddenFilterOk(int z)
            {
                if (z < 26) return true;

                var mod = z % 26;
                if (mod >= 7 && mod <= 20) return true;
                return false;

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

        // https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
        public string Reverse(string text)
        {
            if (text == null) return null;

            // this was posted by petebob as well 
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }

        private bool IsValid(long input)
        {
            Input = new Queue<int>();
            Dict = new()
            {
                { 'w', 0 },
                { 'x', 0 },
                { 'y', 0 },
                { 'z', 0 },
            };

            var s = Convert.ToString(input);
            foreach (var c in s)
            {
                Input.Enqueue(int.Parse(c.ToString()));
            }

            foreach (var line in Content)
            {
                OperateSingleNumber(line);
            }

            var result = Dict['z'] == 0;
            return result;
        }

        private bool DebugIsValid(long input)
        {
            Input = new Queue<int>();
            Dict  = new()
            {
                { 'w', 0 },
                { 'x', 0 },
                { 'y', 0 },
                { 'z', 0 },
            };

            var s = Convert.ToString(input);
            foreach (var c in s)
            {
                Input.Enqueue(int.Parse(c.ToString()));
            }

            Console.WriteLine($"Testing input {input}");
            foreach (var line in Content)
            {
                Console.Write($"{line,-10}");
                Console.Write($"{OperateSingleNumber(line)}");
                Console.Write(Environment.NewLine);
            }

            var result = Dict['z'] == 0;
            Console.WriteLine($"Result: {result}");
            return result;
        }

        private string OperateSingleNumber(string line)
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
                    Dict[a.Name] = Input.Dequeue();
                    break;
                case "add":
                    if (b.IsDigit) Dict[a.Name] += b.Digit;
                    else Dict[a.Name] += Dict[b.Name];
                    break;
                case "mul":
                    if (b.IsDigit) Dict[a.Name] *= b.Digit;
                    else Dict[a.Name] *= Dict[b.Name];
                    break;
                case "div":
                    if (b.IsDigit)
                    {
                        if (b.Digit == 0) throw new DivideByZeroException();
                        Dict[a.Name] /= b.Digit;
                    }
                    else
                    {
                        if (Dict[b.Name] == 0) throw new DivideByZeroException();
                        Dict[a.Name] /= Dict[b.Name];
                    }
                    break;
                case "mod":
                    if (b.IsDigit)
                    {
                        if (Dict[a.Name] < 0 || b.Digit <= 0) throw new DivideByZeroException();
                        Dict[a.Name] %= b.Digit;
                    }
                    else
                    {
                        if (Dict[a.Name] < 0 || Dict[b.Name] <= 0) throw new DivideByZeroException();
                        Dict[a.Name] %= Dict[b.Name];
                    }
                    break;
                case "eql":
                    if (b.IsDigit)
                    {
                        if (Dict[a.Name] == b.Digit) Dict[a.Name] = 1;
                        else Dict[a.Name] = 0;
                    }
                    else
                    {
                        if (Dict[a.Name] == Dict[b.Name]) Dict[a.Name] = 1;
                        else Dict[a.Name] = 0;
                    }
                    break;
                default: throw new ArgumentException("Wrong command", nameof(line));
            }

            return $"- Variable {a.Name} new value: {Dict[a.Name]}";
        }

        public override long SolveB()
        {
            return 0;
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
