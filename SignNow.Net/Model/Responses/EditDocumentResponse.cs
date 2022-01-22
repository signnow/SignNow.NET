using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Represents response for edit document.
    /// </summary>
    public class EditDocumentResponse
    {
        /// <summary>
        /// Identity of the document.
        /// </summary>
        [JsonProperty("id")]
        public string DocumentId { get; set; }
    }
}
