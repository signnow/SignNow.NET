using System;
using System.Runtime.InteropServices;

namespace SignNow.Net.Internal.Infrastructure
{
    /// <summary>
    /// Represents SignNow SDK general info (name, version...)
    /// </summary>
    internal static class SdkRuntime
    {
        /// <summary>
        /// Returns SignNow SDK client name
        /// </summary>
        public const string ClientName = "SignNow .NET API Client";

        /// <summary>
        /// Returns SignNow SDK version.
        /// </summary>
        public static Version Version { get; set; }

        static SdkRuntime()
        {
            SdkRuntime.Version = ParseVersion();
        }

        public static string OsDescription()
        {
#if NETFRAMEWORK
            return Environment.OSVersion.ToString();
#else
            return RuntimeInformation.OSDescription;
#endif
        }

        /// <summary>
        /// Returns framework name for current runtime
        /// </summary>
        /// <returns></returns>
        public static string FrameworkName()
        {
#if NET45
            return ".NET 4.5";
#elif NET451
            return ".NET 4.5.1";
#elif NET452
            return ".NET 4.5.2";
#elif NET46
            return ".NET 4.6";
#elif NET461
            return ".NET 4.6.1";
#elif NET462
            return ".NET 4.6.2";
#elif NET47
            return ".NET 4.7";
#elif NET471
            return ".NET 4.7.1";
#elif NET472
            return ".NET 4.7.2";
#elif NET48
            return ".NET 4.8";
#elif NET5_0
            return ".NET 5.0";
#elif NETCOREAPP3_1
            return ".NET Core 3.1";
#elif NETCOREAPP3_0
            return ".NET Core 3.0";
#else
            return ".NET Core";
#endif
        }

        /// <summary>
        /// Returns framework version for current runtime
        /// </summary>
        /// <returns></returns>
        public static string FrameworkVersion()
        {
#if NETFRAMEWORK
            return Environment.Version.ToString();
#else
            return RuntimeInformation.FrameworkDescription.Replace(".NET Core ", String.Empty);
#endif
        }

        /// <summary>
        /// Parse SKD version from Assembly info
        /// </summary>
        /// <returns></returns>
        private static Version ParseVersion()
        {
            var assemblyFqn =
                typeof(SdkRuntime).AssemblyQualifiedName?.Split(',')
                ?? new[] {"Version=0.0.0.0"};

            var version = Array.Find(assemblyFqn, obj => obj.Contains("Version=")).Trim();
            var parsed = version.Replace("Version=", String.Empty);

            return new Version(parsed);
        }
    }
}
