using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Request model for updating document group template
    /// </summary>
    public class UpdateDocumentGroupTemplateRequest : JsonHttpContent
    {
        /// <summary>
        /// List of document IDs in the document group template
        /// </summary>
        [JsonProperty("order")]
        public IList<string> Order { get; set; } = new List<string>();

        /// <summary>
        /// Name of the document group template
        /// </summary>
        [JsonProperty("template_group_name")]
        public string TemplateGroupName { get; set; }

        /// <summary>
        /// Specifies the action to be taken upon invite completion. 
        /// Allowed values: documents_and_attachments, documents_and_attachments_only_to_recipients, without_documents_and_attachments
        /// </summary>
        [JsonProperty("email_action_on_complete")]
        public string EmailActionOnComplete { get; set; }
    }
}
