using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from signNow API for create invite request.
    /// </summary>
    public class InviteResponse
    {
        /// <summary>
        /// Identity of freeform invite request.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Role-based invite status
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
