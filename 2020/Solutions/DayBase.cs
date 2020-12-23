using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Solutions
{
    abstract class DayBase
    {
        public string DayNumber { get; }
        public List<string> Content { get; }

        protected DayBase(string dayNumberAsString)
        {
            DayNumber = dayNumberAsString;
            Content = FileSystem.GetInput(dayNumberAsString);
        }

        public abstract long SolveA();

        public abstract long SolveB();
    }
}
