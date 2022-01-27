using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from signNow API for User create request.
    /// </summary>
    public class UserCreateResponse
    {
        /// <summary>
        /// User identity.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// User is verified or not.
        /// </summary>
        [JsonProperty("verified")]
        public bool Verified { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
