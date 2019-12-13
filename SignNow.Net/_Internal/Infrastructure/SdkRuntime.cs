using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("SignNow.Net.Test")]
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
#if NET45
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
            return ".NET 4.5+";
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
#if NET45
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
            var assemblyFQN = typeof(SdkRuntime).AssemblyQualifiedName.Split(',');

            var version = Array.Find<string>(assemblyFQN, (string obj) => obj.Contains("Version=")).Trim();

            var parsed = version.Replace("Version=", String.Empty);

            return new Version(parsed);
        }
    }
}
