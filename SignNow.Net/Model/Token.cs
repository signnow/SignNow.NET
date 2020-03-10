using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// The access token you can use to make requests on behalf of this SignNow account.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Time in seconds for which the <see cref="Token"/> was issued.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Access <see cref="Token"/>
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh <see cref="Token"/>
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Access <see cref="SignNow.Net.Model.Scope"/>
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Type of access token (e.g. 'bearer')
        /// </summary>
        [JsonProperty("token_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TokenType TokenType { get; set; }

        [JsonProperty("last_login")]
        public int LastLogin { get; set; }

        /// <summary>
        /// Returns formatted Authorization header param
        /// </summary>
        /// <returns>String value in format: 'bearer access_token'</returns>
        public string GetAuthorizationHeaderValue()
        {
            return $"{TokenType} {AccessToken}";
        }
    }
}
