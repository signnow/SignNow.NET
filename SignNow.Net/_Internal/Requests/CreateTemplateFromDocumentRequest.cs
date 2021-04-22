using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Internal.Requests
{
    internal class CreateTemplateFromDocumentRequest : IContent
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

        /// <summary>
        /// Creates Json Http Content from object
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent GetHttpContent()
        {
            return new StringContent(
                JsonConvert.SerializeObject(this),
                Encoding.UTF8, "application/json");
        }
    }
}
