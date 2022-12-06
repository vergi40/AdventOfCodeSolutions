namespace _2022_AoC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Advent of Code 2022 solutions");

            var code = new Day05();
            code.Initialize();

            Console.WriteLine($"Result a: " + code.SolveA());
            Console.WriteLine($"Result b: " + code.SolveB());
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}