using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests
{
    public class UpdateUserOptions : JsonHttpContent
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

        /// <summary>
        /// User password.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Old User password.
        /// </summary>
        [JsonProperty("old_password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// if "true" - all user tokens with Scope <see cref="Scope.All"/> expire
        /// if "false" - all user tokens except current one are expired
        /// </summary>
        [JsonProperty("logout_all")]
        public bool LogOutAll { get; set; } = true;
    }
}
