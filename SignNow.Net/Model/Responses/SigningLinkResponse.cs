using Newtonsoft.Json;
using System;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from signNow API for signing link creation request.
    /// </summary>
    public class SigningLinkResponse
    {
        /// <summary>
        /// URL to sign the document via web browser using signNow credentials.
        /// </summary>
        [JsonProperty("url")]
        public Uri Url { get; set; }

        /// <summary>
        /// URL to sign the document via web browser without signNow credentials.
        /// </summary>
        [JsonProperty("url_no_signup")]
        public Uri AnonymousUrl { get; set; }
    }
}
