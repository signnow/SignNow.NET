using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Model.Responses.GenericResponses;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Details of a single signer added to an embedded document group invite.
    /// </summary>
    public class DocumentGroupEmbeddedInviteData : IdResponse
    {
        /// <summary>
        /// Email address of the signer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Unique identifier of the role this signer fulfills.
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        /// <summary>
        /// Position of this signer in the signing order.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Current status of the embedded invite, e.g. "pending".
        /// </summary>
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InviteStatus Status { get; set; }
    }

    /// <summary>
    /// Response returned after creating an embedded signing invite for a document group.
    /// </summary>
    public class DocumentGroupEmbeddedInviteResponse
    {
        /// <summary>
        /// Signers added to the embedded document group invite.
        /// </summary>
        [JsonProperty("data")]
        public IReadOnlyList<DocumentGroupEmbeddedInviteData> InviteData { get; internal set; }
    }
}
