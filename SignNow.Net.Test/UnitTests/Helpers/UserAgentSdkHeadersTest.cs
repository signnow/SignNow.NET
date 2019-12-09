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
            /// SignNow .NET API Сlient/v1.0.0.0 (Mac OSX 18.5.0; Unix; X64) .NET Core/v4.0.30319.42000

            var patternClient = @"(?<client>.*)";
            var patternSdk = @"(?<version>v\d+.\d+.\d+.?\d+)";
            var patternOsDetails = @"(?<os>.*);\s(?<platform>\w+);\s(?<arch>\w+)";
            var patternRuntime = @"(?<runtime>.*)\/(?<runtime_ver>v\d+.\d+(.?\d+)?)";

            var patternUserAgent = patternClient + @"\/" +
                patternSdk + @"\s+" +
                @"\(" + patternOsDetails + @"\)\s" +
                patternRuntime;

            var userAgentString = $"{UserAgentSdkHeaders.ClientName()}/{UserAgentSdkHeaders.SdkVersion()} ({UserAgentSdkHeaders.OsDetails()}) {UserAgentSdkHeaders.RuntimeInfo()}";

            StringAssert.Contains(UserAgentSdkHeaders.ClientName(), "SignNow .NET API Client");
            StringAssert.Matches(UserAgentSdkHeaders.ClientName(), new Regex(patternClient));
            StringAssert.Matches(UserAgentSdkHeaders.SdkVersion(), new Regex(patternSdk));
            StringAssert.Matches(UserAgentSdkHeaders.OsDetails(), new Regex(patternOsDetails));
            StringAssert.Matches(UserAgentSdkHeaders.RuntimeInfo(), new Regex(patternRuntime));

            StringAssert.Matches(userAgentString, new Regex(patternUserAgent), "Format mismatch: <client>/<version> (<os>; <platform>; <arch>) <runtime_sdk>/<runtime_ver>");
            Console.WriteLine(userAgentString);
        }
    }
}
