using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// Request model for updating routing detail information
    /// </summary>
    public class UpdateRoutingDetailRequest : JsonHttpContent
    {
        /// <summary>
        /// Unique id of template routing detail
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Unique id of document
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// Array with routing details
        /// </summary>
        [JsonProperty("data")]
        public IReadOnlyList<RoutingDetailData> Data { get; set; }

        /// <summary>
        /// Array of cc's emails
        /// </summary>
        [JsonProperty("cc")]
        public IReadOnlyList<string> Cc { get; set; }

        /// <summary>
        /// Array of cc's steps
        /// </summary>
        [JsonProperty("cc_step")]
        public IReadOnlyList<UpdateRoutingDetailCcStep> CcStep { get; set; }

        /// <summary>
        /// Invite link instruction
        /// </summary>
        [JsonProperty("invite_link_instructions")]
        public string InviteLinkInstructions { get; set; }

        /// <summary>
        /// Array of viewers
        /// </summary>
        [JsonProperty("viewers")]
        public IReadOnlyList<UpdateRoutingDetailViewer> Viewers { get; set; }

        /// <summary>
        /// Array of approvers
        /// </summary>
        [JsonProperty("approvers")]
        public IReadOnlyList<UpdateRoutingDetailApprover> Approvers { get; set; }

    }

    /// <summary>
    /// Routing detail data information
    /// </summary>
    public class RoutingDetailData
    {
        /// <summary>
        /// Default email for routing detail
        /// </summary>
        [JsonProperty("default_email")]
        public string DefaultEmail { get; set; }

        /// <summary>
        /// Always false
        /// </summary>
        [JsonProperty("inviter_role")]
        public bool InviterRole { get; set; }

        /// <summary>
        /// Signer role (actor) name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Signer role (actor) unique_id
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        /// <summary>
        /// Signer order from actor table
        /// </summary>
        [JsonProperty("signer_order")]
        public int SignerOrder { get; set; }

        /// <summary>
        /// Decline by signature flag
        /// </summary>
        [JsonProperty("decline_by_signature")]
        public bool? DeclineBySignature { get; set; }
    }

    /// <summary>
    /// CC step information
    /// </summary>
    public class UpdateRoutingDetailCcStep : CcStepBase
    {
    }

    /// <summary>
    /// Viewer information
    /// </summary>
    public class UpdateRoutingDetailViewer
    {
        /// <summary>
        /// Default email for viewer
        /// </summary>
        [JsonProperty("default_email")]
        public string DefaultEmail { get; set; }

        /// <summary>
        /// Viewer name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Signing order
        /// </summary>
        [JsonProperty("signing_order")]
        public int SigningOrder { get; set; }

        /// <summary>
        /// Always false
        /// </summary>
        [JsonProperty("inviter_role")]
        public bool InviterRole { get; set; }

        /// <summary>
        /// Contact ID
        /// </summary>
        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
    }

    /// <summary>
    /// Approver information
    /// </summary>
    public class UpdateRoutingDetailApprover
    {
        /// <summary>
        /// Default email for approver
        /// </summary>
        [JsonProperty("default_email")]
        public string DefaultEmail { get; set; }

        /// <summary>
        /// Approver name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Signing order
        /// </summary>
        [JsonProperty("signing_order")]
        public int SigningOrder { get; set; }

        /// <summary>
        /// Always false
        /// </summary>
        [JsonProperty("inviter_role")]
        public bool InviterRole { get; set; }

        /// <summary>
        /// Expiration days
        /// </summary>
        [JsonProperty("expiration_days")]
        public int? ExpirationDays { get; set; }

        /// <summary>
        /// Contact ID
        /// </summary>
        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
    }
}
