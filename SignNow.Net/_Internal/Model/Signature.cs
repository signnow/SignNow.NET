using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Internal.Model
{
    /// <summary>
    /// Represents SignNow signature object.
    /// </summary>
    internal class Signature
    {
        /// <summary>
        /// Identity of the signature.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Identity of user that sign the document.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Identity of the signature request.
        /// </summary>
        [JsonProperty("signature_request_id")]
        public string SignatureRequestId { get; set; }

        /// <summary>
        /// <see cref="SignNow.Net.Model.User"/> email who was signed the document.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Timestamp document was signed.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }
    }
}
