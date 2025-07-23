using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class CreateTemplateFromDocumentRequest : JsonHttpContent
    {
        /// <summary>
        /// The new template name.
        /// </summary>
        [JsonProperty("document_name")]
        public string TemplateName { get; set; }

        /// <summary>
        /// ID of the document which is the source of a template
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        public CreateTemplateFromDocumentRequest(string templateName, string documentId)
        {
            TemplateName = templateName;
            DocumentId = documentId;
        }
    }
}
