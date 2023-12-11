namespace CSharp
{
    public class Day01 : DayBase
    {
        // For input automation: 
        // https://adventofcode.com/2023/day/1/input



        [Test]
        public void Test1()
        {
            var input = Input;

            var sum = 0;
            foreach (var line in input)
            {
                var first = line.First(char.IsDigit);
                var last = line.Last(char.IsDigit);

                var num = int.Parse($"{first}{last}");
                sum += num;
            }

            Assert.That(sum, Is.EqualTo(53334));
        }

        [Test]
        public void Test2()
        {
            var input = Input;

            var sum = 0;
            foreach (var line in ConvertLetters(input))
            {
                var first = line.First(char.IsDigit);
                var last = line.Last(char.IsDigit);

                var num = int.Parse($"{first}{last}");
                sum += num;
            }

            // 52418 too low
            Assert.That(sum, Is.EqualTo(52834));
        }

        [Test]
        public void CustomTest()
        {
            var input = new List<string> { "61", "16", "969five", "one8eight", "fourfivefourone9fourqnsjlbgkv9nine", "xtwone3four" };
            var res = ConvertLetters(input).ToList();
        }

        private IEnumerable<string> ConvertLetters(IEnumerable<string> lines)
        {
            var dict = new Dictionary<string, int>
            {
                {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5}, {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9}
            };

            foreach (var line in lines)
            {
                var res = "";
                for (int i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))
                    {
                        res += line[i];
                        continue;
                    }

                    foreach (var (letter, number) in dict)
                    {
                        var sub = line.Substring(i);
                        if (sub.StartsWith(letter))
                        {
                            res += Convert.ToString(number);
                            continue;
                        }
                    }
                }
                yield return res;
            }
        }

        protected override string DayNumber { get; set; } = "01";
    }
}