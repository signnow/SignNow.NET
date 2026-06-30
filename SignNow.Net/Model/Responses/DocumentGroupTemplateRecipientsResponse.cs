using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    public class DocumentGroupTemplateRecipientRole
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("signing_order")]
        public int SigningOrder { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
    }

    public class DocumentGroupTemplateRecipientStep
    {
        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("recipients")]
        public IReadOnlyList<DocumentGroupTemplateRecipientRole> Recipients { get; set; }
    }

    public class DocumentGroupTemplateRecipientsData
    {
        [JsonProperty("invite_steps")]
        public IReadOnlyList<DocumentGroupTemplateRecipientStep> InviteSteps { get; set; }
    }

    public class DocumentGroupTemplateRecipientsResponse
    {
        [JsonProperty("data")]
        public DocumentGroupTemplateRecipientsData Data { get; set; }
    }
}
