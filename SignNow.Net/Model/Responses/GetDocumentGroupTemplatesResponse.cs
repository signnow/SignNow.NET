using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response model for getting document group templates
    /// </summary>
    public class GetDocumentGroupTemplatesResponse
    {
        /// <summary>
        /// List of document group templates
        /// </summary>
        [JsonProperty("document_group_templates")]
        public IReadOnlyList<DocumentGroupTemplate> DocumentGroupTemplates { get; set; }

        /// <summary>
        /// Total count of document group templates
        /// </summary>
        [JsonProperty("document_group_template_total_count")]
        public int DocumentGroupTemplateTotalCount { get; set; }
    }

    /// <summary>
    /// Document group template model
    /// </summary>
    public class DocumentGroupTemplate
    {
        /// <summary>
        /// Folder ID where the template is stored
        /// </summary>
        [JsonProperty("folder_id")]
        public string FolderId { get; set; }

        /// <summary>
        /// Last updated timestamp
        /// </summary>
        [JsonProperty("last_updated")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Template group ID
        /// </summary>
        [JsonProperty("template_group_id")]
        public string TemplateGroupId { get; set; }

        /// <summary>
        /// Template group name
        /// </summary>
        [JsonProperty("template_group_name")]
        public string TemplateGroupName { get; set; }

        /// <summary>
        /// Owner email
        /// </summary>
        [JsonProperty("owner_email")]
        public string OwnerEmail { get; set; }

        /// <summary>
        /// List of templates in this group
        /// </summary>
        [JsonProperty("templates")]
        public IReadOnlyList<DocumentGroupTemplateItem> Templates { get; set; }

        /// <summary>
        /// Whether the template is prepared
        /// </summary>
        [JsonProperty("is_prepared")]
        public bool IsPrepared { get; set; }

        /// <summary>
        /// Routing details for the template
        /// </summary>
        [JsonProperty("routing_details")]
        public DocumentGroupTemplateRoutingDetails RoutingDetails { get; set; }
    }

    /// <summary>
    /// Document group template item model
    /// </summary>
    public class DocumentGroupTemplateItem
    {
        /// <summary>
        /// Template ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Template name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Template thumbnail URLs
        /// </summary>
        [JsonProperty("thumbnail")]
        public Thumbnail Thumbnail { get; set; }

        /// <summary>
        /// List of roles for this template
        /// </summary>
        [JsonProperty("roles")]
        public IReadOnlyList<string> Roles { get; set; }
    }


    /// <summary>
    /// Document group template routing details model
    /// </summary>
    public class DocumentGroupTemplateRoutingDetails
    {
        /// <summary>
        /// Whether to sign as merged
        /// </summary>
        [JsonProperty("sign_as_merged")]
        public bool SignAsMerged { get; set; }

        /// <summary>
        /// Include email attachments
        /// </summary>
        [JsonProperty("include_email_attachments")]
        public object IncludeEmailAttachments { get; set; }

        /// <summary>
        /// List of invite steps
        /// </summary>
        [JsonProperty("invite_steps")]
        public IReadOnlyList<DocumentGroupTemplateInviteStep> InviteSteps { get; set; }
    }

    /// <summary>
    /// Document group template invite step model
    /// </summary>
    public class DocumentGroupTemplateInviteStep
    {
        /// <summary>
        /// Order of the step
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// List of invite emails
        /// </summary>
        [JsonProperty("invite_emails")]
        public IReadOnlyList<DocumentGroupTemplateInviteEmail> InviteEmails { get; set; }

        /// <summary>
        /// List of invite actions
        /// </summary>
        [JsonProperty("invite_actions")]
        public IReadOnlyList<DocumentGroupTemplateInviteAction> InviteActions { get; set; }
    }

    /// <summary>
    /// Document group template invite email model
    /// </summary>
    public class DocumentGroupTemplateInviteEmail
    {
        /// <summary>
        /// Email address
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Email subject
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Email message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Reminder settings
        /// </summary>
        [JsonProperty("reminder")]
        public DocumentGroupTemplateReminder Reminder { get; set; }

        /// <summary>
        /// Expiration days
        /// </summary>
        [JsonProperty("expiration_days")]
        public int ExpirationDays { get; set; }

        /// <summary>
        /// Whether has sign actions
        /// </summary>
        [JsonProperty("has_sign_actions")]
        public bool HasSignActions { get; set; }
    }

    /// <summary>
    /// Document group template reminder model
    /// </summary>
    public class DocumentGroupTemplateReminder
    {
        /// <summary>
        /// Remind before days
        /// </summary>
        [JsonProperty("remind_before")]
        public int RemindBefore { get; set; }

        /// <summary>
        /// Remind after days
        /// </summary>
        [JsonProperty("remind_after")]
        public int RemindAfter { get; set; }

        /// <summary>
        /// Remind repeat days
        /// </summary>
        [JsonProperty("remind_repeat")]
        public int RemindRepeat { get; set; }
    }

    /// <summary>
    /// Document group template invite action model
    /// </summary>
    public class DocumentGroupTemplateInviteAction
    {
        /// <summary>
        /// Email address
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Authentication settings
        /// </summary>
        [JsonProperty("authentication")]
        public DocumentGroupTemplateAuthentication Authentication { get; set; }

        /// <summary>
        /// UUID
        /// </summary>
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        /// <summary>
        /// Allow reassign
        /// </summary>
        [JsonProperty("allow_reassign")]
        public int AllowReassign { get; set; }

        /// <summary>
        /// Decline by signature
        /// </summary>
        [JsonProperty("decline_by_signature")]
        public int DeclineBySignature { get; set; }

        /// <summary>
        /// Action type
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        [JsonProperty("role_name")]
        public string RoleName { get; set; }

        /// <summary>
        /// Document ID
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// Document name
        /// </summary>
        [JsonProperty("document_name")]
        public string DocumentName { get; set; }
    }

    /// <summary>
    /// Document group template authentication model
    /// </summary>
    public class DocumentGroupTemplateAuthentication
    {
        /// <summary>
        /// Authentication type
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AuthenticationInfoType? Type { get; set; }
    }
}
