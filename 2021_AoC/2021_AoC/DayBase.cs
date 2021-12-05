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
        private List<string> _content { get; set; } = new List<string>();
        public IReadOnlyList<string> Content => _content;

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
            var content = FileSystem.GetInputData(DayNumber);
            if(content != null) _content = content;
        }

        protected virtual void InitializeChild()
        {
            // Override if needed
        }

        public abstract long SolveA();

        public abstract long SolveB();



        // Helper methods
        protected static long ParseLineToLong(string line)
        {
            return long.Parse(line);
        }

        protected List<long> GetInputAsLongList()
        {
            return Content.Select(s => long.Parse(s)).ToList();
        }

        protected static int ParseLineToInt(string line)
        {
            return int.Parse(line);
        }

        protected List<int> GetInputAsIntList()
        {
            return Content.Select(s => int.Parse(s)).ToList();
        }

        /// <summary>
        /// Array [width][height] or [x][y].
        /// Assumes that line length is constant
        /// Y
        /// ^
        /// |
        /// |
        /// |
        /// o---------> X
        /// </summary>
        /// <returns></returns>
        protected static char[,] ParseStringListToChar2D(List<string> list)
        {
            var width = list.First().Length;
            var height = list.Count();

            var array = new char[width, height];
            for (int i = 0; i < height; i++)
            {
                var arrayIndex = height - i - 1;
                var line = list[i];

                for (int j = 0; j < width; j++)
                {
                    array[j, arrayIndex] = line[j];
                }
            }

            return array;
        }

        /// <summary>
        /// Array [width][height] or [x][y].
        /// Assumes that line length is constant
        /// Y
        /// ^
        /// |
        /// |
        /// |
        /// o---------> X
        /// </summary>
        /// <returns></returns>
        protected static int[,] ParseStringListToInt2D(List<string> list)
        {
            var width = list.First().Length;
            var height = list.Count();

            var array = new int[width, height];
            for (int i = 0; i < height; i++)
            {
                var arrayIndex = height - i - 1;
                var line = list[i];

                for (int j = 0; j < width; j++)
                {
                    array[j, arrayIndex] = int.Parse(line[j].ToString());
                }
            }

            return array;
        }
    }
}
