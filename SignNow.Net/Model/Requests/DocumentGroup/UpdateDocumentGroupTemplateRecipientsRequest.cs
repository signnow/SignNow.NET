using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Request to update the recipients of a document group template.
    /// </summary>
    public class UpdateDocumentGroupTemplateRecipientsRequest : JsonHttpContent
    {
        /// <summary>
        /// Ordered signing steps with the roles to assign to this template.
        /// </summary>
        [JsonProperty("invite_steps")]
        public IList<DocumentGroupTemplateRecipientStep> InviteSteps { get; set; } = new List<DocumentGroupTemplateRecipientStep>();
    }
}
