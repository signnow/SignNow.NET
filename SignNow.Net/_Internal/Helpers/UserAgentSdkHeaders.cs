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
#endif

    internal static class UserAgentSdkHeaders
    {
        /// <summary>
        /// Detect runtime framework
        /// </summary>
        /// <returns></returns>
        public static string FrameworkVersion()
        {
#if NET45
            return ".NET Framework 4.5+";
#else
            return RuntimeInformation.FrameworkDescription;
#endif
        }

        public static string ClientName()
        {
            return "SignNow .NET API Client";
        }

        public static string SdkVersion()
        {
            return "v0.0.1";
        }

        public static string OsDetails()
        {
            var os = "Unknown";
            var ver = "0.0.0";
            var platform = "unknown";
            var arch = "X86";
#if NET45
            os = Environment.OSVersion.ToString();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                os = "Linux";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                os = "Mac OSX";
#endif
            return $"{os} {ver}; {platform}; {arch}";
        }

        public static string RuntimeInfo()
        {
#if NET45
            return $".NET 4.5+/v{Environment.Version.ToString()}";
#else
            return $".NET Core/v{RuntimeInformation.FrameworkDescription.Replace(".NET Core ", String.Empty)}";
#endif
        }

    }
}
