using System.Collections.Frozen;

namespace CSharp;

public class Day07
{
#pragma warning disable S1144
    private readonly List<string> _exampleInput =
    [
        ".......S.......",
        ".......|.......",
        "......|^|......",
        "......|.|......",
        ".....|^|^|.....",
        ".....|.|.|.....",
        "....|^|^|^|....",
        "....|.|.|.|....",
        "...|^|^|||^|...",
        "...|.|.|||.|...",
        "..|^|^|||^|^|..",
        "..|.|.|||.|.|..",
        ".|^|||^||.||^|.",
        ".|.|||.||.||.|.",
        "|^|^|^|^|^|||^|",
        "|.|.|.|.|.|||.|",
    ];
#pragma warning restore S1144

    [Test]
    public void A()
    {
        var input = Utils.GetInput("07");
        //var input = _exampleInput;
        var activeBeams = new HashSet<int> { input[0].IndexOf('S') };
        var splitCounter = 0;

        foreach (var line in input.Skip(1))
        {
            var nextActiveBeams = new HashSet<int>();

            foreach (var activeBeam in activeBeams)
            {
                var nextChar = line[activeBeam];
                if (nextChar == '^')
                {
                    splitCounter++;

                    nextActiveBeams.Add(activeBeam - 1);
                    nextActiveBeams.Add(activeBeam + 1);
                }
                else
                {
                    // Same beam continues
                    nextActiveBeams.Add(activeBeam);
                }
            }

            activeBeams.Clear();
            activeBeams = nextActiveBeams;
        }

        // 2575 too high
        // 3284 too high
        // 1727 too high
        Assert.That(splitCounter, Is.EqualTo(1642));
    }

    [Test]
    public void B()
    {
        var input = Utils.GetInput("07");
        //var input = _exampleInput;

        // construct mappings for efficiency. [int level, set int split indexes]
        var dict = new Dictionary<int, HashSet<int>>
        {
            [0] = []
        };

        for (int i = 1; i < input.Count; i++)
        {
            dict[i] = new HashSet<int>();
            var line = input[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == '^')
                {
                    dict[i].Add(j);
                }
            }
        }
        _dict = dict.ToFrozenDictionary();
        _maxLevel = _dict.Keys.Max();

        var startIndex = input[0].IndexOf('S');
        var result = Traverse(1, startIndex, 0);

        Assert.That(result, Is.EqualTo(47274292756692));
    }

    private FrozenDictionary<int, HashSet<int>> _dict;
    private int _maxLevel = 0;

    private Dictionary<(int level, int index), ulong> _pruneMap = new();
    private const int MaxPruneDepth = 3;

    private ulong Traverse(int currentLevel, int currentIndex, ulong acc)
    {
        if (currentLevel == _maxLevel)
        {
            // Reached bottom, add +1
            acc++;
            return acc;
        }

        if (_dict[currentLevel].Contains(currentIndex))
        {
            if (currentLevel < (_maxLevel - MaxPruneDepth) && _pruneMap.TryGetValue((currentLevel, currentIndex), out var res))
            {
                return res;
            }

            // Split point
            var leftAcc = Traverse(currentLevel + 1, currentIndex - 1, acc);
            var rightAcc = Traverse(currentLevel + 1, currentIndex + 1, acc);

            if (currentLevel < (_maxLevel - MaxPruneDepth))
            {
                _pruneMap[(currentLevel, currentIndex)] = (leftAcc + rightAcc);
            }

            return leftAcc + rightAcc;
        }
        else
        {
            // Else continue downwards
            acc += Traverse(currentLevel + 1, currentIndex, acc);
        }
            
        return acc;
    }
}
