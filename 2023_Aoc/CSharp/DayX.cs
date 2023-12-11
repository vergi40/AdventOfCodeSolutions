namespace CSharp
{
    public class DayX : DayBase
    {
        protected override string DayNumber { get; set; } = "xx";
        
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}