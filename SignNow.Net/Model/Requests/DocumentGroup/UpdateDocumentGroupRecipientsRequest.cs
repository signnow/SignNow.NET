using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    public class UpdateDocumentGroupRecipientEntry
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("documents")]
        public IList<DocumentGroupRecipientDocument> Documents { get; set; } = new List<DocumentGroupRecipientDocument>();
    }

    public class UpdateDocumentGroupRecipientsRequest : JsonHttpContent
    {
        [JsonProperty("recipients")]
        public IList<UpdateDocumentGroupRecipientEntry> Recipients { get; set; } = new List<UpdateDocumentGroupRecipientEntry>();

        [JsonProperty("cc", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Cc { get; set; }

        [JsonProperty("general_expiration_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? GeneralExpirationDays { get; set; }

        [JsonProperty("general_reminder", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentGroupRecipientReminder GeneralReminder { get; set; }

        [JsonProperty("order_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderType { get; set; }
    }
}
