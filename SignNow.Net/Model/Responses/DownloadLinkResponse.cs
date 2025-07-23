using System;
using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response for creating one-time link to download document with embedded fields and elements.
    /// Link can be used once and then will expire.
    /// </summary>
    public class DownloadLinkResponse
    {
        /// <summary>
        /// Link to download specified document in PDF format.
        /// </summary>
        [JsonProperty("link")]
        public Uri Url { get; set; }
    }
}
