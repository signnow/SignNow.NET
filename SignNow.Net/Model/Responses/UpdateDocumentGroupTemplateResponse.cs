using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response model for updating document group template
    /// </summary>
    public class UpdateDocumentGroupTemplateResponse
    {
        /// <summary>
        /// Operation status
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
