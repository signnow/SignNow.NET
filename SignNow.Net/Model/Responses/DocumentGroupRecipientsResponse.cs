using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Common fields shared by every recipient of a document group, regardless of whether
    /// it comes from a "get recipients" response or an "update recipients" request.
    /// </summary>
    public abstract class DocumentGroupRecipientBase
    {
        /// <summary>
        /// Name of the recipient. Optional.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Email address of the recipient. Optional.
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>
        /// Position of this recipient in the signing order.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }
    }

    /// <summary>
    /// A group of recipients who share the same email address, e.g. a distribution list.
    /// </summary>
    public class DocumentGroupRecipientEmailGroup
    {
        /// <summary>
        /// Unique identifier of the email group.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    /// <summary>
    /// The authentication method a recipient must pass before opening the document.
    /// </summary>
    public class DocumentGroupRecipientAuthentication
    {
        /// <summary>
        /// Type of authentication required, e.g. password or phone verification. Optional.
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Value used to authenticate, e.g. the expected password. Optional.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>
        /// Phone number used for phone-based authentication. Optional.
        /// </summary>
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        /// <summary>
        /// Authentication delivery method. Optional.
        /// </summary>
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }
    }

    /// <summary>
    /// Per-recipient invite settings such as message text, expiration and reminders.
    /// </summary>
    public class DocumentGroupRecipientAttributes
    {
        /// <summary>
        /// Message body of the invite email. Optional.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Subject line of the invite email. Optional.
        /// </summary>
        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        /// <summary>
        /// Number of days before the invite expires. Optional.
        /// </summary>
        [JsonProperty("expiration_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpirationDays { get; set; }

        /// <summary>
        /// Reminder schedule for this recipient. Optional.
        /// </summary>
        [JsonProperty("reminder", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupTemplateReminder Reminder { get; set; }

        /// <summary>
        /// Whether this recipient is allowed to forward the invite to someone else. Optional.
        /// </summary>
        [JsonProperty("allow_forwarding", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowForwarding { get; set; }

        /// <summary>
        /// Whether the "decline" button is shown to this recipient. Optional.
        /// </summary>
        [JsonProperty("show_decline_button", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowDeclineButton { get; set; }

        /// <summary>
        /// Whether the account owner is themselves the recipient of this step. Optional.
        /// </summary>
        [JsonProperty("i_am_recipient", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IAmRecipient { get; set; }

        /// <summary>
        /// Authentication requirements for this recipient. Optional.
        /// </summary>
        [JsonProperty("authentication", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupRecipientAuthentication Authentication { get; set; }
    }

    /// <summary>
    /// A document and the signing action a recipient must perform on it.
    /// </summary>
    public class DocumentGroupRecipientDocument
    {
        /// <summary>
        /// Unique identifier of the document.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the role this recipient fulfills on the document.
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Action the recipient must perform, e.g. "sign".
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }
    }

    /// <summary>
    /// A recipient of a document group, as returned by the API.
    /// </summary>
    public class DocumentGroupRecipient : DocumentGroupRecipientBase
    {
        /// <summary>
        /// Email group this recipient belongs to, if any. Optional.
        /// </summary>
        [JsonProperty("email_group", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupRecipientEmailGroup EmailGroup { get; set; }

        /// <summary>
        /// Invite settings for this recipient, such as message, expiration and reminders. Optional.
        /// </summary>
        [JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupRecipientAttributes Attributes { get; set; }

        /// <summary>
        /// Documents and signing actions assigned to this recipient.
        /// </summary>
        [JsonProperty("documents")]
        public IReadOnlyList<DocumentGroupRecipientDocument> Documents { get; set; }
    }

    /// <summary>
    /// A document that has not been assigned to any recipient, along with the action still required on it.
    /// </summary>
    public class DocumentGroupUnmappedSignDocument
    {
        /// <summary>
        /// Unique identifier of the document.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the role expected to act on the document.
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Email address of the recipient this document could be assigned to. Optional.
        /// </summary>
        [JsonProperty("recipient", NullValueHandling = NullValueHandling.Ignore)]
        public string Recipient { get; set; }
    }

    /// <summary>
    /// Recipients, expiration, reminder and signing order settings for a document group.
    /// </summary>
    public class DocumentGroupRecipientsData
    {
        /// <summary>
        /// Recipients currently assigned to the document group.
        /// </summary>
        [JsonProperty("recipients")]
        public IReadOnlyList<DocumentGroupRecipient> Recipients { get; set; }

        /// <summary>
        /// Documents in the group that have not been assigned to a recipient yet. Optional.
        /// </summary>
        [JsonProperty("unmapped_documents", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<DocumentGroupRecipientDocument> UnmappedDocuments { get; set; }

        /// <summary>
        /// Unmapped documents that are still allowed to be signed. Optional.
        /// </summary>
        [JsonProperty("allowed_unmapped_sign_documents", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<DocumentGroupUnmappedSignDocument> AllowedUnmappedSignDocuments { get; set; }

        /// <summary>
        /// Email addresses copied on invite notifications. Optional.
        /// </summary>
        [JsonProperty("cc", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<string> Cc { get; set; }

        /// <summary>
        /// Default number of days before an invite expires, applied when a recipient doesn't set its own. Optional.
        /// </summary>
        [JsonProperty("general_expiration_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? GeneralExpirationDays { get; set; }

        /// <summary>
        /// Default reminder schedule, applied when a recipient doesn't set its own. Optional.
        /// </summary>
        [JsonProperty("general_reminder", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupTemplateReminder GeneralReminder { get; set; }

        /// <summary>
        /// Signing order mode, e.g. "at_the_same_time", "recipient_order" or "advanced_order". Optional.
        /// </summary>
        [JsonProperty("order_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderType { get; set; }
    }

    /// <summary>
    /// Response returned when getting the recipients of a document group.
    /// </summary>
    public class DocumentGroupRecipientsResponse
    {
        /// <summary>
        /// Recipient and signing order data for the document group.
        /// </summary>
        [JsonProperty("data")]
        public DocumentGroupRecipientsData Data { get; set; }
    }
}
