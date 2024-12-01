using NUnit.Framework;

namespace CSharp
{
    public class Day01 : DayBase
    {
        protected override string DayNumber { get; set; } = "01";

        // For input automation: 
        // https://adventofcode.com/2024/day/1/input



        [Test]
        public void Test1()
        {
            var input = Input;
            var list1 = new List<int>();
            var list2 = new List<int>();

            foreach (var row in input)
            {
                var entries = row.Split("   ");
                list1.Add(int.Parse(entries[0]));
                list2.Add(int.Parse(entries[1]));
            }

            list1.Sort();
            list2.Sort();

            var sum = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                sum += Math.Abs(list1[i] - list2[i]);
            }

            Assert.That(sum, Is.EqualTo(2166959));
        }

        [Test]
        public void Test2()
        {
            var input = Input;
            var list1 = new List<int>();
            var list2 = new List<int>();

            foreach (var row in input)
            {
                var entries = row.Split("   ");
                list1.Add(int.Parse(entries[0]));
                list2.Add(int.Parse(entries[1]));
            }

            var resultMap = new Dictionary<int, int>();

            var sum = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                var toFind = list1[i];
                if (resultMap.TryGetValue(toFind, out var similarityScore))
                {
                    //
                }
                else
                {
                    var count = list2.Count(x => x == toFind);

                    similarityScore = toFind * count;
                    resultMap[toFind] = similarityScore;
                }
                
                sum += similarityScore;
            }

            Assert.That(sum, Is.EqualTo(23741109));
        }

    }
}