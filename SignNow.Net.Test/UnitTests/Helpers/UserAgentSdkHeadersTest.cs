using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SignNow.Net.Internal.Helpers;
using System.Text.RegularExpressions;

namespace UnitTests
{
    [TestClass]
    public class UserAgentSdkHeadersTest
    {
        [TestMethod]
        public void UserAgentStringIsCorrect()
        {
            /// SignNow .NET API Сlient/v1.0.0.0 (Windows_NT 10.0.14393; win32; ia32) .NET Core/v4.0.30319.42000
            /// SignNow .NET API Сlient/v1.0.0.0 (Linux 3.10.0; Unix; X64) .NET Core/v4.0.30319.42000
            /// SignNow .NET API Сlient/v1.0.0.0 (Mac OSX 17.5.0; Unix; X64) .NET Core/v4.0.30319.42000
            var pattern = "^" +
                "(?<client>.*)\\/" +
                "(?<version>v\\d+.\\d+.\\d+.?\\d+)\\s+" +
                "\\(" +
                "(?<os>.*);\\s" +
                "(?<platform>\\w+);\\s" +
                "(?<arch>\\w+)" +
                "\\)\\s" +
                "(?<runtime>.*)" +
                "\\/" +
                "(?<runtime_ver>v\\d+.\\d+.?\\d?)";

            var userAgentString = $"{UserAgentSdkHeaders.ClientName()}/{UserAgentSdkHeaders.SdkVersion()} ({UserAgentSdkHeaders.OsDetails()}) {UserAgentSdkHeaders.RuntimeInfo()}";

            StringAssert.Contains(UserAgentSdkHeaders.FrameworkVersion(), ".NET");
            StringAssert.Contains(UserAgentSdkHeaders.ClientName(), "SignNow .NET API Client");
            StringAssert.Matches(UserAgentSdkHeaders.SdkVersion(), new Regex("v\\d+.\\d+.\\d+.?\\d+"));

            StringAssert.Matches(userAgentString, new Regex(pattern));
        }
    }
}
