using SignNow.Net.Internal.Infrastructure;

namespace SignNow.Net.Internal.Helpers
{
    /// <summary>
    /// Creates User-Agent string from current runtime info
    /// </summary>
    internal static class UserAgentSdkHeaders
    {
        /// <summary>
        /// Creates pre-formatted string with SDK, OS, Runtime information
        /// e.g.: client_name>/version (OS_type OS_release; platform; arch) runtime/version
        /// </summary>
        /// <returns></returns>
        public static string BuildUserAgentString()
        {
            return $"{SdkRuntime.ClientName}/v{SdkRuntime.Version.ToString()} " +
                   $"({RuntimeInfo.OsName}; {RuntimeInfo.Platform}; {RuntimeInfo.Arch}) " +
                   $"{SdkRuntime.FrameworkName()}/v{SdkRuntime.FrameworkVersion()}";
        }

        /// <summary>
        /// Returns RAW runtime information with OS details.
        /// </summary>
        /// <returns></returns>
        public static string RawOsDescription()
        {
            return SdkRuntime.OsDescription();
        }
    }
}
