using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day02 : DayBase
    {
        public Day02() : base("02")
        {
        }

        public override long SolveA()
        {
            var instructions = new List<Instruction>();
            foreach(var line in Content)
            {
                instructions.Add(new Instruction(line));
            }

            var depth = 0;
            var horizontal = 0;
            foreach(var instruction in instructions)
            {
                if (instruction.Direction == "down") depth += instruction.Amount;
                else if (instruction.Direction == "up") depth -= instruction.Amount;
                else if (instruction.Direction == "forward") horizontal += instruction.Amount;
                else throw new ArgumentException("Unknown command");
            }


            return depth * horizontal;
        }

        public override long SolveB()
        {
            var instructions = new List<Instruction>();
            foreach (var line in Content)
            {
                instructions.Add(new Instruction(line));
            }

            var depth = 0;
            var horizontal = 0;
            var aim = 0;
            foreach (var instruction in instructions)
            {
                if (instruction.Direction == "down") aim += instruction.Amount;
                else if (instruction.Direction == "up") aim -= instruction.Amount;
                else if (instruction.Direction == "forward")
                {
                    horizontal += instruction.Amount;
                    depth += (aim * instruction.Amount);
                }
                else throw new ArgumentException("Unknown command");
            }


            return depth * horizontal;
        }

        record Instruction
        {
            public string Direction { get; }
            public int Amount { get; }
            public Instruction(string line)
            {
                var data = line.Split(' ');
                Direction = data[0];
                Amount = int.Parse(data[1]);
            }
        }
    }
}
