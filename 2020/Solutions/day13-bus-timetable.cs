using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Solutions
{
    class Day13 : DayBase
    {
        public Day13() : base("13")
        {
        }

        public override int SolveA()
        {
            var earliest = int.Parse(Content[0]);
            var buses = Content[1].Split(',').Where(b => b != "x").Select(int.Parse).ToList();

            var smallestDifference = 10000000;
            var best = 0;

            foreach (var bus in buses)
            {
                // time before earliest
                var modulo = earliest % bus;

                // time after
                var busDeparture = earliest - modulo + bus;
                var waitingTime = busDeparture - earliest;
                if (waitingTime < smallestDifference)
                {
                    smallestDifference = waitingTime;
                    best = bus;
                }
            }

            return smallestDifference * best;
        }

        public override int SolveB()
        {
            // 100 000 000 000 000
            var line = Content[1];
            line = "67,7,x,59,61";
            //line = "1789,37,47,1889";

            var list = line.Split(',');
            var buses = new List<int>();

            foreach (var data in list)
            {
                if (data == "x")
                {
                    buses.Add(-1);
                }
                else
                {
                    buses.Add(int.Parse(data));
                }
            }

            var distinct = new List<(int interval, int index)>();
            for (int i = 0; i < buses.Count; i++)
            {
                var bus = buses[i];
                if (bus != -1)
                {
                    distinct.Add((bus, i));
                }
            }

            long departure = 0;
            for (int i = 0; i < distinct.Count - 1; i++)
            {
                while (true)
                {
                    var tuple = distinct[i];
                    var tupleNext = distinct[i + 1];
                    if ((departure + tuple.index) % tuple.interval == 0)
                    {
                        if ((departure + tupleNext.index) % tupleNext.interval == 0)
                        {
                            // Found common departure for both
                            // Start checking next
                            break;
                        }
                    }
                    departure += tuple.interval;
                }
            }

            return 0;
        }

        public int SolveBOld()
        {
            // 100 000 000 000 000
            var line = Content[1];
            //line = "67,7,x,59,61";
            line = "1789,37,47,1889";

            var list = line.Split(',');
            var buses = new List<int>();

            foreach (var data in list)
            {
                if (data == "x")
                {
                    buses.Add(-1);
                }
                else
                {
                    buses.Add(int.Parse(data));
                }
            }

            // If common modulo found, increase step
            long step = buses.First();
            long departure = step;
            var found = false;

            var highestModuloDeciderIndex = 0;
            
            while (true)
            {
                // for (int i = indexForCurrentStepDecider; i < buses.Count; i++)
                for (int i = 0; i < buses.Count; i++)
                {
                    var bus = buses[i];
                    if (bus == -1) continue;

                    if ((departure + i) % bus != 0) break;
                    
                    else
                    {
                        if (i == buses.Count - 1)
                        {
                            // Success
                            found = true;
                            break;
                        }

                        //if (i > highestModuloDeciderIndex && bus > step)
                        //{
                        //    step = bus;
                        //    highestModuloDeciderIndex = i;
                        //}
                        
                        //if (i > indexForCurrentStepDecider)
                        //{
                        //    // Found later number with matching modulos.
                        //    // Increase step to current time
                        //    indexForCurrentStepDecider = i;
                        //    if (bus > step)
                        //    {
                        //        step = bus;
                        //    }
                        //}
                    }
                }

                if (found) break;
                else departure += step;
            }

            return 0;
        }
    }
}
