using System;

namespace SignNow.Net.Internal.Model
{
    /// <summary>
    /// Represents SignNow SDK general info
    /// </summary>
    public class SDKInfo
    {
        /// <summary>
        /// SignNow SDK client name
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// SignNow SDK version
        /// </summary>
        public Version Version { get; set; }
    }
}
