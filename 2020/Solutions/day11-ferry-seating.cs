using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    class Day11: DayBase
    {
        public Day11() : base("11") { }
        public override long SolveA()
        {
            var area = new SeatArea(Content, 4);
            var hasChanges = true;
            var totalOccupied = 0;

            while (hasChanges)
            {
                //
                var result = area.ApplyRound(false);
                totalOccupied = result.totalOccupied;
                hasChanges = result.stateChanges > 0;
            }

            return totalOccupied;
        }

        public override long SolveB()
        {
            var area = new SeatArea(Content, 5);
            var hasChanges = true;
            var totalOccupied = 0;

            while (hasChanges)
            {
                //
                var result = area.ApplyRound(true);
                totalOccupied = result.totalOccupied;
                hasChanges = result.stateChanges > 0;
            }

            return totalOccupied;
        }


        class SeatArea
        {
            /// <summary>
            /// Column, row
            /// </summary>
            public Seat[,] Seats { get; set; }

            public int Columns { get; }
            public int Rows { get; }
            public int LimitToEmptySeat { get; }

            public SeatArea(List<string> input, int limitToEmpty)
            {
                LimitToEmptySeat = limitToEmpty;
                Columns = input.First().Length;
                Rows = input.Count;
                Seats = new Seat[Columns, Rows];

                input.Reverse();
                for (int i = 0; i < input.Count; i++)
                {
                    var row = input[i];
                    for (int j = 0; j < row.Length; j++)
                    {
                        Seats[j, i] = new Seat(row[j]);
                    }
                }
            }

            public (int stateChanges, int totalOccupied) ApplyRound(bool checkTillOut)
            {
                var newSeats = new Seat[Columns, Rows];
                var stateChanges = 0;
                var totalOccupied = 0;

                for (int i = 0; i < Columns; i++)
                {
                    for (int j = 0; j < Rows; j++)
                    {
                        var seat = Seats[i, j];
                        if (seat.State == '.')
                        {
                            newSeats[i, j] = new Seat('.');
                            continue;
                        }

                        int occupied;
                        if (checkTillOut) occupied = seat.OccupiedAround(this, i, j);
                        else occupied = seat.OccupiedAroundOld(this, i, j);

                        if (seat.State == 'L')
                        {
                            if (occupied == 0)
                            {
                                newSeats[i, j] = new Seat('#');
                                stateChanges++;
                                totalOccupied++;
                            }
                            else
                            {
                                newSeats[i, j] = new Seat('L');
                            }
                        }
                        else
                        {
                            if (occupied >= LimitToEmptySeat)
                            {
                                newSeats[i, j] = new Seat('L');
                                stateChanges++;
                            }
                            else
                            {
                                newSeats[i, j] = new Seat('#');
                                totalOccupied++;
                            }
                        }
                    }
                }

                Seats = newSeats;
                return (stateChanges, totalOccupied);
            }
        }

        class Seat
        {
            public char State { get; set; }

            public Seat(char state)
            {
                State = state;
            }

            private char GetState(SeatArea area, int column, int row)
            {
                if (column < 0 || column > area.Columns - 1) return '.';
                if (row < 0 || row > area.Rows - 1) return '.';
                return area.Seats[column, row].State;
            }

            private bool IsOutside(SeatArea area, int column, int row)
            {
                if (column < 0 || column > area.Columns - 1) return true;
                if (row < 0 || row > area.Rows - 1) return true;
                return false;
            }

            private char GetStateUntilOccupiedOrOut(SeatArea area, int columnDirection, int rowDirection, int column, int row)
            {
                var i = 1;
                while (true)
                {
                    var nextColumn = column + i * columnDirection;
                    var nextRow = row + i * rowDirection;
                    if (IsOutside(area, nextColumn, nextRow)) return 'o';

                    var state = GetState(area, nextColumn, nextRow);
                    if (state != '.') return state;
                    i++;
                }
            }

            public int OccupiedAroundOld(SeatArea area, int column, int row)
            {
                var count = 0;
                if (GetState(area, column - 1, row) == '#') count++;
                if (GetState(area, column - 1, row - 1) == '#') count++;
                if (GetState(area, column - 1, row + 1) == '#') count++;
                if (GetState(area, column, row - 1) == '#') count++;
                if (GetState(area, column, row + 1) == '#') count++;
                if (GetState(area, column + 1, row) == '#') count++;
                if (GetState(area, column + 1, row - 1) == '#') count++;
                if (GetState(area, column + 1, row + 1) == '#') count++;
                return count;
            }

            public int OccupiedAround(SeatArea area, int column, int row)
            {
                var count = 0;
                if (GetStateUntilOccupiedOrOut(area, -1, 1, column, row) == '#') count++;
                if (GetStateUntilOccupiedOrOut(area, -1, 0, column, row) == '#') count++;
                if (GetStateUntilOccupiedOrOut(area, -1, -1, column, row) == '#') count++;
                if (GetStateUntilOccupiedOrOut(area, 1, 1, column, row) == '#') count++;
                if (GetStateUntilOccupiedOrOut(area, 1, 0, column, row) == '#') count++;
                if (GetStateUntilOccupiedOrOut(area, 1, -1, column, row) == '#') count++;
                if (GetStateUntilOccupiedOrOut(area, 0, 1, column, row) == '#') count++;
                if (GetStateUntilOccupiedOrOut(area, 0, -1, column, row) == '#') count++;
                return count;
            }
        }
    }
}
