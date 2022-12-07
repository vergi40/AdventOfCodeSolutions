using _2021_AoC;

namespace _2022_AoC
{
    internal class Day03 : DayBase
    {
        public Day03() : base("03") { }

        public override long SolveA()
        {
            var sum = 0;
            foreach (var line in Content)
            {
                var first = line.Substring(0, line.Length / 2);
                var second = line.Substring(line.Length / 2);

                foreach (var c in first)
                {
                    if (second.Contains(c))
                    {
                        sum += ConvertToInt(c);
                        break;
                    }
                }
            }
            return sum;
        }

        public override long SolveB()
        {
            var sum = 0;
            for (int groupIndex = 0; groupIndex < Content.Count; groupIndex += 3)
            {
                var group = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    var line = Content[i + groupIndex];
                    group.Add(line);
                }

                sum += FindCommonItem(group);
            }

            return sum;
        }

        private int FindCommonItem(List<string> lines)
        {
            var first = lines.First();
            var dist = first.Distinct();

            foreach (var c in dist)
            {
                if (lines[1].Contains(c) && lines[2].Contains(c))
                {
                    return ConvertToInt(c);
                }
            }

            throw new ArgumentException();
        }

        private int ConvertToInt(char alphabet)
        {
            if (char.IsLower(alphabet))
            {
                return (int)alphabet - 96;
            }
            else
            {
                return (int)alphabet - 38;
            }
        }
    }
}