namespace Wasla_Backend.Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        [Test]
        public void Test2()
        {
            Assert.Fail();
        }
        [Test]
        public void Test3()
        {
            Assert.That(1, Is.EqualTo(1));
        }
    }
}
