using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    public class GroupInviteEmail
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("expiration_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpirationDays { get; set; }

        [JsonProperty("reminder", NullValueHandling = NullValueHandling.Ignore)]
        public int? Reminder { get; set; }
    }

    public class GroupInviteAction
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role_name")]
        public string RoleName { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; } = "sign";

        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        [JsonProperty("allow_reassign")]
        public string AllowReassign { get; set; } = "0";

        [JsonProperty("decline_by_signature")]
        public string DeclineBySignature { get; set; } = "0";
    }

    public class GroupInviteStep
    {
        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("invite_emails")]
        public IList<GroupInviteEmail> InviteEmails { get; set; } = new List<GroupInviteEmail>();

        [JsonProperty("invite_actions")]
        public IList<GroupInviteAction> InviteActions { get; set; } = new List<GroupInviteAction>();
    }

    public class CreateGroupInviteRequest : JsonHttpContent
    {
        [JsonProperty("invite_steps")]
        public IList<GroupInviteStep> InviteSteps { get; set; } = new List<GroupInviteStep>();

        [JsonProperty("cc", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Cc { get; set; }
    }
}
