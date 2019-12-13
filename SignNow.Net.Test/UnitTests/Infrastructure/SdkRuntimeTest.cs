using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Infrastructure;

namespace UnitTests
{
    [TestClass]
    public class SdkRuntimeTest
    {
        [TestMethod]
        public void ShouldReturnProperClientName()
        {
            Equals(SdkRuntime.ClientName, "SignNow .NET API Client");
        }

        [TestMethod]
        public void ShouldReturnProperSdkVersion()
        {
            var pattern = new Regex(@"(?<version>\d+.\d+.\d+.\d+)");

            StringAssert.Matches(SdkRuntime.Version.ToString(), pattern);
        }
    }
}
