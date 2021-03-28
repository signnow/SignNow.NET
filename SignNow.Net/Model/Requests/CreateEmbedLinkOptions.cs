using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Exceptions;

namespace SignNow.Net.Model.Requests
{
    public class CreateEmbedLinkOptions
    {
        private uint? linkExpiration { get; set; }

        /// <summary>
        /// Signer authentication method.
        /// </summary>
        [JsonProperty("auth_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbeddedAuthType AuthMethod { get; set; } = EmbeddedAuthType.None;

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
                    throw new ArgumentException(ExceptionMessages.AllowedRangeMustBeFrom15to45, nameof(LinkExpiration));
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

    public enum EmbeddedAuthType
    {
        [EnumMember(Value = "password")]
        Password,

        [EnumMember(Value = "email")]
        Email,

        [EnumMember(Value = "mfa")]
        Mfa,

        [EnumMember(Value = "social")]
        Social,

        [EnumMember(Value = "biometric")]
        Biometric,

        [EnumMember(Value = "other")]
        Other,

        [EnumMember(Value = "none")]
        None
    }
}
