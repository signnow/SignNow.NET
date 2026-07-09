using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// Options for generating a link to open the embedded sending workflow.
    /// </summary>
    public class EmbeddedSendingOptions : JsonHttpContent
    {
        /// <summary>
        /// In how many minutes the link expires. Optional.
        /// </summary>
        [JsonProperty("link_expiration", NullValueHandling = NullValueHandling.Ignore)]
        public uint? LinkExpiration { get; set; }

        /// <summary>
        /// URL to redirect to once sending is complete. Optional.
        /// </summary>
        [JsonProperty("redirect_uri", NullValueHandling = NullValueHandling.Ignore)]
        public Uri RedirectUri { get; set; }

        /// <summary>
        /// Whether the redirect should open in the same tab or a new one. Optional.
        /// </summary>
        [JsonProperty("redirect_target", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public RedirectTarget? RedirectTarget { get; set; }
    }
}
