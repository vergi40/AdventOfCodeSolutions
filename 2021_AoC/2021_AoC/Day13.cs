using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day13:DayBase
    {
        public Day13() : base("13")
        {
        }

        public override long SolveA()
        {
            var index = Content.ToList().FindIndex(string.IsNullOrWhiteSpace);
            var set = new HashSet<(int,int)>();

            var instructions = Content.ToList().GetRange(0, index);
            var splits = Content.ToList().GetRange(index + 1, Content.Count - index - 1);
            foreach (var line in instructions)
            {
                var xy = line.Split(',').Select(int.Parse).ToList();
                set.Add((xy[0], xy[1]));
            }

            // Split at y=655
            // 653      o
            // 654 o
            // 655 ------------
            // 656 #
            // 657      #   
            var split = 655;
            var newSet = new HashSet<(int, int)>();
            var deleteSet = new HashSet<(int, int)>();
            foreach (var (x,y) in set)
            {
                // if y == 700
                // split 655
                // 700 - 655 = 35
                // 655 - 35 = 620
                if (x == split)
                {
                    deleteSet.Add((x, y));
                }
                else if (x > split)
                {
                    var diff = x - split;
                    var next = (split - diff, y);

                    newSet.Add(next);
                    deleteSet.Add((x, y));
                }
            }

            set.ExceptWith(deleteSet);
            set.UnionWith(newSet);

            // 661 low
            // 903 too high
            // 901 too high
            return set.Count;
        }

        public override long SolveB()
        {
            var index = Content.ToList().FindIndex(string.IsNullOrWhiteSpace);
            var set = new HashSet<(int, int)>();

            var instructions = Content.ToList().GetRange(0, index);
            var splits = Content.ToList().GetRange(index + 1, Content.Count - index - 1);
            foreach (var line in instructions)
            {
                var xy = line.Split(',').Select(int.Parse).ToList();
                set.Add((xy[0], xy[1]));
            }


            foreach (var splitCommand in splits)
            {
                set = Split(set, splitCommand);
            }

            Print(set);
            // H7IFHJRK
            //  
            // HZIFHJRK
            // HZLEHJRK
            return 0;
        }

        private HashSet<(int, int)> Split(HashSet<(int, int)> full, string command)
        {
            var a = command.Split(' ').Last();
            var b = a.Split('=');

            // Split at y=655
            // 653      o
            // 654 o
            // 655 ------------
            // 656 #
            // 657      #   
            var split = int.Parse(b[1]);
            var translatedSet = new HashSet<(int, int)>();
            var deleteSet = new HashSet<(int, int)>();
            foreach (var (x, y) in full)
            {
                // if y == 700
                // split 655
                // 700 - 655 = 35
                // 655 - 35 = 620
                if (x == split)
                {
                    deleteSet.Add((x, y));
                }

                if (b[0] == "x")
                {
                    if(x > split)
                    {
                        var diff = x - split;
                        var next = (split - diff, y);

                        translatedSet.Add(next);
                        deleteSet.Add((x, y));
                    }
                }
                else
                {
                    if(y > split)
                    {
                        var diff = y - split;
                        var next = (x, split - diff);

                        translatedSet.Add(next);
                        deleteSet.Add((x, y));
                    }
                }
            }

            full.ExceptWith(deleteSet);
            full.UnionWith(translatedSet);
            return full;
        }

        private void Print(HashSet<(int, int)> grid)
        {
            var (x,y) = grid.Max(xy => (xy.Item1, xy.Item2));

            for (int i = 0; i < y + 1; i++)
            {
                for (int j = 0; j < x + 1; j++)
                {
                    if (grid.Contains((j, i)))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }

                Console.Write(Environment.NewLine);
            }
        }
    }
}
