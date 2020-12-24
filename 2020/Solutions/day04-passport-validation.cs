using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    class Day04 : DayBase
    {
        public Day04 () : base("04") { }
        public override long SolveA()
        {
            var list = new List<Passport>();
            var current = new Passport();
            foreach (var line in Content)
            {
                if (string.IsNullOrEmpty(line))
                {
                    list.Add(current);
                    current = new Passport();
                    continue;
                }

                var entries = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                current.AddPairs(entries);
            }
            list.Add(current);
            return list.Count(p => p.IsValid());
        }

        class Passport
        {
            public Dictionary<string, string> Data { get; } = new Dictionary<string, string>();

            public void AddPairs(string[] entries)
            {
                foreach (var entry in entries)
                {
                    var pair = entry.Split(':');
                    Data.Add(pair[0], pair[1]);
                }
            }

            public bool IsValid()
            {
                var required = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
                foreach (var field in required)
                {
                    if (!Data.ContainsKey(field)) return false;
                }

                return true;
            }
        }

        public override long SolveB()
        {
            return 0;
        }
    }
}
