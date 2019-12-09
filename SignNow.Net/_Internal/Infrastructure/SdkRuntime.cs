using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SignNow.Net.Internal.Model;

[assembly: InternalsVisibleTo("SignNow.Net.Test")]
namespace SignNow.Net.Internal.Infrastructure
{
    /// <summary>
    /// Represents SignNow SDK general info (name, version...)
    /// </summary>
    internal static class SdkRuntime
    {
        private static SDKInfo SignNowSDK;

        static SdkRuntime()
        {
            SignNowSDK = new SDKInfo
            {
                ClientName = "SignNow .NET API Client",
                Version = ParseVersion()
            };
        }

        /// <summary>
        /// Returns SignNow SDK client name
        /// </summary>
        /// <returns></returns>
        public static string ClientName()
        {
            return SignNowSDK.ClientName;
        }

        /// <summary>
        /// Returns SignNow SDK version.
        /// </summary>
        /// <returns></returns>
        public static Version Version()
        {
            return SignNowSDK.Version;
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
