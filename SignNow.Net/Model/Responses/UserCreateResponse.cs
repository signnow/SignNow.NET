using Newtonsoft.Json;
using SignNow.Net.Model.Responses.GenericResponses;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from signNow API for User create request.
    /// </summary>
    public class UserCreateResponse : IdResponse
    {
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
