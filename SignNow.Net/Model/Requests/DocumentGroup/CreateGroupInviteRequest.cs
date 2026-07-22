using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// A signer to notify by email as part of a document group invite step.
    /// </summary>
    public class GroupInviteEmail
    {
        /// <summary>
        /// Email address of the signer to invite.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Name of the role this signer fulfills.
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Unique identifier of the role this signer fulfills.
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        /// <summary>
        /// Position of this signer within the invite step's signing order.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Subject line of the invite email. Optional.
        /// </summary>
        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        /// <summary>
        /// Message body of the invite email. Optional.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Number of days before the invite expires. Optional.
        /// </summary>
        [JsonProperty("expiration_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpirationDays { get; set; }

        /// <summary>
        /// Number of days after which a reminder email is sent. Optional.
        /// </summary>
        [JsonProperty("reminder", NullValueHandling = NullValueHandling.Ignore)]
        public int? Reminder { get; set; }
    }

    /// <summary>
    /// The signing action a signer must perform on a specific document within a document group invite step.
    /// </summary>
    public class GroupInviteAction
    {
        /// <summary>
        /// Email address of the signer performing the action.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Name of the role this signer fulfills.
        /// </summary>
        [JsonProperty("role_name")]
        public string RoleName { get; set; }

        /// <summary>
        /// Type of action to perform, e.g. "sign".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; } = "sign";

        /// <summary>
        /// Unique identifier of the document this action applies to.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// Whether the signer is allowed to reassign this action to someone else. 1 to allow, 0 to disallow.
        /// </summary>
        [JsonProperty("allow_reassign")]
        public int AllowReassign { get; set; }

        /// <summary>
        /// Whether the signer can decline by providing a signature. 1 to allow, 0 to disallow.
        /// </summary>
        [JsonProperty("decline_by_signature")]
        public int DeclineBySignature { get; set; }
    }

    /// <summary>
    /// A single step of a document group signing workflow, listing the signers to notify and the actions they must perform.
    /// </summary>
    public class GroupInviteStep
    {
        /// <summary>
        /// Position of this step in the overall signing order.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Signers to notify by email as part of this step.
        /// </summary>
        [JsonProperty("invite_emails")]
        public IList<GroupInviteEmail> InviteEmails { get; set; } = new List<GroupInviteEmail>();

        /// <summary>
        /// Signing actions to be performed as part of this step.
        /// </summary>
        [JsonProperty("invite_actions")]
        public IList<GroupInviteAction> InviteActions { get; set; } = new List<GroupInviteAction>();
    }

    /// <summary>
    /// Request to send a signing invite for a document group.
    /// </summary>
    public class CreateGroupInviteRequest : JsonHttpContent
    {
        /// <summary>
        /// Ordered signing steps that make up this group invite.
        /// </summary>
        [JsonProperty("invite_steps")]
        public IList<GroupInviteStep> InviteSteps { get; set; } = new List<GroupInviteStep>();

        /// <summary>
        /// Email addresses to be copied on invite notifications. Optional.
        /// </summary>
        [JsonProperty("cc", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Cc { get; set; }
    }
}
