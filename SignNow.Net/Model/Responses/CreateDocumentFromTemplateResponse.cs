using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from SignNow API for Create Document from Template request.
    /// </summary>
    public class CreateDocumentFromTemplateResponse
    {
        /// <summary>
        /// Identity of new Document.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
