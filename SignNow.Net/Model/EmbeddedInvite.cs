using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represent Embedded signing invite params.
    /// </summary>
    public class EmbeddedInvite
    {
        private string email { get; set; }
        private uint signingOrder { get; set; }

        /// <summary>
        /// Signer's email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email
        {
            get { return email; }
            set { email = value.ValidateEmail(); }
        }

        /// <summary>
        /// Signer's role ID.
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        /// <summary>
        /// Order of signing. Cannot be 0.
        /// </summary>
        [JsonProperty("order")]
        public uint SigningOrder
        {
            get { return signingOrder; }
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("Value cannot be 0", nameof(SigningOrder));
                }

                signingOrder = value;
            }
        }

        /// <summary>
        /// Signer authentication method.
        /// </summary>
        [JsonProperty("auth_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbeddedAuthType AuthMethod { get; set; } = EmbeddedAuthType.None;
    }
}
