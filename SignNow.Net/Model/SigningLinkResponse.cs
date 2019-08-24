using Newtonsoft.Json;
using System;

namespace SignNow.Net.Model
{
    public class SigningLinkResponse
    {
        /// <summary>
        /// URL to sign the document via web browser using SignNow credentials
        /// </summary>
        [JsonProperty("url")]
        public Uri Url { get; set; }
        /// <summary>
        /// URL to sign the document via web browser without SignNow credentials
        /// </summary>
        [JsonProperty("url_no_signup")]
        public Uri AnonymousUrl { get; set; }
    }
}
