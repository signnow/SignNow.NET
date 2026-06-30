using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    public class UpdateDocumentGroupTemplateRecipientsRequest : JsonHttpContent
    {
        [JsonProperty("invite_steps")]
        public IList<DocumentGroupTemplateRecipientStep> InviteSteps { get; set; } = new List<DocumentGroupTemplateRecipientStep>();
    }
}
