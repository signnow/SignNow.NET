using Newtonsoft.Json;
using SignNow.Net.Model.Responses.GenericResponses;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from signNow API for User update request.
    /// </summary>
    public class UserUpdateResponse : IdResponse
    {
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
