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
            var content = FileSystem.GetInputData(DayNumber);
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
            return Content.Select(s => long.Parse(s)).ToList();
        }

        protected int ParseLineToInt(string line)
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
        protected char[,] GetInputAsChar2D()
        {
            var width = Content.First().Length;
            var height = Content.Count();

            var array = new char[width, height];
            for(int i = 0; i < height; i++)
            {
                var arrayIndex = height - i - 1;
                var line = Content[i];

                for(int j = 0; j < width; j++)
                {
                    array[j, arrayIndex] = line[j];
                }
            }

            return array;
        }
    }
}
