using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public class Company
    {
        /// <summary>
        /// Company name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// User have full access to Company when it belongs to Company domain.
        /// </summary>
        [JsonProperty("full_access")]
        public bool FullAccess { get; set; }
    }
}
