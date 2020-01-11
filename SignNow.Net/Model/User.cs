using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represent a user resource
    /// </summary>
    public class User
    {
        /// <summary>
        /// <see cref="User"/> is active or not.
        /// </summary>
        [JsonProperty("active")]
        [JsonConverter(typeof(StringToBoolJsonConverter))]
        public bool Active { get; set; }

        /// <summary>
        /// <see cref="User"/> first name.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// <see cref="User"/> last name.
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// <see cref="User"/> email.
        /// </summary>
        [JsonProperty("primary_email")]
        public string Email { get; set; }
    }
}
