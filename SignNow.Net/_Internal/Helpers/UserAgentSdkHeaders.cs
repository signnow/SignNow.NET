using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SignNow.Net.Test")]
namespace SignNow.Net.Internal.Helpers
{
    /// <summary>
    /// Creates User-Agent string from current runtime info
    /// </summary>
    internal static class UserAgentSdkHeaders
    {
        /// <summary>
        /// Returns SDK version (e.g: v1.0.0.0)
        /// </summary>
        /// <returns></returns>
        public static string SdkVersion()
        {
            return $"v{Infrastructure.SdkRuntime.Version().ToString()}";
        }

        /// <summary>
        /// Detect OS name, platform and architecture and returns formatted string representation
        /// </summary>
        /// <returns></returns>
        public static string OsDetails()
        {
            return $"{Infrastructure.RuntimeInfo.OsName()}; {Infrastructure.RuntimeInfo.OsPlatform()}; {Infrastructure.RuntimeInfo.OsArch()}";
        }

        /// <summary>
        /// Returns runtime framework name and version
        /// </summary>
        /// <returns></returns>
        public static string RuntimeInfo()
        {
            return $"{Infrastructure.SdkRuntime.FrameworkName()}/v{Infrastructure.SdkRuntime.FrameworkVersion()}";
        }

        /// <summary>
        /// Creates pre-formatted string with SDK, OS, Runtime information
        /// </summary>
        /// <returns></returns>
        public static string BuildUserAgentString()
        {
            return $"{Infrastructure.SdkRuntime.ClientName()}/{UserAgentSdkHeaders.SdkVersion()} ({UserAgentSdkHeaders.OsDetails()}) {UserAgentSdkHeaders.RuntimeInfo()}";
        }
    }
}
