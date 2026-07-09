using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Options for generating a signing link for an embedded document group invite.
    /// </summary>
    public class CreateDocumentGroupEmbedLinkOptions : JsonHttpContent
    {
        /// <summary>
        /// Authentication method the signer must pass before opening the link.
        /// </summary>
        [JsonProperty("auth_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbeddedAuthType AuthMethod { get; set; } = EmbeddedAuthType.None;

        /// <summary>
        /// In how many minutes the link expires. Optional.
        /// </summary>
        [JsonProperty("link_expiration", NullValueHandling = NullValueHandling.Ignore)]
        public uint? LinkExpiration { get; set; }
    }
}
