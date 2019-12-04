using System;

namespace SignNow.Net.Internal.Helpers
{

#if NET45
    using System.Reflection;
    using Microsoft.Win32;
#else
    using System.Runtime.InteropServices;
#endif

    // <client name>/<version> (<OS type> <OS release>; <platform>; <arch>) <runtime>/<version>
    // e.g. SignNow Node.js API Сlient/v1.5.0 (Windows_NT 10.0.14393; win32; ia32) node/v8.16.1
    public static class UserAgentSdkHeaders
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

        public static string OsType()
        {
            var os = "Unknown";
#if NET45
            os = Environment.OSVersion.Platform.ToString();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                os = "Linux";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                os = "Mac OSX";
#endif
            return os;
        }

        public static string OsRelease()
        {
#if NET45
            return Environment.OSVersion.Version.ToString();
#else
            return RuntimeInformation.OSDescription;
#endif
        }




        public static string OsDetails()
        {
#if NET45
            return Environment.OSVersion.ToString();
#else
            return RuntimeInformation.OSArchitecture.ToString();
#endif
        }

        public static string RuntimeInfo()
        {
#if NET45
            return "";
#else
            return "none";
#endif
        }

        public static string OsInfo()
        {
#if NET45
            return Environment.OSVersion.Platform.ToString();
#else
            return RuntimeInformation.OSArchitecture.ToString();
#endif
        }
    }
}
