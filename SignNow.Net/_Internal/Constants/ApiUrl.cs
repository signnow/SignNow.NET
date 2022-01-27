using System;

namespace SignNow.Net.Internal.Constants
{
    /// <summary>
    /// Determines base signNow API URL.
    /// </summary>
    internal static class ApiUrl
    {
#if DEBUG
        /// <summary>
        /// Base signNow API URL for Debug configuration.
        /// </summary>
        public static Uri ApiBaseUrl = new Uri("https://api-eval.signnow.com");
#else
        /// <summary>
        /// Base signNow API URL for Release configuration.
        /// </summary>
        public static Uri ApiBaseUrl = new Uri("https://api.signnow.com");
#endif
    }
}
