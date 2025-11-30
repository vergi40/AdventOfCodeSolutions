using NUnit.Framework;

namespace CSharp
{
    public class Day02 : DayBase
    {
        protected override string DayNumber { get; set; } = "02";

        [Test]
        public void Test1()
        {
            var sum = 0;

            foreach (var row in Input)
            {
                var numbers = row.Split(' ').Select(int.Parse).ToList();

                if (IsIncreasing(numbers))
                {
                    sum += 1;
                }
                else if (IsDecreasing(numbers))
                {
                    sum += 1;
                }
            }

            Assert.That(sum, Is.EqualTo(332));
        }

        private bool IsIncreasing(List<int> numbers)
        {
            for (int i = 1; i < numbers.Count; i++)
            {
                var previous = numbers[i - 1];
                var next = numbers[i];

                if (next - previous < 1)
                {
                    return false;
                }

                if (next - previous > 3)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsDecreasing(List<int> numbers)
        {
            for (int i = 1; i < numbers.Count; i++)
            {
                var previous = numbers[i - 1];
                var next = numbers[i];

                // 6 3 2
                if (next - previous > -1)
                {
                    return false;
                }

                if (next - previous < -3)
                {
                    return false;
                }
            }

            return true;
        }

    }
}