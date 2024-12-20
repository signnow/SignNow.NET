using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SignNow.Net.Internal.Infrastructure;
using System.Text.RegularExpressions;

namespace UnitTests.Helpers
{
    [TestClass]
    public class UserAgentSdkHeadersTest
    {
        [TestMethod]
        public void UserAgentStringIsCorrect()
        {
            // SignNow .NET API Сlient/v1.0.0.0 (Windows_NT 10.0.14393; win32; ia32) .NET Core 3.1/v4.0.30319.42000
            // SignNow .NET API Сlient/v1.0.0.0 (Linux 3.10.0; Unix; X64) .NET Core 3.0/v4.0.30319.42000
            // SignNow .NET API Сlient/v1.0.0.0 (macOs 10.15.0; Unix; X64) .NET Core 3.1/v4.0.30319.42000

            var patternClient = @"(?<client>.*)";
            var patternSdk = @"(?<version>v\d+.\d+.\d+.?\d+)";
            var patternOsDetails = @"(?<os>.*);\s(?<platform>\w+);\s(?<arch>\w+)";
            var patternRuntime = @"(?<runtime>.*)\/(?<runtime_ver>v\d+.\d+(.?\d+)?)";

            var patternUserAgent = patternClient + @"\/" +
                patternSdk + @"\s+" +
                @"\(" + patternOsDetails + @"\)\s" +
                patternRuntime;

            var osdetails = $"({RuntimeInfo.OsName}; {RuntimeInfo.Platform}; {RuntimeInfo.Arch})";
            var runtimeinfo = $"{SdkRuntime.FrameworkName()}/v{SdkRuntime.FrameworkVersion()}";

            var userAgentString = $"{SdkRuntime.ClientName}/v{SdkRuntime.Version} ({RuntimeInfo.OsName}; {RuntimeInfo.Platform}; {RuntimeInfo.Arch}) {SdkRuntime.FrameworkName()}/v{SdkRuntime.FrameworkVersion()}";

            Console.WriteLine("Detected User-Agent string: {0}", userAgentString);

            StringAssert.Contains(SdkRuntime.ClientName, "SignNow .NET API Client");
            StringAssert.Matches(SdkRuntime.ClientName, new Regex(patternClient));
            StringAssert.Matches($"v{SdkRuntime.Version}", new Regex(patternSdk));
            StringAssert.Matches(osdetails, new Regex(patternOsDetails));
            StringAssert.Matches(runtimeinfo, new Regex(patternRuntime));

            StringAssert.Matches(userAgentString, new Regex(patternUserAgent), "Format mismatch: <client>/<version> (<os>; <platform>; <arch>) <runtime_sdk>/<runtime_ver>");
        }

        [TestMethod]
        public void OsDescriptionTest()
        {
            Console.WriteLine("Detected OS description: {0}", SdkRuntime.OsDescription());

            StringAssert.Matches(SdkRuntime.OsDescription().ToLower(), new Regex(@"windows|darwin|macos|osx|linux||ubuntu|debian|redhat|fedora|alpine|freebsd"));
        }

        [TestMethod]
        public void FrameworkNameTest()
        {
            Console.WriteLine("Detected Framework name: {0}", SdkRuntime.FrameworkName());

            StringAssert.Contains(SdkRuntime.FrameworkName(), ".NET");
        }

        [TestMethod]
        public void FrameworkVersionTest()
        {
            Console.WriteLine("Detected Framework version: {0}", SdkRuntime.FrameworkVersion());

            var expectedVersion = "4.";

            #if NETCOREAPP3_0 || NETCOREAPP3_1
                expectedVersion = "3.";
            #elif NET7_0
                expectedVersion = "7.";
            #elif NET8_0
                expectedVersion = "8.";
            #endif

            StringAssert.StartsWith(SdkRuntime.FrameworkVersion(), expectedVersion);
        }
    }
}
