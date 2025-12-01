using vergiCommon;

namespace CSharp;

internal static class Utils
{
    public static IReadOnlyList<string> GetInput(string dayNumber)
    {
        if (dayNumber.Length == 1)
            throw new InvalidOperationException("Please override DayNumber with 2 char number. E.g. 01");

        var fileName = Path.Combine(GetPath.ThisProject(), "..", "..", "Input", "2025", $"{dayNumber}.txt");
        return File.ReadAllLines(fileName);
    }
}