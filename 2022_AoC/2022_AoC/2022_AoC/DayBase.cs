using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2022_AoC;

namespace _2021_AoC
{
    internal abstract class DayBase
    {
        public string DayNumber { get; }
        private List<string> _content { get; set; } = new List<string>();

        /// <summary>
        /// Challenge-specific input as list of string lines
        /// </summary>
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
            var content = Files.GetInputData(DayNumber);
            if (content != null) _content = content;
        }

        protected virtual void InitializeChild()
        {
            // Override if needed
        }

        public abstract long SolveA();

        public abstract long SolveB();

        public virtual long SolveAF()
        {
            return 0;
        }
        public virtual long SolveBF()
        {
            return 0;
        }

        // Helper methods
        protected static long ParseLineToLong(string line)
        {
            return long.Parse(line);
        }

        protected IReadOnlyList<long> GetInputAsLongList()
        {
            return Content.Select(s => long.Parse(s)).ToList();
        }

        protected static int ParseLineToInt(string line)
        {
            return int.Parse(line);
        }

        protected IReadOnlyList<int> GetInputAsIntList()
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
        protected static char[,] ParseStringListToChar2D(IReadOnlyList<string> list)
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
        protected static int[,] ParseStringListToInt2D(IReadOnlyList<string> list)
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

        /// <summary>
        /// Creates perimeter around 2D array for easier edge value checking
        ///
        /// -1 -1 -1 -1
        /// -1  0  0 -1
        /// -1  0  0 -1
        /// -1 -1 -1 -1
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="perimeterValue"></param>
        /// <returns></returns>
        protected int[,] Int2DToWrappedInt2D(int[,] original, int perimeterValue = -1)
        {
            var (width, height) = Get2DMeasures(original);

            var array = new int[width + 2, height + 2];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array[i + 1, j + 1] = original[i, j];
                }
            }

            // Edges
            for (int i = 0; i < width + 2; i++)
            {
                array[i, 0] = perimeterValue;
                array[i, height + 1] = perimeterValue;
            }
            for (int i = 0; i < height + 2; i++)
            {
                array[0, i] = perimeterValue;
                array[width + 1, i] = perimeterValue;
            }

            return array;
        }

        protected (int width, int height) Get2DMeasures(int[,] array) => (array.GetLength(0), array.GetLength(1));
    }
}
