namespace _2022_AoC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Day number: ");

            Console.WriteLine();

            var day = "01";
            var code = new Day01();
            code.Initialize();

            var result1 = code.SolveA();
            Console.WriteLine($"Result a: " + result1);

            var result2 = code.SolveB();
            Console.WriteLine($"Result b: " + result2);
            Console.Read();
        }
    }
}