using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from signNow API for create embedded invite request.
    /// </summary>
    public class EmbeddedInviteData
    {
        /// <summary>
        /// Identity of embedded invite request.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Signer's email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Signer's role ID.
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        /// <summary>
        /// Order of signing. Cannot be 0.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Current signing status.
        /// </summary>
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InviteStatus Status { get; set; }
    }

    public class EmbeddedInviteResponse
    {
        /// <summary>
        /// Returns the invite data for newly created embedded invite.
        /// </summary>
        [JsonProperty("data")]
        public IReadOnlyList<EmbeddedInviteData> InviteData { get; internal set; }
    }
}
