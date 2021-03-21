using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class EmbeddedSigningLinkRequest : IContent
    {
        private CreateEmbedLinkOptions LinkOptions { get; set; }

        /// <inheritdoc cref="CreateEmbedLinkOptions.AuthTypeMethodAsync" />
        [JsonProperty("auth_method")]
        public EmbeddedAuthType AuthMethod => LinkOptions.AuthTypeMethodAsync;

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

        public HttpContent GetHttpContent()
        {
            return new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
        }
    }
}
