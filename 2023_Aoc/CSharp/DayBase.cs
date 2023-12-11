using vergiCommon;

namespace CSharp
{
    public abstract class DayBase
    {
        /// <summary>
        /// "01", "24"
        /// </summary>
        protected abstract string DayNumber { get; set; }
        protected IReadOnlyList<string> Input { get; private set; }

        [SetUp]
        public void Setup()
        {
            if (DayNumber.Length == 1)
                throw new InvalidOperationException("Please override DayNumber with 2 char number. E.g. 01");

            var fileName = Path.Combine(GetPath.ThisProject(), "../", "Input", $"{DayNumber}.txt");
            Input = File.ReadAllLines(fileName);
        }
    }
}
