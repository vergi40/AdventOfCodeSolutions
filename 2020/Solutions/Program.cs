using System;
using System.Collections.Generic;
using Common;

namespace Solutions
{
    class Program
    {
        static void Main(string[] args)
        {
            //var solution = new Day01();
            //var solution = new Day02();
            //var solution = new Day03();
            //var solution = new Day04();
            //var solution = new Day10();
            //var solution = new Day11();
            //var solution = new Day12();
            //var solution = new Day13();
            //var solution = new Day14();
            //var solution = new Day15();
            //var solution = new Day17();
            //var solution = new Day20();
            var solution = new Day21();



            var result1 = solution.SolveA();
            Console.WriteLine($"Result a: " + result1);

            var result2 = solution.SolveB();
            Console.WriteLine($"Result b: " + result2);
            Console.Read();
        }
    }
}
