using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests
{
    public class EmbeddedEditorOptions : JsonHttpContent
    {
        [JsonProperty("link_expiration", NullValueHandling = NullValueHandling.Ignore)]
        public uint? LinkExpiration { get; set; }

        [JsonProperty("redirect_uri", NullValueHandling = NullValueHandling.Ignore)]
        public Uri RedirectUri { get; set; }

        [JsonProperty("redirect_target", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public RedirectTarget? RedirectTarget { get; set; }
    }
}
