using _2021_AoC;

namespace _2022_AoC
{
    // Turned out pretty bloated design. Should have defined all processes A->B, A->C etc or do char value comparison

    internal class Day02 : DayBase
    {
        public Day02() : base("02") { }

        public override long SolveA()
        {
            // A Y
            // B X
            // C Z
            var sum = 0;
            foreach (var line in Content)
            {
                var lineSplit = line.Split(' ');
                var opponent = new Weapon(lineSplit[0]);
                var player = new Weapon(lineSplit[1]);

                sum += player.Fight(opponent) + player.WeaponValue();
            }

            return sum;
        }


        public override long SolveB()
        {
            var sum = 0;
            foreach (var line in Content)
            {
                var lineSplit = line.Split(' ');
                var opponentChar = lineSplit[0];
                var opponent = new Weapon(opponentChar);
                var roundResult = lineSplit[1];

                if (roundResult == "X")
                {
                    // Need to lose
                    var playerChar = opponentChar switch
                    {
                        "A" => "C",
                        "B" => "A",
                        "C" => "B"
                    };
                    var player = new Weapon(playerChar);
                    sum += player.Fight(opponent) + player.WeaponValue();
                }
                else if (roundResult == "Y")
                {
                    // Need a tie
                    var player = new Weapon(opponentChar);
                    sum += player.Fight(opponent) + player.WeaponValue();
                }
                else
                {
                    // Need to win
                    var playerChar = opponentChar switch
                    {
                        "A" => "B",
                        "B" => "C",
                        "C" => "A"
                    };
                    var player = new Weapon(playerChar);
                    sum += player.Fight(opponent) + player.WeaponValue();
                }
            }

            return sum;
        }

        class Weapon
        {
            public WeaponType WeaponType { get; }
            public Weapon(string input)
            {
                WeaponType = input switch
                {
                    "A" or "X" => WeaponType.Rock,
                    "B" or "Y" => WeaponType.Paper,
                    "C" or "Z" => WeaponType.Scissors,
                    _ => throw new ArgumentException()
                };
            }

            public int Fight(Weapon opponent)
            {
                if (opponent.WeaponType == WeaponType) return 3;

                if(WeaponType == WeaponType.Rock)
                {
                    return opponent.WeaponType switch
                    {
                        WeaponType.Scissors => 6,
                        WeaponType.Paper => 0,
                    };
                }
                if (WeaponType == WeaponType.Paper)
                {
                    return opponent.WeaponType switch
                    {
                        WeaponType.Rock => 6,
                        WeaponType.Scissors => 0,
                    };
                }
                if (WeaponType == WeaponType.Scissors)
                {
                    return opponent.WeaponType switch
                    {
                        WeaponType.Paper => 6,
                        WeaponType.Rock => 0,
                    };
                }

                throw new ArgumentException();
            }

            public int WeaponValue()
            {
                return WeaponType switch
                {
                    WeaponType.Rock => 1,
                    WeaponType.Paper => 2,
                    WeaponType.Scissors => 3
                };
            }
        }
        enum WeaponType
        {
            Rock,
            Paper,
            Scissors
        }
    }
}
