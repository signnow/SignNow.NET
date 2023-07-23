using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    public class EventUpdateResponse
    {
        /// <summary>
        /// Event subscription identity
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
