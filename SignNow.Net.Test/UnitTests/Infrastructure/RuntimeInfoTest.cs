using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Infrastructure;
using System.Text.RegularExpressions;

namespace UnitTests
{
    [TestClass]
    public class RuntimeInfoTest
    {
        [DataTestMethod]
        [DataRow("Linux 4.4.0 - 43 - Microsoft #1-Microsoft Wed Dec 31 14:42:53 PST 2014", "4.4.0", DisplayName = "Short Linux version")]
        [DataRow("Linux 3.10.0-693.21.1.el7.x86_64 #1 SMP Wed Mar 7 19:03:37 UTC 2018", "3.10.0-693.21.1.el7.x86_64", DisplayName = "Long Linux version")]
        [DataRow("Linux bad3.10.0-693.21.1.el7.x86_64 #1 SMP Wed Mar 7 19:03:37 UTC 2018", "", DisplayName = "Broken Linux version")]
        [DataRow("CentOs 3.10.0 - 693.21.1.el7.x86_64 #1 SMP Wed Mar 7 19:03:37 UTC 2018", "3.10.0", DisplayName = "Not a Linux")]
#if NET45
        [Ignore]
#endif
        public void ShouldProperParseLinuxOsDetails(string kernelStr, string expected)
        {
            var actual = RuntimeInfo.GetLinuxVersion(kernelStr);

            Assert.AreEqual(actual, expected);
        }

#if NET45
        [Ignore]
#endif
        [DataTestMethod]
        [DataRow("Darwin 1.2.0 Darwin Kernel Version 1.2.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.0", DisplayName = "Cheetah")]
        [DataRow("Darwin 1.4.0 Darwin Kernel Version 1.4.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.1", DisplayName = "Puma")]
        [DataRow("Darwin 6.5.0 Darwin Kernel Version 6.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.2", DisplayName = "Jaguar")]
        [DataRow("Darwin 7.5.0 Darwin Kernel Version 7.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.3", DisplayName = "Panther")]
        [DataRow("Darwin 8.5.0 Darwin Kernel Version 8.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.4", DisplayName = "Tiger")]
        [DataRow("Darwin 9.5.0 Darwin Kernel Version 9.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.5", DisplayName = "Leopard")]
        [DataRow("Darwin 10.5.0 Darwin Kernel Version 10.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.6", DisplayName = "Snow Leopard")]
        [DataRow("Darwin 11.5.0 Darwin Kernel Version 11.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.7", DisplayName = "Lion")]
        [DataRow("Darwin 12.5.0 Darwin Kernel Version 12.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.8", DisplayName = "Mountain Lion")]
        [DataRow("Darwin 13.5.0 Darwin Kernel Version 13.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.9", DisplayName = "Mavericks")]
        [DataRow("Darwin 14.5.0 Darwin Kernel Version 14.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.10", DisplayName = "Yosemite")]
        [DataRow("Darwin 15.5.0 Darwin Kernel Version 15.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.11", DisplayName = "El Capitan")]
        [DataRow("Darwin 16.5.0 Darwin Kernel Version 16.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.12", DisplayName = "Sierra")]
        [DataRow("Darwin 17.5.0 Darwin Kernel Version 17.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.13", DisplayName = "High Sierra")]
        [DataRow("Darwin 18.5.0 Darwin Kernel Version 18.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.14", DisplayName = "Mojave")]
        [DataRow("Darwin 19.0.0 Darwin Kernel Version 19.0.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "10.15", DisplayName = "Catalina")]
        [DataRow("Darwin 99.0.0 Darwin Kernel Version 99.0.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64", "", DisplayName = "Unknown")]
        public void ShouldProperParseMacOsDetails(string kernelStr, string expected)
        {
            var actual = RuntimeInfo.GetMacOsVersion(kernelStr);

            Assert.AreEqual(actual, expected);
        }

#if NET45
        [Ignore]
#endif
        [DataTestMethod]
        [DataRow("Microsoft Windows 10.0.18363", "10.0.18363", DisplayName = "FQN windows string")]
        [DataRow("Microsoft Windows 10.0", "10.0", DisplayName = "Only <major>.<minor>")]
        [DataRow("nonMicrosoft nonWindows no-10.0", "", DisplayName = "Broken string")]
        public void ShouldProperParseWindowsVersionNetStandard(string osDetails, string expected)
        {
            var actual = RuntimeInfo.GetWindowsVersion(osDetails);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ShouldProperCollectOsInfo()
        {
            var patternOsName = @"(?<name>\w+)+\s+(?<version>\d+.?\d+(?:\S)\S+)";

            StringAssert.Matches(RuntimeInfo.OsName, new Regex(patternOsName));
        }
    }
}
