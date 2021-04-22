using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from SignNow API for Create Template from Document request.
    /// </summary>
    [JsonObject]
    public class CreateTemplateFromDocumentResponse
    {
        /// <summary>
        /// Identity of new Template.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
