using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// Embedded sending mode.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmbeddedSendingType
    {
        /// <summary>
        /// Recipient can manage the document group (default).
        /// </summary>
        [EnumMember(Value = "manage")]
        Manage,

        /// <summary>
        /// Recipient can edit the document group.
        /// </summary>
        [EnumMember(Value = "edit")]
        Edit,

        /// <summary>
        /// Recipient can send the document group for signing.
        /// </summary>
        [EnumMember(Value = "send-invite")]
        SendInvite
    }

    /// <summary>
    /// Options for generating a link to open the embedded sending workflow.
    /// </summary>
    public class EmbeddedSendingOptions : JsonHttpContent
    {
        /// <summary>
        /// In how many minutes the link expires. Optional.
        /// </summary>
        [JsonProperty("link_expiration", NullValueHandling = NullValueHandling.Ignore)]
        public uint? LinkExpiration { get; set; }

        /// <summary>
        /// URL to redirect to once sending is complete. Optional.
        /// </summary>
        [JsonProperty("redirect_uri", NullValueHandling = NullValueHandling.Ignore)]
        public Uri RedirectUri { get; set; }

        /// <summary>
        /// Whether the redirect should open in the same tab or a new one. Optional.
        /// </summary>
        [JsonProperty("redirect_target", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public RedirectTarget? RedirectTarget { get; set; }

        /// <summary>
        /// Embedded sending mode.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbeddedSendingType Type { get; set; } = EmbeddedSendingType.Manage;
    }
}
