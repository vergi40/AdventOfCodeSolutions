using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using _2021_AoC;

namespace _2022_AoC
{
    internal class Day25 : DayBase
    {
        private List<string> TestContent => new List<string> { "1=-0-2", "12111", "2=0=", "21", "2=01", "111", "20012", "112", "1=-1=", "1-12", "12", "1=", "122" };

        public Day25() : base("25")
        {
        }

        public override long SolveA()
        {
            var sum = 0;
            var interpreter = new SnafuInterpreter();

            var test1 = interpreter.To5BaseSystem(21);
            var test2 = interpreter.To5BaseSystem(22);
            var test3 = interpreter.To5BaseSystem(36);

            foreach (var line in TestContent)
            {
                var dec = interpreter.ToDecimal(line);
                sum += dec;
            }

            Console.WriteLine($"A: {interpreter.ToSnafu2(sum)}");
            return sum;
        }

        public override long SolveB()
        {
            return 0;
        }
    }

    class SnafuInterpreter
    {
        private int Base { get; } = 5;

        public int ToDecimal(string snafu)
        {
            var sum = 0;
            var rev = snafu.Reverse().ToList();
            //var rev = snafu.Reverse().ToString();
            for (int i = 0; i < snafu.Length; i++)
            {
                var c = rev[i];
                var number = c switch
                {
                    '-' => -1,
                    '=' => -2,
                    _ => int.Parse(c.ToString())
                };
                sum += number * (int)Math.Pow(Base, i);

            }
            return sum;
        }

        public string ToSnafu2(int number)
        {
            var (quotient, remainder) = (number, 0);
            var newNumber = new List<string>(Enumerable.Repeat("", 10));
            var index = 0;
            while (quotient != 0)
            {
                (quotient, remainder) = Math.DivRem(quotient, Base);

                if (remainder > 2)
                {
                    // Reset 
                    if (Base - remainder == -2)
                    {
                        newNumber[index] = "=";
                    }
                    else
                    {
                        newNumber[index] = "-";
                    }

                    var negative = ToDecimal(ToSnafuString(newNumber));
                    quotient = number - negative;
                }
                else
                {
                    newNumber[index] = remainder.ToString();
                    index++;
                }
            }

            return ToSnafuString(newNumber);

        }

        public string ToSnafu(int number)
        {
            // E.g. 7
            // 1 * 5^1 + 2 * 5^0

            // E.g. 36 % 5
            // 7*5 + 1 -> 1
            // 7 % 5
            // 1*5 + 2 -> 2
            // 1 % 5
            // 0*5 + 1 -> 1
            // 1 * 5^2 +  2 * 5^1 + 1 * 5^0

            // E.g. 8
            // 1*5 + 3 -> 3 -> -2 -> -2 here
            // Apply inverse -2. for second section do 8+2 % 5

            // E.g. 21. Should be 1-1
            // 4*5 + 1 -> 1
            // 0*5 + 4 -> 4 -> -. -5 here
            // Apply inverse -4. 

            // E.g. 22. Should be 1-2
            // 4*5 + 2 -> 2
            // 4 = 0*5 + 4 -> -. -5 here
            // Apply inverse -3. for third section do 22+3 % 25

            var (quotient, remainder) = (number,0);
            var newNumber = new Stack<string>();

            while (quotient != 0)
            {
                (quotient, remainder) = Math.DivRem(quotient, Base);
                if (remainder > 2)
                {
                    // Reset 
                    if (Base - remainder == -2)
                    {
                        newNumber.Push("=");
                    }
                    else
                    {
                        newNumber.Push("-");
                    }

                    var negative = ToDecimal(ToSnafuString(newNumber));
                    quotient = number - negative;
                }
                else
                {
                    newNumber.Push(remainder.ToString());

                }
            }
            
            return ToSnafuString(newNumber);
        }

        private string ToSnafuString(Stack<string> stack)
        {
            var result = "";
            var newNumber = new Stack<string>(stack);
            while (newNumber.TryPop(out var next))
            {
                result += next;
            }

            return result;
        }
        private string ToSnafuString(IReadOnlyList<string> list)
        {
            var result = "";
            foreach (var num in list.Reverse().ToList())
            {
                if (string.IsNullOrEmpty(num)) break;
                result += num;
            }

            return result;
        }

        public string To5BaseSystem(int number)
        {
            // https://www.tutorialspoint.com/computer_logical_organization/number_system_conversion.htm
            // E.g. 7
            // 1 * 5^1 + 2 * 5^0

            // Step 1 − Divide the decimal number to be converted by the value of the new base.
            var (quotient, remainder) = Math.DivRem(number, Base);
            // Step 2 − Get the remainder from Step 1 as the rightmost digit (least significant digit) of new base number.
            var newNumber = new Stack<int>();
            newNumber.Push(remainder);

            // Step 3 − Divide the quotient of the previous divide by the new base.
            while (quotient != 0)
            {
                (quotient, remainder) = Math.DivRem(quotient, Base);
                // Step 4 − Record the remainder from Step 3 as the next digit (to the left) of the new base number.
                newNumber.Push(remainder);
            }

            var result = "";
            while(newNumber.TryPop(out var next))
            {
                result += next.ToString();
            }

            return result;
        }
    }
}
