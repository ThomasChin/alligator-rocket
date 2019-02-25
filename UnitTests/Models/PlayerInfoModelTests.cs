using NUnit.Framework;
using Crawl.Models;

namespace UnitTests.Models
{
    [TestFixture]
    public class PlayerInfoModelTests
    {
        [Test]
        public void Model_PlayerInfo_Instantiate_Should_Pass()
        {
            var myData = new PlayerInfo();

            Assert.AreNotEqual(null, myData, TestContext.CurrentContext.Test.Name);
        }
    }
}