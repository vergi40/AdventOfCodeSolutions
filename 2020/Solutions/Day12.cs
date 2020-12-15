using System;
using System.Collections.Generic;

namespace Solutions
{
    class Day12 : DayBase
    {
        public Day12() : base("12")
        {
        }

        public override int SolveA()
        {
            var instructions = ParseContent();
            var ferry = new Ferry();

            foreach (var instruction in instructions)
            {
                if (instruction.dir == 'N' || instruction.dir == 'E' || instruction.dir == 'S' || instruction.dir == 'W')
                {
                    ferry.MoveToDirection(instruction);
                }
                else if (instruction.dir == 'L' || instruction.dir == 'R')
                {
                    ferry.Rotate(instruction);
                }
                else if (instruction.dir == 'F')
                {
                    ferry.MoveToDirection((ferry.Heading, instruction.amount));
                }
                else throw new ArgumentException();
            }

            return ferry.Distance;
        }

        private List<(char dir, int amount)> ParseContent()
        {
            var instructions = new List<(char, int)>();
            foreach (var line in Content)
            {
                var direction = line[0];
                var amount = int.Parse(line.Substring(1));
                instructions.Add((direction, amount));
            }

            return instructions;
        }

        class Ferry
        {
            public char Heading { get; set; } = 'E';
            private int North { get; set; } = 0;
            private int East { get; set; } = 0;
            public int Distance => Math.Abs(North) + Math.Abs(East);

            public WayPoint WayPoint { get; } = new WayPoint();

            public void MoveToDirection((char dir, int amount) data)
            {
                if (data.dir == 'N') North += data.amount;
                else if (data.dir == 'S') North -= data.amount;
                else if (data.dir == 'E') East += data.amount;
                else if (data.dir == 'W') East -= data.amount;
            }

            public void Rotate((char side, int amount) data)
            {
                var rotation = data.amount;
                if (data.side == 'R') rotation = 360 - data.amount;
                for (int i = 0; i < rotation; i = i + 90)
                {
                    Heading = GetNext(Heading);

                }
            }

            private char GetNext(char previous)
            {
                // Assume Z is upwards
                if (previous == 'E') return 'N';
                else if (previous == 'N') return 'W';
                else if (previous == 'W') return 'S';
                else if (previous == 'S') return 'E';
                else throw new ArgumentException();
            }

            public void MoveWayPoint((char dir, int amount) data)
            {
                if (data.dir == 'N') WayPoint.North += data.amount;
                else if (data.dir == 'S') WayPoint.North -= data.amount;
                else if (data.dir == 'E') WayPoint.East += data.amount;
                else if (data.dir == 'W') WayPoint.East -= data.amount;
            }

            public void RotateWayPoint((char side, int amount) data)
            {
                // Assume Z upwards. Rotate counter-clockwise
                var rotation = data.amount;
                if (data.side == 'R') rotation = 360 - data.amount;
                for (int i = 0; i < rotation; i = i + 90)
                {
                    //var next = GetNext(Heading);
                    var temp = WayPoint.North;
                    WayPoint.North = WayPoint.East;
                    WayPoint.East = temp * -1;
                }
            }

            public void MoveToWayPointDirection(int amount)
            {
                MoveToDirection(('N', WayPoint.North * amount));
                MoveToDirection(('E', WayPoint.East * amount));
            }
        }

        public override int SolveB()
        {
            var instructions = ParseContent();
            var ferry = new Ferry();

            foreach (var instruction in instructions)
            {
                if (instruction.dir == 'N' || instruction.dir == 'E' || instruction.dir == 'S' || instruction.dir == 'W')
                {
                    ferry.MoveWayPoint(instruction);
                }
                else if (instruction.dir == 'L' || instruction.dir == 'R')
                {
                    ferry.RotateWayPoint(instruction);
                }
                else if (instruction.dir == 'F')
                {
                    ferry.MoveToWayPointDirection(instruction.amount);
                }
                else throw new ArgumentException();
            }

            return ferry.Distance;
        }

        class WayPoint
        {
            public int North { get; set; } = 1;
            public int East { get; set; } = 10;


        }
    }
}
