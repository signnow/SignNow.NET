using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.Requests
{
    public class CreateTemplateFromDocumentRequest
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
    }
}
