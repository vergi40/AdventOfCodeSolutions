namespace _2023_Aoc
{
    public class Day02 : DayBase
    {
        protected override string DayNumber { get; set; } = "02";

        [Test]
        public void Test1()
        {
            // Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
            // https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching#list-patterns
            var maxCounts = new Dictionary<Color, int>
            {
                { Color.Red, 12 }, { Color.Green, 13 }, { Color.Blue, 14 }
            };
            var sum = 0;
            foreach (var line in Input)
            {
                var split = line.Split(':');
                var (gameString, actions) = (split[0], split[1]);

                var gameNumber = gameString.Split(' ')[1];
                var allPicksInGame = actions.Split(';');
                var overMaximum = false;
                foreach (var pick in allPicksInGame)
                {
                    var colors = pick.Split(',');
                    foreach (var colorString in colors)
                    {
                        // If any color in pick passes the max count
                        var (color, count) = ParseColor(colorString);
                        if (count > maxCounts[color])
                        {
                            overMaximum = true;
                        }
                    }
                }

                if (!overMaximum)
                {
                    sum += int.Parse(gameNumber);
                }

                //if (line is [.., ' ', char gameNumber, ':', var rest])
            }

            Assert.That(sum, Is.EqualTo(1931));
        }

        [Test]
        public void Test2()
        {
            var sum = 0;
            foreach (var line in Input)
            {
                var maxCounts = new Dictionary<Color, int>
                {
                    { Color.Red, 0 }, { Color.Green, 0 }, { Color.Blue, 0 }
                };

                var split = line.Split(':');
                var (_, actions) = (split[0], split[1]);

                var allPicksInGame = actions.Split(';');
                foreach (var pick in allPicksInGame)
                {
                    var colors = pick.Split(',');
                    foreach (var colorString in colors)
                    {
                        // If greater than maxCount, increase maxCount
                        var (color, count) = ParseColor(colorString);
                        if (count > maxCounts[color])
                        {
                            maxCounts[color] = count;
                        }
                    }
                }

                sum += maxCounts[Color.Red] * maxCounts[Color.Green] * maxCounts[Color.Blue];
            }

            Assert.That(sum, Is.EqualTo(83105));
        }

        private enum Color{Red, Green, Blue};

        private (Color color, int count) ParseColor(string input)
        {
            input = input.Trim();
            var split = input.Split(' ');
            var count = int.Parse(split[0]);
            var color = split[1] switch
            {
                "red" => Color.Red,
                "green" => Color.Green,
                "blue" => Color.Blue,
                _ => throw new InvalidOperationException()
            };

            return (color, count);

        }
    }
}