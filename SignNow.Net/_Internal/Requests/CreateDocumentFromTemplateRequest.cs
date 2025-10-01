using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class CreateDocumentFromTemplateRequest : JsonHttpContent
    {
        /// <summary>
        /// The new document name.
        /// </summary>
        [JsonProperty("document_name")]
        public string DocumentName { get; set; }

        public CreateDocumentFromTemplateRequest(string documentName)
        {
            DocumentName = documentName;
        }
    }
}
