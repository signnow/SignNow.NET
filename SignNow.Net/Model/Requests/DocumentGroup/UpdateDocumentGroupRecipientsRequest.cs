using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// A recipient to assign to a document group, along with the documents they must act on.
    /// </summary>
    public class UpdateDocumentGroupRecipientEntry : DocumentGroupRecipientBase
    {
        /// <summary>
        /// Documents and signing actions to assign to this recipient.
        /// </summary>
        [JsonProperty("documents")]
        public IList<DocumentGroupRecipientDocument> Documents { get; set; } = new List<DocumentGroupRecipientDocument>();
    }

    /// <summary>
    /// Request to update recipients, expiration, reminder and signing order settings for a document group.
    /// </summary>
    public class UpdateDocumentGroupRecipientsRequest : JsonHttpContent
    {
        /// <summary>
        /// Recipients to assign to the document group.
        /// </summary>
        [JsonProperty("recipients")]
        public IList<UpdateDocumentGroupRecipientEntry> Recipients { get; set; } = new List<UpdateDocumentGroupRecipientEntry>();

        /// <summary>
        /// Email addresses to be copied on invite notifications. Optional.
        /// </summary>
        [JsonProperty("cc", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Cc { get; set; }

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
        /// Signing order mode. Optional.
        /// </summary>
        [JsonProperty("order_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DocumentGroupOrderType? OrderType { get; set; }
    }
}
