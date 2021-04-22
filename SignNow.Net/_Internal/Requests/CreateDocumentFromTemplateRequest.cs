using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Internal.Requests
{
    internal class CreateDocumentFromTemplateRequest : IContent
    {
        /// <summary>
        /// The new document name.
        /// </summary>
        [JsonProperty("document_name")]
        public string DocumentName { get; set; }

        /// <summary>
        /// ID of the template which is the source of a document
        /// </summary>
        [JsonProperty("template_id ")]
        public string TemplateId { get; set; }

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
