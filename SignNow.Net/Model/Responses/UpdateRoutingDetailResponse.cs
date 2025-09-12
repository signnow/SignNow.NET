using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response model for updating routing detail information
    /// </summary>
    public class UpdateRoutingDetailResponse
    {
        /// <summary>
        /// Array with routing details
        /// </summary>
        [JsonProperty("template_data")]
        public IReadOnlyList<UpdateRoutingDetailTemplateData> TemplateData { get; set; }

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

        /// <summary>
        /// Routing attributes
        /// </summary>
        [JsonProperty("attributes")]
        public UpdateRoutingDetailAttributes Attributes { get; set; }
    }

    /// <summary>
    /// Put routing detail template data information
    /// </summary>
    public class UpdateRoutingDetailTemplateData
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
    /// Put routing detail CC step information
    /// </summary>
    public class UpdateRoutingDetailCcStep : CcStepBase
    {
    }

    /// <summary>
    /// Put routing detail viewer information
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
    /// Put routing detail approver information
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
        /// Contact ID
        /// </summary>
        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
    }

    /// <summary>
    /// Put routing detail attributes
    /// </summary>
    public class UpdateRoutingDetailAttributes
    {
        /// <summary>
        /// Brand ID
        /// </summary>
        [JsonProperty("brand_id")]
        public string BrandId { get; set; }

        /// <summary>
        /// Redirect URI
        /// </summary>
        [JsonProperty("redirect_uri")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri RedirectUri { get; set; }

        /// <summary>
        /// Close redirect URI
        /// </summary>
        [JsonProperty("close_redirect_uri")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri CloseRedirectUri { get; set; }
    }
}
