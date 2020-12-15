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

        protected DayBase(string dayNumber)
        {
            DayNumber = dayNumber;
            Content = FileSystem.GetInput(dayNumber);
        }

        public abstract int SolveA();

        public abstract int SolveB();
    }
}
