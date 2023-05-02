using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;

namespace SignNow.Net.Internal.Requests
{
    internal class GetAccessTokenRequest : IContent
    {
        /// <summary>
        /// User's email.
        /// </summary>
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        /// <summary>
        /// Can be 'password', 'refresh_token', 'authorization_code'.
        /// </summary>
        [JsonProperty("grant_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public GrantType GrantType { get; set; } = GrantType.Password;

        /// <summary>
        /// Token Scope.
        /// </summary>
        [JsonProperty("scope")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Scope TokenScope { get; set; } = Scope.All;

        /// <summary>
        /// Required if 'grant_type: refresh_token'
        /// </summary>
        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Required if 'grant_type: authorization_code'
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// The amount of time till the token expires in seconds.
        /// </summary>
        [JsonProperty("expiration_time", NullValueHandling = NullValueHandling.Ignore)]
        public int ExpirationTime { get; set; } = 60;

        public HttpContent GetHttpContent()
        {
            var json = JsonConvert.SerializeObject(this);
            return new FormUrlEncodedContent(
                JsonConvert.DeserializeObject<Dictionary<string, string>>(json));
        }
    }
}
