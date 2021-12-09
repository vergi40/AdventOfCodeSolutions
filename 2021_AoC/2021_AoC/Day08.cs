namespace _2021_AoC
{
    internal class Day08 : DayBase
    {
        public Day08() : base("08")
        {
            // 4 digits. e.g. "1234"
            // 7 segments for each digit - abcdefg
        }

        public override long SolveA()
        {
            // beacf afbd bcead cgefa ecdbga efb gbfdeac ecgfbd acbdfe fb | bf efb bgecdfa egcfa
            // ------------------signal patterns-------------------------   ----output value----
            var times = 0;
            var targetLength = new[] { 2, 3, 4, 7 };
            foreach(var line in Content)
            {
                var outputValues = line.Split(" | ")[1].Split(" ");
                foreach (var value in outputValues)
                {
                    foreach (var length in targetLength)
                    {
                        if (value.Length == length) times++;
                    }
                }
            }

            return times;
        }

        public override long SolveB()
        {
            long sum = 0;

            foreach (var line in Content)
            {
                var output = line.Split(" | ");
                var signalValues = output[0].Split(" ").ToList();
                var outputValues = output[1].Split(" ").ToList();

                var connection = new Connection(signalValues);
                sum += connection.Solve(outputValues);
            }

            return sum;
        }

        class Connection
        {
            private Dictionary<int, string> digits = new();
            private List<string> fives = new();
            private List<string> sixes = new();

            public Connection(List<string> start)
            {
                foreach (var pattern in start)
                {
                    switch (pattern.Length)
                    {
                        case 2: 
                            digits.Add(1, pattern);
                            break;
                        case 3: 
                            digits.Add(7, pattern);
                            break;
                        case 4: 
                            digits.Add(4, pattern);
                            break;
                        case 7: 
                            digits.Add(8, pattern);
                            break;
                        case 5:
                            fives.Add(pattern);
                            break;
                        case 6:
                        default:
                            sixes.Add(pattern);
                            break;
                    }
                }

            }

            public int Solve(List<string> end)
            {

                // fives: 2 3 5
                var digit3 = fives.Find(s => ContainsSubSet(s, digits[1]));
                fives.Remove(digit3);
                var digit5 = fives.Find(s => ContainsSubSetWithWildcards(s, digits[4]));
                fives.Remove(digit5);
                var digit2 = fives.Single();

                // sixes: 0 6 9
                var digit9 = sixes.Find(s => ContainsSubSet(s, digits[4]));
                sixes.Remove(digit9);
                var digit0 = sixes.Find(s => ContainsSubSet(s, digits[7]));
                sixes.Remove(digit0);
                var digit6 = sixes.Single();

                var all = digits.Select(d => (d.Value, d.Key)).ToList();
                all.AddRange(new List<(string, int)>
                {
                    (digit0, 0), (digit2, 2), (digit3, 3),
                    (digit5, 5), (digit6, 6), (digit9, 9)
                });

                // Sort each string
                for (int i = 0; i < all.Count; i++)
                {

                    all[i] = (SortString(all[i].Value), all[i].Key);
                }

                for (int i = 0; i < end.Count; i++)
                {
                    end[i] = SortString(end[i]);
                }

                var result = "";
                foreach (var stringPresentation in end)
                {
                    var integer = all.Find(d => d.Value == stringPresentation).Key;
                    result += integer.ToString();
                }

                return int.Parse(result);
            }

            private bool ContainsSubSet(string set1, string set2)
            {
                foreach (var digit in set2)
                {
                    if (!set1.Contains(digit)) return false;
                }

                return true;
            }

            private bool ContainsSubSetWithWildcards(string set1, string set2, int wildcards = 1)
            {
                foreach (var digit in set2)
                {
                    if (!set1.Contains(digit))
                    {
                        if (wildcards > 0)
                        {
                            wildcards--;
                            continue;
                        }
                        return false;
                    }
                }

                return true;
            }

            private string SortString(string input)
            {
                var sortedArray = input.ToArray();
                Array.Sort(sortedArray);
                return new string(sortedArray);
            }
        }
        
    }
}
