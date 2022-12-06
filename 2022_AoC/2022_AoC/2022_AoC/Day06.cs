using _2021_AoC;

namespace _2022_AoC
{
    internal class Day06 : DayBase
    {
        public Day06() : base("06")
        {
        }

        public override long SolveA()
        {
            return GetDistinctCharsIndex(0, 4);
        }

        public override long SolveB()
        {
            var packetStart = GetDistinctCharsIndex(0, 4);
            return GetDistinctCharsIndex(packetStart, 14);
        }

        private int GetDistinctCharsIndex(int start, int distinctCount)
        {
            var line = Content.First();
            for (int i = start; i < line.Length - distinctCount; i++)
            {
                var set = new HashSet<char>();
                var next = line.Substring(i, distinctCount);
                set.Add(next[0]);

                var success = true;
                for (int j = 1; j < distinctCount; j++)
                {
                    if (!set.Add(next[j]))
                    {
                        success = false;
                        break;
                    }
                }
                if(success) return i + distinctCount;
            }

            return 0;
        }
    }
}
