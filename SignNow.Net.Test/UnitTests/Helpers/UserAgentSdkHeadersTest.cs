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

        [DataTestMethod]
        [DataRow("Linux 4.4.0 - 43 - Microsoft #1-Microsoft Wed Dec 31 14:42:53 PST 2014", "Linux 4.4.0", DisplayName="Short Linux version")]
        [DataRow("Linux 3.10.0-693.21.1.el7.x86_64 #1 SMP Wed Mar 7 19:03:37 UTC 2018", "Linux 3.10.0-693.21.1.el7.x86_64", DisplayName="Long Linux version")]
        [DataRow("Linux bad3.10.0-693.21.1.el7.x86_64 #1 SMP Wed Mar 7 19:03:37 UTC 2018", "Linux", DisplayName="Broken Linux version")]
        [DataRow("CentOs 3.10.0 - 693.21.1.el7.x86_64 #1 SMP Wed Mar 7 19:03:37 UTC 2018", "Linux 3.10.0", DisplayName="Not a Linux")]
#if NET45
        [Ignore]
#endif
        public void ShouldProperParseLinuxOsDetails(string kernelStr, string expected)
        {
            var actual = UserAgentSdkHeaders.GetLinuxVersion(kernelStr);

            Assert.AreEqual(actual, expected);
        }

        [DataTestMethod]
        [DataRow("Darwin 1.2.0 Darwin Kernel Version 1.2.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.0", DisplayName="Cheetah")]
        [DataRow("Darwin 1.4.0 Darwin Kernel Version 1.4.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.1", DisplayName="Puma")]
        [DataRow("Darwin 6.5.0 Darwin Kernel Version 6.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.2", DisplayName="Jaguar")]
        [DataRow("Darwin 7.5.0 Darwin Kernel Version 7.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.3", DisplayName="Panter")]
        [DataRow("Darwin 8.5.0 Darwin Kernel Version 8.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.4", DisplayName="Tiger")]
        [DataRow("Darwin 9.5.0 Darwin Kernel Version 9.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.5", DisplayName="Leopard")]
        [DataRow("Darwin 10.5.0 Darwin Kernel Version 10.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.6", DisplayName="Snow Leopard")]
        [DataRow("Darwin 11.5.0 Darwin Kernel Version 11.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.7", DisplayName="Lion")]
        [DataRow("Darwin 12.5.0 Darwin Kernel Version 12.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.8", DisplayName="Mountain Lion")]
        [DataRow("Darwin 13.5.0 Darwin Kernel Version 13.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.9", DisplayName="Mavericks")]
        [DataRow("Darwin 14.5.0 Darwin Kernel Version 14.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.10", DisplayName="Yosemite")]
        [DataRow("Darwin 15.5.0 Darwin Kernel Version 15.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.11", DisplayName="El Capitan")]
        [DataRow("Darwin 16.5.0 Darwin Kernel Version 16.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.12", DisplayName="Sierra")]
        [DataRow("Darwin 17.5.0 Darwin Kernel Version 17.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.13", DisplayName="High Sierra")]
        [DataRow("Darwin 18.5.0 Darwin Kernel Version 18.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.14", DisplayName="Mojave")]
        [DataRow("Darwin 19.0.0 Darwin Kernel Version 19.0.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs 10.15", DisplayName="Catalina")]
        [DataRow("Darwin 99.0.0 Darwin Kernel Version 99.0.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "macOs", DisplayName="Unknown")]
#if NET45
        [Ignore]
#endif
        public void ShouldProperParseMacOsDetails(string kernelStr, string expected)
        {
            var actual = UserAgentSdkHeaders.GetMacOsVersion(kernelStr);

            Assert.AreEqual(actual, expected);
        }
    }
}
