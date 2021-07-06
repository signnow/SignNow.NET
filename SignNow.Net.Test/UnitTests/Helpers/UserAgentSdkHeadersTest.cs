using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SignNow.Net.Internal.Infrastructure;
using System.Text.RegularExpressions;

namespace UnitTests
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

            #if DEBUG
            Console.WriteLine(userAgentString);
            #endif

            StringAssert.Contains(SdkRuntime.ClientName, "SignNow .NET API Client");
            StringAssert.Matches(SdkRuntime.ClientName, new Regex(patternClient));
            StringAssert.Matches($"v{SdkRuntime.Version.ToString()}", new Regex(patternSdk));
            StringAssert.Matches(osdetails, new Regex(patternOsDetails));
            StringAssert.Matches(runtimeinfo, new Regex(patternRuntime));

            StringAssert.Matches(userAgentString, new Regex(patternUserAgent), "Format mismatch: <client>/<version> (<os>; <platform>; <arch>) <runtime_sdk>/<runtime_ver>");
        }

        [TestMethod]
        public void OsDescriptionTest()
        {
            #if DEBUG
            Console.WriteLine(SdkRuntime.OsDescription());
            #endif

            var expectedOs = "Darwin";

            if (RuntimeInfo.OsName == "Linux")
                expectedOs = "Linux";

            if (RuntimeInfo.OsName == "Windows")
                expectedOs = "Windows";

            StringAssert.Contains(SdkRuntime.OsDescription(), expectedOs);
        }

        [TestMethod]
        public void FrameworkNameTest()
        {
            #if DEBUG
            Console.WriteLine(SdkRuntime.FrameworkName());
            #endif

            StringAssert.Contains(SdkRuntime.FrameworkName(), ".NET");
        }

        [TestMethod]
        public void FrameworkVersionTest()
        {
            #if DEBUG
            Console.WriteLine(SdkRuntime.FrameworkVersion());
            #endif

            var expectedVersion = "4.";

            #if NETCOREAPP3_0 || NETCOREAPP3_1
                expectedVersion = "3.";
            #elif NET5_0
                expectedVersion = "5.";
            #endif

            StringAssert.StartsWith(SdkRuntime.FrameworkVersion(), expectedVersion);
        }
    }
}
