using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represent a user resource
    /// </summary>
    public class User
    {
        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}