using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests
{
    public class CreateEmbedLinkOptions
    {
        /// <summary>
        /// Signer authentication method.
        /// </summary>
        [JsonProperty("auth_method")]
        public string AuthMethod { get; set; } = "none";

        /// <summary>
        /// In how many minutes the link expires, ranges from 15 to 45 minutes or null.
        /// </summary>
        [JsonProperty("link_expiration")]
        public int? LinkExpiration { get; set; }

        /// <summary>
        /// Signature invite you'd like to embed.
        /// </summary>
        [JsonIgnore]
        public FieldInvite FieldInvite { get; set; }
    }
}
