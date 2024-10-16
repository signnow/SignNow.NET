using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Represents successful status response from signNow API.
    /// </summary>
    public class SuccessStatusResponse
    {
        /// <summary>
        /// Operation status
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
