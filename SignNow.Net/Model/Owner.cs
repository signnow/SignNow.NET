using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public class Owner
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Email of document owner.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Organization info.
        /// </summary>
        [JsonProperty("organization", NullValueHandling = NullValueHandling.Ignore)]
        public Organization Organization { get; set; }
    }
}
