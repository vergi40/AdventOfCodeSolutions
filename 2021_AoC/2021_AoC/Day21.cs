using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day21 : DayBase
    {
        public Day21() : base("21")
        {
        }

        private int totalDice = 0;

        public override long SolveA()
        {
            var p1Pos = 8;
            var p2Pos = 3;
            //var p1Pos = 4;
            //var p2Pos = 8;

            var p1Score = 0;
            var p2Score = 0;
            var dice = 0;

            while (true)
            {
                var p1Space = CalculateSpace(ref dice, ref p1Pos);
                p1Score += p1Space;

                if (p1Score >= 1000)
                {
                    Console.WriteLine($"P1 wins with {p1Score}");
                    Console.WriteLine($"Answer {totalDice * p2Score}");
                    break;
                }

                var p2Space = CalculateSpace(ref dice, ref p2Pos);
                p2Score += p2Space;

                if (p2Score >= 1000)
                {
                    Console.WriteLine($"P2 wins with {p2Score}");
                    Console.WriteLine($"Answer {totalDice * p1Score}");
                    break;
                }
            }
            return 0;
        }

        private int CalculateSpace(ref int dice, ref int playerPos)
        {
            var space = playerPos;
            for (int i = 0; i < 3; i++)
            {
                dice++;
                totalDice++;
                dice = dice % 100;
                if (dice == 0) dice = 100;
                space += dice;
            }

            space = space % 10;
            if (space == 0) space = 10;
            playerPos = space;
            return space;
        }

        public override long SolveB()
        {
            return 0;
        }
    }
}
