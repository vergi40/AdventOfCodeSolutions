using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day05 : DayBase
    {
        public Day05() : base("05")
        {
        }

        public override long SolveA()
        {
            var segmentList = Initialize(true);

            var array = new int[1000, 1000];
            foreach(var segment in segmentList)
            {
                foreach(var point in segment.EnumerateSegmentPoints())
                {
                    array[point.x, point.y]++;
                }
            }

            var sum = 0;
            for(var i = 0; i < 1000; i++)
            {
                for(var j = 0; j < 1000; j++)
                {
                    if(array[i, j] > 1)
                    {
                        sum++;
                    }
                }
            }

            return sum;
        }

        public override long SolveB()
        {
            var segmentList = Initialize(false);

            var array = new int[1000, 1000];
            foreach (var segment in segmentList)
            {
                foreach (var point in segment.EnumerateSegmentPoints())
                {
                    array[point.x, point.y]++;
                }
            }

            var sum = 0;
            for (var i = 0; i < 1000; i++)
            {
                for (var j = 0; j < 1000; j++)
                {
                    if (array[i, j] > 1)
                    {
                        sum++;
                    }
                }
            }

            return sum;
        }

        private List<Segment> Initialize(bool onlyHorizontalOrVertical)
        {
            // 955,125 -> 151,929
            // https://spatial.mathdotnet.com/api/MathNet.Spatial.Euclidean/LineSegment2D.htm
            var segmentList = new List<Segment>();

            foreach (var entry in Content.ToList())
            {
                var points = entry.Split(" -> ");
                var a = points[0].Split(",");
                var b = points[1].Split(",");


                var segment = new Segment(int.Parse(a[0]), int.Parse(a[1]), int.Parse(b[0]), int.Parse(b[1]));
                if (onlyHorizontalOrVertical)
                {
                    if (segment.Horizontal || segment.Vertical)
                    {
                        // ok
                    }
                    else
                    {
                        continue;
                    }
                }
                segmentList.Add(segment);
            }
            return segmentList;
        }

        class Segment
        {
            public (int x, int y) Start { get; }
            public (int x, int y) End { get; }
            public bool Horizontal { get; }
            public bool Vertical { get; }

            public Segment(int x1, int y1, int x2, int y2)
            {    
                if (x1 == x2)
                {
                    Vertical = true;

                    if (y1 > y2)
                    {
                        var temp = y1;
                        y1 = y2;
                        y2 = temp;
                    }
                }
                else if (y1 == y2)
                {
                    Horizontal = true;

                    if (x1 > x2)
                    {
                        var temp = x1;
                        x1 = x2;
                        x2 = temp;
                    }
                }
                else
                {
                    // Diagonal
                    // Make default rule that x is always ascending
                    if(x1 > x2)
                    {
                        var temp = (x1, y1);
                        (x1,y1) = (x2,y2);
                        (x2,y2) = temp;
                    }
                }

                Start = (x1, y1);
                End = (x2, y2);
            }

            public IEnumerable<(int x, int y)> EnumerateSegmentPoints()
            {
                if (Horizontal)
                {
                    for(int i = Start.x; i <= End.x; i++)
                    {
                        yield return (i, Start.y);
                    }
                }
                else if (Vertical)
                {
                    for (int i = Start.y; i <= End.y; i++)
                    {
                        yield return (Start.x, i);
                    }
                }
                else
                {
                    var ascending = End.y > Start.y;
                    var sign = ascending ? 1 : -1;

                    var j = 0;
                    for(int i = Start.x; i <= End.x; i++)
                    {
                        var newY = Start.y + sign * j;
                        yield return (i, newY);

                        j++;
                    }
                }
            }

            public override string ToString()
            {
                return $"{Start} {End}";
            }
        }
    }
}
