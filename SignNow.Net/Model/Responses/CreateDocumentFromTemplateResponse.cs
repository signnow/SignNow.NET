using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Represents response from signNow API for Create Document from Template request.
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
