using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// Request model for updating routing detail information
    /// </summary>
    public class PutRoutingDetailRequest : IContent
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
        public IReadOnlyList<PutRoutingDetailData> Data { get; set; }

        /// <summary>
        /// Array of cc's emails
        /// </summary>
        [JsonProperty("cc")]
        public IReadOnlyList<string> Cc { get; set; }

        /// <summary>
        /// Array of cc's steps
        /// </summary>
        [JsonProperty("cc_step")]
        public IReadOnlyList<PutCcStep> CcStep { get; set; }

        /// <summary>
        /// Invite link instruction
        /// </summary>
        [JsonProperty("invite_link_instructions")]
        public string InviteLinkInstructions { get; set; }

        /// <summary>
        /// Array of viewers
        /// </summary>
        [JsonProperty("viewers")]
        public IReadOnlyList<PutViewer> Viewers { get; set; }

        /// <summary>
        /// Array of approvers
        /// </summary>
        [JsonProperty("approvers")]
        public IReadOnlyList<PutApprover> Approvers { get; set; }

        /// <summary>
        /// Gets the HTTP content for the PUT routing detail request
        /// </summary>
        /// <returns>HTTP content representing the request</returns>
        public System.Net.Http.HttpContent GetHttpContent()
        {
            var json = JsonConvert.SerializeObject(this);
            return new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }
    }

    /// <summary>
    /// Put routing detail data information
    /// </summary>
    public class PutRoutingDetailData
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
    /// Put CC step information
    /// </summary>
    public class PutCcStep
    {
        /// <summary>
        /// Email of cc step
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Step number
        /// </summary>
        [JsonProperty("step")]
        public int Step { get; set; }

        /// <summary>
        /// Name of cc step
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Put viewer information
    /// </summary>
    public class PutViewer
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
    /// Put approver information
    /// </summary>
    public class PutApprover
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
