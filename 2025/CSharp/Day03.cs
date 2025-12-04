namespace CSharp;

public class Day03
{

    [Test]
    public void A()
    {
        var input = Utils.GetInput("03");

        var sum = 0;
        foreach (var line in input)
        {
            var maxJoltage= CalculateJoltage(line);
            sum += maxJoltage;
        }

        Assert.That(sum, Is.EqualTo(17144));
    }

    private int CalculateJoltage(string line)
    {
        var numbers = line.Select(l => int.Parse($"{l}")).ToList();
        var (max, maxIndex) = GetMaxAndIndex(numbers);

        if (maxIndex == numbers.Count - 1)
        {
            // Last one highest
            numbers.RemoveAt(numbers.Count - 1);
            var (secondInBeginning, _) = GetMaxAndIndex(numbers);
            return int.Parse($"{secondInBeginning}{max}");
        }

        // Take next highest from remaining
        numbers.RemoveRange(0, maxIndex + 1);
        var (second, _) = GetMaxAndIndex(numbers);
        return int.Parse($"{max}{second}");
    }

    private (int max, int index) GetMaxAndIndex(List<int> numbers)
    {
        var max = 0;
        var index = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            var next = numbers[i];
            if (next > max)
            {
                max = next;
                index = i;
            }
        }

        return (max, index);
    }

    [Test]
    [Ignore("WIP")]
    public void B()
    {
        var input = Utils.GetInput("03");

        long sum = 0;
        foreach (var line in input)
        {
            var maxJoltage = CalculateJoltageB(line);
            sum += maxJoltage;
        }

        throw new NotImplementedException();
        Assert.That(sum, Is.EqualTo(17144));
    }

    private const int DigitCount = 12;
    private long CalculateJoltageB(string line)
    {
        var numbers = line.Select(l => int.Parse($"{l}")).ToList();
        var (max, maxIndex) = GetMaxAndIndex(numbers);

        if (maxIndex == numbers.Count - DigitCount - 1)
        {
            // Special case: Max is 12th last, take the rest
            var special = numbers.Slice(numbers.Count - DigitCount - 1, DigitCount);
            return ConvertToLong(special);
        }

        if (maxIndex == numbers.Count - 1)
        {
            // Last one highest *8889
            numbers.RemoveAt(numbers.Count - 1);
            var (secondInBeginning, _) = GetMaxAndIndexB(numbers);
            return int.Parse($"{secondInBeginning}{max}");
        }

        // Take next highest from remaining
        numbers.RemoveRange(0, maxIndex + 1);
        var (second, _) = GetMaxAndIndexB(numbers);
        return int.Parse($"{max}{second}");
    }

    private static long ConvertToLong(IReadOnlyList<int> numbers)
    {
        var strList = numbers.Select(n => $"{n}").ToList();
        var str = string.Join("", strList);
        return long.Parse(str);
    }

    private (int max, int index) GetMaxAndIndexB(List<int> numbers)
    {
        var max = 0;
        var index = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            var next = numbers[i];
            if (next > max)
            {
                max = next;
                index = i;
            }
        }

        return (max, index);
    }
}
