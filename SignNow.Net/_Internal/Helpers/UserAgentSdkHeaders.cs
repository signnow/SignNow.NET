using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SignNow.Net.Test")]
namespace SignNow.Net.Internal.Helpers
{

#if NET45
    using System.Reflection;
    using Microsoft.Win32;
#else
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
#endif

    internal static class UserAgentSdkHeaders
    {
        /// <summary>
        /// Returns SDK client's name
        /// </summary>
        /// <returns></returns>
        public static string ClientName()
        {
            return "SignNow .NET API Client";
        }

        /// <summary>
        /// Returns SDK version (e.g: v1.0.0.0)
        /// </summary>
        /// <returns></returns>
        public static string SdkVersion()
        {
            var assemblyFQN = typeof(UserAgentSdkHeaders).AssemblyQualifiedName.Split(',');
            var version = assemblyFQN.GetValue(2).ToString().Trim();

            return version.Replace("Version=", "v");
        }

        /// <summary>
        /// Detect OS name, platform and architecture and returns formatted string representation
        /// </summary>
        /// <returns></returns>
        public static string OsDetails()
        {
            var os = "Unknown";
            var platform = "unknown";
            var arch = "X64";
#if NET45
            os = Environment.OSVersion.ToString();
            platform = Environment.Is64BitOperatingSystem() ? "win64" : "win32";
            arch = typeof(UserAgentSdkHeaders).Assembly.GetName().ProcessorArchitecture.ToString();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                os = "Linux";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                os = GetMacOsVersion();
            }

            platform = RuntimeInformation.ProcessArchitecture.ToString();
            arch = RuntimeInformation.OSArchitecture.ToString();
#endif
            return $"{os}; {platform}; {arch}";
        }

        /// <summary>
        /// Returns runtime framework name and version
        /// </summary>
        /// <returns></returns>
        public static string RuntimeInfo()
        {
#if NET45
            return $".NET 4.5+/v{Environment.Version.ToString()}";
#else
            return $".NET Core/v{RuntimeInformation.FrameworkDescription.Replace(".NET Core ", String.Empty)}";
#endif
        }

        public static string GetMacOsVersion()
        {
            string version = String.Empty;

#if NETSTANDARD
            /* Darwin 17.5.0 Darwin Kernel Version 17.5.0: Mon Mar  5 22:24:32 PST 2018; root:xnu-4570.51.1~1/RELEASE_X86_64 */
            var macOsKernel = RuntimeInformation.OSDescription;
            var matched = new Regex(@"^(?<kernel>\w+)+\s+(?<major>\d+).(?<minor>\d+).(?<patch>\d+)?").Match(macOsKernel.Trim());
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
