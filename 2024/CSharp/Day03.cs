using NUnit.Framework;
using System.Text;

namespace CSharp
{
    internal class Day03 : DayBase
    {
        protected override string DayNumber { get; set; } = "03";

        [Test]
        public void Test1()
        {
            var fullInput = string.Concat(Input);
            var result = 0;

            // mul(x,y)

            // Find mul prefixes and split by them
            var mulPrefixes = fullInput.Split("mul(", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var mulPrefix in mulPrefixes)
            {
                if(TryParseNext(mulPrefix, out var mulResult))
                {
                    result += mulResult;
                }
            }

            Assert.That(result, Is.EqualTo(187194524));
        }

        private bool TryReadNumber(string input, int startIndex, out int number, out int nextIndex)
        {
            number = 0;
            nextIndex = startIndex;
            var validDigits = new StringBuilder();
            for (int i = startIndex; i < input.Length && validDigits.Length < 3; i++)
            {
                var next = input[i];
                if (char.IsDigit(next))
                {
                    validDigits.Append(next);
                    nextIndex++;
                }
                else
                {
                    break;
                }
            }
            if (validDigits.Length > 0)
            {
                number = int.Parse(validDigits.ToString());
                return true;
            }
            return false;
        }

        [Test]
        public void Test2()
        {
            var fullInput = string.Concat(Input);
            var result = 0;

            // mul(x,y)
            var currentIndex = 0;
            var enabled = true;
            while (currentIndex < fullInput.Length)
            {
                if (!TryGetNextOperation(fullInput, currentIndex, out var nextOperation, out var nextIndex))
                {
                    currentIndex++;
                    continue;
                }

                if (nextOperation == Operation.Do)
                {
                    enabled = true;
                }
                else if (nextOperation == Operation.Dont)
                {
                    enabled = false;
                }
                else if (nextOperation == Operation.Multiply && enabled)
                {
                    // Process mul
                    if (TryParseNext(fullInput.Substring(nextIndex, 13), out var mulResult))
                    {
                        result += mulResult;
                    }

                }

                currentIndex = nextIndex;
            }

            Assert.That(result, Is.EqualTo(127092535));
        }

        private bool TryGetNextOperation(string input, int startIndex, out Operation operation, out int nextIndex)
        {
            operation = Operation.Do;
            nextIndex = startIndex;
            var validChars = new StringBuilder();

            // do()
            // don't()
            // mul(
            var dict = new Dictionary<string, Operation>
            {
                { "do()", Operation.Do },
                { "don't()", Operation.Dont },
                { "mul(", Operation.Multiply }
            };

            foreach (var kvp in dict)
            {
                var target = kvp.Key;
                if(startIndex + target.Length > input.Length)
                {
                    // End of input
                    continue;
                }

                var isEqual = true;
                for (int i = 0; i < target.Length; i++)
                {
                    var currentIndex = startIndex + i;
                    if (!input[currentIndex].Equals(target[i]))
                    {
                        isEqual = false;
                        break;
                    }
                }
                // Each char matches target
                if (isEqual) 
                {
                    operation = kvp.Value;
                    nextIndex += kvp.Key.Length;
                    return true; 
                }
            }

            return false;

        }

        private bool TryParseNext(string input, out int result)
        {
            result = 0;

            // Should have 1-3 integers, ",", 1-3 integers, ")"
            if (!TryReadNumber(input, 0, out var first, out var nextIndex))
            {
                return false;
            }

            if (nextIndex >= input.Length || input[nextIndex] != ',')
            {
                return false;
            }
            nextIndex++;

            if (!TryReadNumber(input, nextIndex, out var second, out nextIndex))
            {
                return false;
            }

            if (nextIndex >= input.Length || input[nextIndex] != ')')
            {
                return false;
            }

            result = first * second;
            return true;
        }

        enum Operation
        {
            Do, Dont, Multiply
        }

    }
}
