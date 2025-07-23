using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class EmbeddedSigningLinkRequest : JsonHttpContent
    {
        private CreateEmbedLinkOptions LinkOptions { get; set; }

        /// <inheritdoc cref="CreateEmbedLinkOptions.AuthMethod" />
        [JsonProperty("auth_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbeddedAuthType AuthMethod => LinkOptions.AuthMethod;

        /// <inheritdoc cref="CreateEmbedLinkOptions.LinkExpiration" />
        [JsonProperty("link_expiration")]
        public uint? LinkExpiration => LinkOptions.LinkExpiration;

        /// <summary>
        /// Embedded Signing Link Request ctor.
        /// </summary>
        /// <param name="options">options to create link for embedded signing.</param>
        public EmbeddedSigningLinkRequest(CreateEmbedLinkOptions options)
        {
            LinkOptions = options;
        }
    }
}
