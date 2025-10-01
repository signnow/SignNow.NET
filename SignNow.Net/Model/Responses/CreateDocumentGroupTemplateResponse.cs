using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response model for creating a document group template
    /// </summary>
    public class CreateDocumentGroupTemplateResponse
    {
        /// <summary>
        /// The ID of the created document group template
        /// Note: This may be null for 202 Accepted responses with empty body
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Status of the operation
        /// Note: This may be null for 202 Accepted responses with empty body
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Indicates if the operation was accepted (202 status)
        /// </summary>
        public bool IsAccepted => string.IsNullOrEmpty(Id) && (string.IsNullOrEmpty(Status) || Status == "accepted");
    }
}
