using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    public class DocumentGroupRecipientEmailGroup
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class DocumentGroupRecipientReminder
    {
        [JsonProperty("remind_before")]
        public int RemindBefore { get; set; }

        [JsonProperty("remind_after")]
        public int RemindAfter { get; set; }

        [JsonProperty("remind_repeat")]
        public int RemindRepeat { get; set; }
    }

    public class DocumentGroupRecipientAuthentication
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }
    }

    public class DocumentGroupRecipientAttributes
    {
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        [JsonProperty("expiration_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpirationDays { get; set; }

        [JsonProperty("reminder", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupRecipientReminder Reminder { get; set; }

        [JsonProperty("allow_forwarding", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowForwarding { get; set; }

        [JsonProperty("show_decline_button", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowDeclineButton { get; set; }

        [JsonProperty("i_am_recipient", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IAmRecipient { get; set; }

        [JsonProperty("authentication", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupRecipientAuthentication Authentication { get; set; }
    }

    public class DocumentGroupRecipientDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
    }

    public class DocumentGroupRecipient
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("email_group", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupRecipientEmailGroup EmailGroup { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupRecipientAttributes Attributes { get; set; }

        [JsonProperty("documents")]
        public IReadOnlyList<DocumentGroupRecipientDocument> Documents { get; set; }
    }

    public class DocumentGroupUnmappedSignDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("recipient", NullValueHandling = NullValueHandling.Ignore)]
        public string Recipient { get; set; }
    }

    public class DocumentGroupRecipientsData
    {
        [JsonProperty("recipients")]
        public IReadOnlyList<DocumentGroupRecipient> Recipients { get; set; }

        [JsonProperty("unmapped_documents", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<DocumentGroupRecipientDocument> UnmappedDocuments { get; set; }

        [JsonProperty("allowed_unmapped_sign_documents", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<DocumentGroupUnmappedSignDocument> AllowedUnmappedSignDocuments { get; set; }

        [JsonProperty("cc", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<string> Cc { get; set; }

        [JsonProperty("general_expiration_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? GeneralExpirationDays { get; set; }

        [JsonProperty("general_reminder", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupRecipientReminder GeneralReminder { get; set; }

        [JsonProperty("order_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderType { get; set; }
    }

    public class DocumentGroupRecipientsResponse
    {
        [JsonProperty("data")]
        public DocumentGroupRecipientsData Data { get; set; }
    }
}
