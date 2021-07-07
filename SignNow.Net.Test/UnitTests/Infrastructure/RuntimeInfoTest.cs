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
#if NETFRAMEWORK
        [Ignore]
#endif
        public void ShouldProperParseLinuxOsDetails(string kernelStr, string expected)
        {
            var actual = RuntimeInfo.GetLinuxVersion(kernelStr);

            Assert.AreEqual(actual, expected);
        }

#if NETFRAMEWORK
        [Ignore]
#endif
        [DataTestMethod]
        [DataRow("1.2.0", "10.0", DisplayName = "Cheetah")]
        [DataRow("1.4.0", "10.1", DisplayName = "Puma")]
        [DataRow("6.5.0", "10.2", DisplayName = "Jaguar")]
        [DataRow("7.5.0", "10.3", DisplayName = "Panther")]
        [DataRow("8.5.0", "10.4", DisplayName = "Tiger")]
        [DataRow("9.5.0", "10.5", DisplayName = "Leopard")]
        [DataRow("10.5.0", "10.6", DisplayName = "Snow Leopard")]
        [DataRow("11.5.0", "10.7", DisplayName = "Lion")]
        [DataRow("12.5.0", "10.8", DisplayName = "Mountain Lion")]
        [DataRow("13.5.0", "10.9", DisplayName = "Mavericks")]
        [DataRow("14.5.0", "10.10", DisplayName = "Yosemite")]
        [DataRow("15.5.0", "10.11", DisplayName = "El Capitan")]
        [DataRow("16.5.0", "10.12", DisplayName = "Sierra")]
        [DataRow("17.5.0", "10.13", DisplayName = "High Sierra")]
        [DataRow("18.5.0", "10.14", DisplayName = "Mojave")]
        [DataRow("19.0.0", "10.15", DisplayName = "Catalina")]
        [DataRow("20.0.0", "11.0", DisplayName = "Big Sur")]
        [DataRow("21.0.0", "12.0", DisplayName = "Monterey")]
        [DataRow("99.0.0", "", DisplayName = "Unknown")]
        public void ShouldProperParseMacOsDetails(string kernelVer, string expected)
        {
            var kernelStr = $"Darwin {kernelVer} Darwin Kernel Version {kernelVer}: Mon Mar  5 22:24:32 PST 2021; root:xnu-7195.121.3~9/RELEASE_X86_64";
            var actual = RuntimeInfo.GetMacOsVersion(kernelStr);

            Assert.AreEqual(actual, expected);
        }

#if NETFRAMEWORK
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
