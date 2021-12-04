using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day04 : DayBase
    {
        private List<BingoBoard> _resultBoards = new List<BingoBoard>();

        public Day04() : base("04")
        {
        }

        public override long SolveA()
        {
            var content = new Queue<string>(Content);
            var winNumbersRaw = content.Dequeue();
            var winNumbers = winNumbersRaw.Split(",").ToList();
            content.Dequeue();

            var boards = new List<BingoBoard>();

            while (content.Any())
            {
                var boardInput = new List<string>();
                while (content.Any())
                {
                    var line = content.Dequeue();
                    if (string.IsNullOrEmpty(line)) break;
                    boardInput.Add(line);
                }

                boards.Add(new BingoBoard(boardInput));
            }

            // GAme on!
            for(int i = 0; i < winNumbers.Count; i++)
            {
                var next = int.Parse(winNumbers[i]);
                foreach (var board in boards)
                {
                    if (board.IsBingo) continue;

                    board.PlayNext(next);
                    if (board.IsBingo)
                    {
                        var finalScore = board.CalculateFinalScore(next);
                        board.BingoIndex = i;
                    }
                }
            }

            var bingoed = boards.Where(b => b.IsBingo);
            _resultBoards = bingoed.ToList();

            var first = bingoed.OrderBy(b => b.BingoIndex).First();
            return first.FinalScore;

        }

        public override long SolveB()
        {
            var last = _resultBoards.OrderByDescending(b => b.BingoIndex).First();
            return last.FinalScore;
        }

        class BingoBoard
        {
            public bool IsBingo { get; private set; }
            private int[,] _board { get; } = new int[5, 5];
            private bool[,] _checked { get; } = new bool[5, 5];

            public int BingoIndex { get; set; } = -1;
            public long FinalScore { get; set; } = -1;

            // Unnecessary optimization
            private HashSet<int> _numberSet = new HashSet<int>();
            
            public BingoBoard(List<string> input)
            {
                for(int i = 0; i < 5; i++)
                {
                    var line = input[i];
                    var lineList = line.Split(' ', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    for(int j = 0; j < 5; j++)
                    {
                        _board[i,j] = int.Parse(lineList[j]);
                    }
                }

                Initialize();
            }

            internal void Initialize()
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        _numberSet.Add(_board[i, j]);
                    }
                }
            }

            internal void PlayNext(int next)
            {
                if (!_numberSet.Contains(next)) return;

                for(int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if(_board[i,j] == next)
                        {
                            _checked[i, j] = true;
                            IsBingo = CheckBingo();
                            if (IsBingo)
                            {
                                FinalScore = CalculateFinalScore(next);
                            }
                        }
                    }
                }
            }

            private bool CheckBingo()
            {
                for(int i = 0; i < 5; i++)
                {
                    // hor
                    if (_checked[i, 0] && _checked[i, 1] && _checked[i, 2] && _checked[i, 3] && _checked[i, 4])
                    {
                        return true;
                    }

                    // ver
                    if (_checked[0, i] && _checked[1, i] && _checked[2, i] && _checked[3, i] && _checked[4, i])
                    {
                        return true;
                    }
                }


                // cross
                if (_checked[0,0] && _checked[1, 1] && _checked[2, 2] && _checked[3, 3] && _checked[4, 4])
                {
                    return true;
                }
                if (_checked[0, 4] && _checked[1, 3] && _checked[2, 2] && _checked[3, 1] && _checked[4, 0])
                {
                    return true;
                }

                return false;
            }

            internal long CalculateFinalScore(int next)
            {
                var sum = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (!_checked[i, j])
                        {
                            sum += _board[i, j];
                        }
                    }
                }

                return sum * next;
            }
            
        }

        
    }
}
