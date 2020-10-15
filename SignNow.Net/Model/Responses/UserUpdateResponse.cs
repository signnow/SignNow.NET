using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from SignNow API for User update request.
    /// </summary>
    public class UserUpdateResponse
    {
        /// <summary>
        /// User identity.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

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
