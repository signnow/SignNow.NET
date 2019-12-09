using System;
using System.Runtime.CompilerServices;
using SignNow.Net.Internal.Model;

[assembly: InternalsVisibleTo("SignNow.Net.Test")]
namespace SignNow.Net.Internal.Infrastructure
{

#if NET45
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
        private static OsRuntime Runtime;

        static RuntimeInfo()
        {
            Runtime = new OsRuntime
            {
                Name = GetOSName(),
                Arch = GetArchitecture(),
                Platform = GetPlatform()
            };
        }

        /// <summary>
        /// Gets OS name from runtime
        /// </summary>
        /// <returns></returns>
        private static string GetOSName()
        {
            var os = "Unknown";
#if NET45
            os = Environment.OSVersion.ToString();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                os = "Linux";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                os = "macOs";
#endif
            return os;
        }

        /// <summary>
        /// Gets OS architecture from runtime
        /// </summary>
        /// <returns></returns>
        private static string GetArchitecture()
        {
            var arch = "X64";
#if NET45
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
        public static string GetPlatform()
        {
            var platform = "unknown";
#if NET45
            platform = Environment.Is64BitOperatingSystem ? "win64" : "win32";
#else
            platform = RuntimeInformation.ProcessArchitecture.ToString();
#endif
            return platform;
        }

        /// <summary>
        /// Returns OS name (e.g. Linux, macOs, Windows)
        /// </summary>
        /// <returns></returns>
        public static string OsName()
        {
            return Runtime.Name;
        }

        /// <summary>
        /// Returns OS architecture.
        /// </summary>
        /// <returns></returns>
        public static string OsArch()
        {
            return Runtime.Arch;
        }

        /// <summary>
        /// Returns OS platform.
        /// </summary>
        /// <returns></returns>
        public static string OsPlatform()
        {
            return Runtime.Platform;
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
            version = matched.Groups["version"].Value;
#endif
            return $"Linux {version}".Trim();
        }

        public static string GetMacOsVersion(string kernel)
        {
            string version = String.Empty;

#if NETSTANDARD
            /* Darwin 17.5.0 Darwin Kernel Version 17.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64 */
            var matched = new Regex(@"^(?<kernel>\w+)+\s+(?<major>\d+).(?<minor>\d+).(?<patch>\d+)?").Match(kernel.Trim());
            var major = int.Parse(matched.Groups["major"].Value);
            var minor = int.Parse(matched.Groups["minor"].Value);

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

                case (int)MacOsVersions.MacOsX1012:
                    version = "10.12";
                    break;

                case (int)MacOsVersions.MacOsX1013:
                    version = "10.13";
                    break;

                case (int)MacOsVersions.MacOsX1014:
                    version = "10.14";
                    break;

                case (int)MacOsVersions.MacOsX1015:
                    version = "10.15";
                    break;

                default:
                    break;
            }
#endif
            return $"macOs {version}".Trim();
        }

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
            MacOsX1012,
            MacOsX1013,
            MacOsX1014,
            MacOsX1015,
        }
    }
}
