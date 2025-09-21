using NUnit.Framework;
using System;
using System.Collections.Generic;
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
                // Should have 1-3 integers, ",", 1-3 integers, ")"
                if(!TryReadNumber(mulPrefix, 0, out var first, out var nextIndex)){
                    continue;
                }

                if (nextIndex >= mulPrefix.Length || mulPrefix[nextIndex] != ',')
                {
                    continue;
                }
                nextIndex++;

                if (!TryReadNumber(mulPrefix, nextIndex, out var second, out nextIndex))
                {
                    continue;
                }

                if (nextIndex >= mulPrefix.Length || mulPrefix[nextIndex] != ')')
                {
                    continue;
                }

                result += first * second;
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
    }
}
