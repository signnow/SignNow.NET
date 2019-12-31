using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represent a user resource
    /// </summary>
    public class User
    {
        [JsonProperty("active"), JsonConverter(typeof(StringToBoolJsonConverter))]
        public bool Active { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("primary_email")]
        public string Email { get; set; }
    }
}
