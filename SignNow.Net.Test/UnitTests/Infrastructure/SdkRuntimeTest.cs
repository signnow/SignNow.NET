using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Infrastructure;

namespace UnitTests.Infrastructure
{
    [TestClass]
    public class SdkRuntimeTest
    {
        [TestMethod]
        public void ShouldReturnProperClientName()
        {
            Assert.AreEqual(SdkRuntime.ClientName, "SignNow .NET API Client");
        }

        [TestMethod]
        public void ShouldReturnProperSdkVersion()
        {
            var pattern = new Regex(@"(?<version>\d+.\d+.\d+.\d+)");

            StringAssert.Matches(SdkRuntime.Version.ToString(), pattern);
        }
    }
}
