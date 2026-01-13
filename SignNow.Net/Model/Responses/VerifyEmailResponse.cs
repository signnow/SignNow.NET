using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response from email verification request
    /// </summary>
    public class VerifyEmailResponse
    {
        /// <summary>
        /// Verified email address
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
