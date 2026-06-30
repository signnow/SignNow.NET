using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    public class CreateDocumentGroupEmbedLinkOptions : JsonHttpContent
    {
        [JsonProperty("auth_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbeddedAuthType AuthMethod { get; set; } = EmbeddedAuthType.None;

        [JsonProperty("link_expiration", NullValueHandling = NullValueHandling.Ignore)]
        public uint? LinkExpiration { get; set; }
    }
}
