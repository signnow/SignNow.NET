using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests
{
    public class CreateUserOptions : JsonHttpContent
    {
        /// <summary>
        /// User email.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// User firstname.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// User lastname.
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}
