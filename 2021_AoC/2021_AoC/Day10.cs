using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day10:DayBase
    {
        public Day10() : base("10")
        {
        }

        public override long SolveA()
        {
            var sum = 0;
            var illegalScore = new Dictionary<char, int>()
            {
                { ')', 3 },
                { ']', 57 },
                { '}', 1197 },
                { '>', 25137 }
            };

            var opening = "([{<";


            foreach (var line in Content)
            {
                var openingStack = new Stack<char>();
                foreach (var iter in line)
                {
                    if (opening.Contains(iter))
                    {
                        openingStack.Push(iter);
                        continue;
                    }

                    try
                    {
                        var opener = Opener(iter);
                        if (openingStack.Peek() == opener)
                        {
                            openingStack.Pop();
                        }
                        else
                        {
                            // Error
                            sum += illegalScore[iter];

                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        break;
                    }
                }
            }

            return sum;
        }

        public override long SolveB()
        {
            var opening = "([{<";

            var validLines = new List<string>();


            foreach (var line in Content)
            {
                var invalid = false;
                var openingStack = new Stack<char>();
                foreach (var iter in line)
                {
                    if (opening.Contains(iter))
                    {
                        openingStack.Push(iter);
                        continue;
                    }

                    try
                    {
                        var opener = Opener(iter);
                        if (openingStack.Peek() == opener)
                        {
                            openingStack.Pop();
                        }
                        else
                        {
                            invalid = true;
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        invalid = true;
                        Console.WriteLine(e);
                        break;
                    }
                }
                if(!invalid) validLines.Add(line);
            }

            var allSums = new List<long>();
            
            foreach (var line in validLines)
            {
                var openingStack = new Stack<char>();
                foreach (var iter in line)
                {
                    if (opening.Contains(iter))
                    {
                        openingStack.Push(iter);
                        continue;
                    }

                    try
                    {
                        var opener = Opener(iter);
                        if (openingStack.Peek() == opener)
                        {
                            openingStack.Pop();
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        break;
                    }
                }

                var result = "";
                var scoreDict = new Dictionary<char, int>()
                {
                    { ')', 1 },
                    { ']', 2 },
                    { '}', 3 },
                    { '>', 4 }
                };

                while (openingStack.Any())
                {
                    var next = openingStack.Pop();
                    result += $"{Closer(next)}";
                }

                long sum = 0;
                foreach (var iter in result)
                {
                    sum *= 5;
                    sum += scoreDict[iter];
                }

                allSums.Add(sum);
            }

            // 1483081
            return allSums.OrderBy(s => s).ToList()[23];
        }

        private char Opener(char closer)
        {
            if (closer == ')') return '(';
            if (closer == ']') return '[';
            if (closer == '}') return '{';
            if (closer == '>') return '<';
            throw new ArgumentException();
        }
        private char Closer(char opener)
        {
            if (opener == '(') return ')';
            if (opener == '[') return ']';
            if (opener == '{') return '}';
            if (opener == '<') return '>';
            throw new ArgumentException();
        }
    }
}
