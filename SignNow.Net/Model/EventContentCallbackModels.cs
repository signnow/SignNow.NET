using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Required only to limit possible models in CallbacksResponse
    /// </summary>
    public interface IEventContentCallback
    {
    }

    /// <summary>
    /// Represents content data for document deletion events.
    /// Used for: document.delete, user.document.delete
    /// </summary>
    public class DocumentDeleteEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The document name.
        /// </summary>
        [JsonProperty("document_name")]
        public string DocumentName { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The initiator ID.
        /// </summary>
        [JsonProperty("initiator_id")]
        public string InitiatorId { get; set; }

        /// <summary>
        /// The initiator email.
        /// </summary>
        [JsonProperty("initiator_email")]
        public string InitiatorEmail { get; set; }
    }

    /// <summary>
    /// Represents content data for document update/create/complete events.
    /// Used for: document.update, user.document.update, user.document.create, user.document.complete, document.complete
    /// </summary>

    public class DocumentUpdateEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The document name.
        /// </summary>
        [JsonProperty("document_name")]
        public string DocumentName { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    /// <summary>
    /// Represents content data for document open events.
    /// Used for: document.open, user.document.open
    /// </summary>
    public class DocumentOpenEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The document name.
        /// </summary>
        [JsonProperty("document_name")]
        public string DocumentName { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The viewer user unique ID.
        /// </summary>
        [JsonProperty("viewer_user_unique_id")]
        public string ViewerUserUniqueId { get; set; }
    }

    /// <summary>
    /// Represents content data for template copy events.
    /// Used for: template.copy, user.template.copy
    /// </summary>
    public class TemplateCopyEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The template ID.
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The viewer user unique ID.
        /// </summary>
        [JsonProperty("viewer_user_unique_id")]
        public string ViewerUserUniqueId { get; set; }
    }

    /// <summary>
    /// Represents content data for document invite and form events.
    /// Used for: user.document.fieldinvite.*, document.fieldinvite.*, user.document.freeform.*, document.freeform.*
    /// </summary>
    public class DocumentInviteEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The invite ID.
        /// </summary>
        [JsonProperty("invite_id")]
        public string InviteId { get; set; }

        /// <summary>
        /// The signer email.
        /// </summary>
        [JsonProperty("signer")]
        public string Signer { get; set; }

        /// <summary>
        /// The document status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    /// <summary>
    /// Represents content data for document field invite reassign events.
    /// Used for: user.document.fieldinvite.reassign, document.fieldinvite.reassign
    /// </summary>
    public class DocumentInviteReassignEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The invite ID.
        /// </summary>
        [JsonProperty("invite_id")]
        public string InviteId { get; set; }

        /// <summary>
        /// The signer email.
        /// </summary>
        [JsonProperty("signer")]
        public string Signer { get; set; }

        /// <summary>
        /// The old invite unique ID.
        /// </summary>
        [JsonProperty("old_invite_unique_id")]
        public string OldInviteUniqueId { get; set; }
    }

    /// <summary>
    /// Represents content data for document field invite replace events.
    /// Used for: user.document.fieldinvite.replace, document.fieldinvite.replace
    /// </summary>
    public class DocumentInviteReplaceEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The invite ID.
        /// </summary>
        [JsonProperty("invite_id")]
        public string InviteId { get; set; }

        /// <summary>
        /// The signer email.
        /// </summary>
        [JsonProperty("signer")]
        public string Signer { get; set; }

        /// <summary>
        /// The old invite unique ID.
        /// </summary>
        [JsonProperty("old_invite_unique_id")]
        public string OldInviteUniqueId { get; set; }

        /// <summary>
        /// Indicates if this is a group invite.
        /// </summary>
        [JsonProperty("group_invite")]
        public string GroupInvite { get; set; }
    }

    /// <summary>
    /// Represents content data for document group create/update/complete events.
    /// Used for: user.document_group.create, user.document_group.update, user.document_group.complete, document_group.update, document_group.complete
    /// </summary>
    public class DocumentGroupEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document group ID.
        /// </summary>
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        /// <summary>
        /// The document group name.
        /// </summary>
        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    /// <summary>
    /// Represents content data for document group delete events.
    /// Used for: document_group.delete, user.document_group.delete
    /// </summary>
    public class DocumentGroupDeleteEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document group ID.
        /// </summary>
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        /// <summary>
        /// The document group name.
        /// </summary>
        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The initiator ID.
        /// </summary>
        [JsonProperty("initiator_id")]
        public string InitiatorId { get; set; }

        /// <summary>
        /// The initiator email.
        /// </summary>
        [JsonProperty("initiator_email")]
        public string InitiatorEmail { get; set; }
    }

    /// <summary>
    /// Represents content data for document group invite events.
    /// Used for: user.document_group.invite.*, document_group.invite.*
    /// </summary>
    public class DocumentGroupInviteEventContent : IEventContentCallback
    {
        /// <summary>
        /// The document group ID.
        /// </summary>
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        /// <summary>
        /// The group invite ID.
        /// </summary>
        [JsonProperty("group_invite_id")]
        public string GroupInviteId { get; set; }

        /// <summary>
        /// The invite status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    /// <summary>
    /// Represents the content data in a callback.
    /// </summary>
    public class CallbackContentAllFields : IEventContentCallback
    {
        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The template ID.
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// The invite ID.
        /// </summary>
        [JsonProperty("invite_id")]
        public string InviteId { get; set; }

        /// <summary>
        /// The signer information.
        /// </summary>
        [JsonProperty("signer")]
        public string Signer { get; set; }

        /// <summary>
        /// The status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The old invite unique ID.
        /// </summary>
        [JsonProperty("old_invite_unique_id")]
        public string OldInviteUniqueId { get; set; }

        /// <summary>
        /// The group ID.
        /// </summary>
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        /// <summary>
        /// The group name.
        /// </summary>
        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        /// <summary>
        /// The group invite information.
        /// </summary>
        [JsonProperty("group_invite")]
        public string GroupInvite { get; set; }

        /// <summary>
        /// The group invite ID.
        /// </summary>
        [JsonProperty("group_invite_id")]
        public string GroupInviteId { get; set; }

        /// <summary>
        /// The document name.
        /// </summary>
        [JsonProperty("document_name")]
        public string DocumentName { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The initiator ID.
        /// </summary>
        [JsonProperty("initiator_id")]
        public string InitiatorId { get; set; }

        /// <summary>
        /// The initiator email.
        /// </summary>
        [JsonProperty("initiator_email")]
        public string InitiatorEmail { get; set; }

        /// <summary>
        /// The viewer user unique ID.
        /// </summary>
        [JsonProperty("viewer_user_unique_id")]
        public string ViewerUserUniqueId { get; set; }
    }
}
