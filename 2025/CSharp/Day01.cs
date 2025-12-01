namespace CSharp;

[TestFixture]
public class Day01
{
    [Test]
    public void TestA()
    {
        var input = Utils.GetInput("01");

        var rotations = input.Select(i => (i[0], int.Parse($"{i[1..]}")));

        var sum = 0;
        var current = 50;
        foreach (var (leftRight, amount) in rotations)
        {
            var direction = leftRight == 'R' ? 1 : -1;
            sum += DoRotation(direction, amount, ref current);
        }

        Assert.That(sum, Is.EqualTo(1100));
    }

    private static int DoRotation(int direction, int amount, ref int current)
    {
        var next = current + (direction * amount);

        // If result is 100, goes around to 0
        while(next > 99) next -= 100;
        // If result is -1, goes around to 99
        while(next < 0) next += 100;
        current = next;

        if (next == 0) return 1;
        return 0;
    }

    [Test]
    public void TestBExample()
    {
        var input = new List<string>
        {
            "L68",
            "L30",
            "R48",
            "L5",
            "R60",
            "L55",
            "L1",
            "L99",
            "R14",
            "L82"
        };

        var result = RunB(input);

        Assert.That(result, Is.EqualTo(6));
    }

    [Test]
    public void TestB()
    {
        var input = Utils.GetInput("01");
        var result = RunB(input);

        // 6364 6814
        Assert.That(result, Is.EqualTo(6358));
    }

    public static int RunB(IReadOnlyList<string> input)
    {
        var rotations = input.Select(i => (i[0], int.Parse($"{i[1..]}")));

        var sum = 0;
        var current = 50;
        foreach (var (leftRight, amount) in rotations)
        {
            var direction = leftRight == 'R' ? 1 : -1;
            sum += DoRotationB(direction, amount, ref current);
        }

        return sum;
    }

    private static int DoRotationB(int direction, int amount, ref int current)
    {
        var zeros = 0;
        var remaining = amount;
        var previous = current;
        while (true)
        {
            if (Math.Abs(remaining) < 100)
            {
                current += direction * remaining;
                if (current < 0)
                {
                    if (previous != 0 && current != 0)
                    {
                        zeros++;
                    }
                    current += 100;
                }
                // 50->50 should only add 1
                else if (current > 99)
                {
                    if (previous != 0 && current != 100)
                    {
                        zeros++;
                    }
                    current -= 100;
                }

                if (current == 0)
                {
                    zeros++;
                }
                break;
            }
            
            remaining -= 100;
            zeros++;
        }
        
        return zeros;
    }
}