using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Represents response from signNow API for Bulk Invite Template request.
    /// </summary>
    [JsonObject]
    public class BulkInviteTemplateResponse
    {
        /// <summary>
        /// Status of the bulk invite job.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
