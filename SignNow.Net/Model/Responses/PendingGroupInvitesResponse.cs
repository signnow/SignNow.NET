using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// A signer within a document group invite who has not yet signed.
    /// </summary>
    public class PendingGroupInvite
    {
        /// <summary>
        /// Unique identifier of the pending invite.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Email address of the signer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Name of the role this signer fulfills.
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Current status of the pending invite, e.g. "pending".
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    /// <summary>
    /// Response listing the signers who have not yet signed a document group.
    /// </summary>
    public class PendingGroupInvitesResponse
    {
        /// <summary>
        /// Signers who have not yet completed their signing action.
        /// </summary>
        [JsonProperty("data")]
        public IReadOnlyList<PendingGroupInvite> Data { get; set; }
    }
}
