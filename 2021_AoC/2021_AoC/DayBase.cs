using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal abstract class DayBase
    {
        public string DayNumber { get; }
        public List<string> Content { get; private set; } = new List<string>();

        protected DayBase(string dayNumberAsString)
        {
            DayNumber = dayNumberAsString;
        }

        public void Initialize()
        {
            InitializeBase();
            InitializeChild();
        }

        private void InitializeBase()
        {
            var content = FileSystem.GetInput(DayNumber);
            if(content != null) Content = content;
        }

        protected virtual void InitializeChild()
        {
            // Override if needed
        }

        public abstract long SolveA();

        public abstract long SolveB();



        // Helper methods
        protected long ParseLineToLong(string line)
        {
            return long.Parse(line);
        }

        protected List<long> GetInputAsLongList()
        {
            var result = new List<long>();
            foreach(var line in Content)
            {
                result.Add(ParseLineToLong(line));
            }

            return result;
        }

        protected int ParseLineToInt(string line)
        {
            return int.Parse(line);
        }

        protected List<int> GetInputAsIntList()
        {
            var result = new List<int>();
            foreach (var line in Content)
            {
                result.Add(ParseLineToInt(line));
            }

            return result;
        }
    }
}
