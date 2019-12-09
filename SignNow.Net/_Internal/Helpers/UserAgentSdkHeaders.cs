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
            return $"{Infrastructure.RuntimeInfo.OsName()}; {Infrastructure.RuntimeInfo.OsPlatform()}; {Infrastructure.RuntimeInfo.OsArch()}";
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

        /// <summary>
        /// Creates pre-formatted string with SDK, OS, Runtime information
        /// </summary>
        /// <returns></returns>
        public static string BuildUserAgentString()
        {
            return $"{UserAgentSdkHeaders.ClientName()}/{UserAgentSdkHeaders.SdkVersion()} ({UserAgentSdkHeaders.OsDetails()}) {UserAgentSdkHeaders.RuntimeInfo()}";
        }
    }
}
