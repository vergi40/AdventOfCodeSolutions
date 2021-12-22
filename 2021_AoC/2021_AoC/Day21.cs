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

        public override long SolveB()
        {
            var p1Pos = 8;
            var p2Pos = 3;
            //var p1Pos = 4;
            //var p2Pos = 8;

            var p1Score = 0;
            var p2Score = 0;
            var dice = 0;


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

        // 3 rolls [1,2,3]
        // 1,1,1 1,1,2 1,1,3
        // 1,2,1 1,2,2 1,2,3
        // 1,3,1 1,3,2 1,3,3
        // all starting number 1: 3,4,5 4,5,6 5,6,7
        // aka 3,4,4,5,5,5,6,6,7
        // all starting number 2 and 3: +1 and +2
        private List<int> QuantumRolls { get; } = new List<int>
        {
            3, 4, 4, 5, 5, 5, 6, 6, 7,
            4, 5, 5, 6, 6, 6, 7, 7, 8,
            5, 6, 6, 7, 7, 7, 8, 8, 9
        };

        private List<int> CalculateNextScore(int currentScore)
        {
            var playerPos = currentScore % 10;
            if (playerPos == 0) playerPos = 10;

            foreach (var quantumRoll in QuantumRolls)
            {
                var nPos = playerPos + quantumRoll;
            }

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
    }
}
