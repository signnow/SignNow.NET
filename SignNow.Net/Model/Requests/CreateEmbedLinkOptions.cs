using System;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests
{
    public class CreateEmbedLinkOptions
    {
        private uint? linkExpiration { get; set; }

        /// <summary>
        /// Signer authentication method.
        /// </summary>
        [JsonProperty("auth_method")]
        public string AuthMethod { get; set; } = "none";

        /// <summary>
        /// In how many minutes the link expires, ranges from 15 to 45 minutes or null.
        /// </summary>
        [JsonProperty("link_expiration")]
        public uint? LinkExpiration
        {
            get { return linkExpiration; }
            set
            {
                if (value < 15 || value > 45)
                {
                    throw new ArgumentException("Allowed ranges must be from 15 to 45 minutes", nameof(LinkExpiration));
                }

                linkExpiration = value;
            }
        }

        /// <summary>
        /// Signature invite you'd like to embed.
        /// </summary>
        [JsonIgnore]
        public FieldInvite FieldInvite { get; set; }
    }
}
