using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Represents response from signNow API for Create Template from Document request.
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
