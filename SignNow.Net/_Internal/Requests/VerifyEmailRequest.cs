using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// Request for email verification using verification token
    /// </summary>
    internal class VerifyEmailRequest : JsonHttpContent
    {
        /// <summary>
        /// User's email address
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The token included in the verification link sent to the user's email address
        /// </summary>
        [JsonProperty("verification_token")]
        public string VerificationToken { get; set; }
    }
}
