using System;
using System.Globalization;

namespace SignNow.Net.Internal.Infrastructure
{

#if NETFRAMEWORK
    using System.Reflection;
    using Microsoft.Win32;
#else
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
#endif

    /// <summary>
    /// Detect OS name, platform and architecture.
    /// </summary>
    internal static class RuntimeInfo
    {
        /// <summary>
        /// Returns OS name (e.g. Linux, macOs, Windows)
        /// </summary>
        public static string OsName { get; set; }

        /// <summary>
        /// Returns OS architecture.
        /// </summary>
        public static string Arch { get; set; }

        /// <summary>
        /// Returns OS platform.
        /// </summary>
        public static string Platform { get; set; }

        static RuntimeInfo()
        {
            OsName = $"{GetOSName()} {GetOsVersion()}".Trim();
            Arch = GetArchitecture();
            Platform = GetPlatform();
        }

        /// <summary>
        /// Gets OS name from runtime
        /// </summary>
        /// <returns></returns>
        private static string GetOSName()
        {
            var os = "Unknown";
#if NETFRAMEWORK
            os = Environment.OSVersion.ToString();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                os = "Linux";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                os = "macOS";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                os = "Windows";
#endif
            return os;
        }

#pragma warning disable CS0162 // Unreachable code detected
        /// <summary>
        /// Gets OS version from runtime
        /// </summary>
        /// <returns></returns>
        private static string GetOsVersion()
        {
            var version = string.Empty;
#if NETFRAMEWORK
            // On Windows we already have FQ string, e.g: WINDOWS_NT 10.11
            return version;
#else
            // For known Unix-like OS we should parse version from OS description string
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                version = GetLinuxVersion(RuntimeInformation.OSDescription);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                version = GetMacOsVersion(RuntimeInformation.OSDescription);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                version = GetWindowsVersion(RuntimeInformation.OSDescription);
#endif
            return version ?? "0.0.0";
        }
#pragma warning restore CS0162 // Unreachable code detected

        /// <summary>
        /// Gets OS architecture from runtime
        /// </summary>
        /// <returns></returns>
        private static string GetArchitecture()
        {
            var arch = "x86_64";
#if NETFRAMEWORK
            arch = typeof(RuntimeInfo).Assembly.GetName().ProcessorArchitecture.ToString();
#else
            arch = RuntimeInformation.OSArchitecture.ToString();
#endif
            return arch;
        }

        /// <summary>
        /// Gets OS platform from runtime.
        /// </summary>
        /// <returns></returns>
        private static string GetPlatform()
        {
            var platform = "unknown";
#if NETFRAMEWORK
            platform = Environment.Is64BitOperatingSystem ? "win64" : "win32";
#else
            platform = RuntimeInformation.ProcessArchitecture.ToString();
#endif
            return platform;
        }

#pragma warning disable CA1801 // Parameter kernel of method GetMacOsVersion is never used. Remove the parameter or use it in the method body.
        /// <summary>
        /// Get Windows version from string with os details.
        /// </summary>
        /// <param name="osDescription"></param>
        /// <returns></returns>
        public static string GetWindowsVersion(string osDescription)
        {
            string version = String.Empty;
#if NETSTANDARD
            // Microsoft Windows 10.0.18363
            var matched = new Regex(@"(?<name>\w+)+\s+(?<version>\d+.?\d+(?:\S)\S+)").Match(osDescription.Trim());
            version = matched.Groups["version"].Value.Trim();
#endif
            return version;
        }

        /// <summary>
        /// Get Linux version from string with kernel details.
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        public static string GetLinuxVersion(string kernel)
        {
            string version = String.Empty;
#if NETSTANDARD
            // Linux 4.4.0 - 43 - Microsoft #1-Microsoft Wed Dec 31 14:42:53 PST 2014
            // Linux 3.10.0-693.21.1.el7.x86_64 #1 SMP Wed Mar 7 19:03:37 UTC 2018
            var matched = new Regex(@"^(?<kernel>\w+)+\s+(?<version>\d+.?\d+(?:\S)\S+)").Match(kernel.Trim());
            version = matched.Groups["version"].Value.Trim();
#endif
            return version;
        }

        /// <summary>
        /// Get MacOs version from string with kernel details.
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        public static string GetMacOsVersion(string kernel)
        {
            string version = String.Empty;

#if NETSTANDARD
            /* Darwin 17.5.0 Darwin Kernel Version 17.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64 */
            var matched = new Regex(@"^(?<kernel>\w+)+\s+(?<major>\d+).(?<minor>\d+).(?<patch>\d+)?").Match(kernel.Trim());
            int major = int.Parse(matched.Groups["major"].Value, CultureInfo.InvariantCulture);
            int minor = int.Parse(matched.Groups["minor"].Value, CultureInfo.InvariantCulture);

            switch (major)
            {
                case (int)MacOsVersions.MacOsX101:
                    version = minor == 4 ? "10.1" : "10.0";
                    break;

                case (int)MacOsVersions.MacOsX102:
                    version = "10.2";
                    break;

                case (int)MacOsVersions.MacOsX103:
                    version = "10.3";
                    break;

                case (int)MacOsVersions.MacOsX104:
                    version = "10.4";
                    break;

                case (int)MacOsVersions.MacOsX105:
                    version = "10.5";
                    break;

                case (int)MacOsVersions.MacOsX106:
                    version = "10.6";
                    break;

                case (int)MacOsVersions.MacOsX107:
                    version = "10.7";
                    break;

                case (int)MacOsVersions.MacOsX108:
                    version = "10.8";
                    break;

                case (int)MacOsVersions.MacOsX109:
                    version = "10.9";
                    break;

                case (int)MacOsVersions.MacOsX1010:
                    version = "10.10";
                    break;

                case (int)MacOsVersions.MacOsX1011:
                    version = "10.11";
                    break;

                case (int)MacOsVersions.macOS1012:
                    version = "10.12";
                    break;

                case (int)MacOsVersions.macOS1013:
                    version = "10.13";
                    break;

                case (int)MacOsVersions.macOS1014:
                    version = "10.14";
                    break;

                case (int)MacOsVersions.macOS1015:
                    version = "10.15";
                    break;

                case (int)MacOsVersions.macOS1100:
                    version = "11.0";
                    break;

                case (int)MacOsVersions.macOS1200:
                    version = "12.0";
                    break;

                default:
                    break;
            }
#endif
            return version;
        }
#pragma warning restore CA1801 // Parameter kernel of method GetMacOsVersion is never used. Remove the parameter or use it in the method body.

        /// <summary>
        /// MacOS version based on `Major` octet
        /// </summary>
        internal enum MacOsVersions
        {
            MacOsX101 = 1,
            MacOsX102 = 6,
            MacOsX103,
            MacOsX104,
            MacOsX105,
            MacOsX106,
            MacOsX107,
            MacOsX108,
            MacOsX109,
            MacOsX1010,
            MacOsX1011,
            macOS1012,
            macOS1013,
            macOS1014,
            macOS1015,
            macOS1100,
            macOS1200
        }
    }
}
