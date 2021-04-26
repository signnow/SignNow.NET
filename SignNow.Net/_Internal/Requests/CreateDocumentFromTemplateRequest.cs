using Newtonsoft.Json;

namespace SignNow.Net.Internal.Requests
{
    internal class CreateDocumentFromTemplateRequest
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
