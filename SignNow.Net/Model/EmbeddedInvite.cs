using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represent Embedded signing invite params.
    /// </summary>
    public class EmbeddedInvite
    {
        /// <summary>
        /// Signer's email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Signer's role ID.
        /// </summary>
        [JsonProperty("role_id")]
        public  string RoleId { get; set; }

        /// <summary>
        /// Order of signing. Cannot be 0.
        /// </summary>
        [JsonProperty("order")]
        public int SigningOrder { get; set; }

        /// <summary>
        /// Signer authentication method.
        /// </summary>
        [JsonProperty("auth_method")]
        public string AuthMethod { get; set; } = "none";
    }
}
