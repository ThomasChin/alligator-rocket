using NUnit.Framework;

using Crawl.Models;

namespace UnitTests.Models
{
    [TestFixture]
    class VersionGlobalTests
    {

        [Test]
        public void Model_VersionGlobals_GetCodeVersion_Should_Pass()
        {
            var Actual = VersionGlobals.GetCodeVersion();
            
            Assert.AreNotEqual(null, Actual,TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Model_VersionGlobals_GetDataVersion_Should_Pass()
        {
            var Actual = VersionGlobals.GetDataVersion();

            Assert.AreNotEqual(null, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Model_VersionGlobals_GetCombinedVersion_Should_Pass()
        {
            var Actual = VersionGlobals.GetCombinedVersion();
            string Expected = "Version: " + VersionGlobals.GetCodeVersion() + " Data: " + VersionGlobals.GetDataVersion();
            
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }
    }
}
