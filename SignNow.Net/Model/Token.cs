using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public class Token
    {
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("token_type")]
        public TokenType TokenType { get; set; }

        [JsonProperty("last_login")]
        public int LastLogin { get; set; }

        public string GetAuthorizationHeaderValue()
        {
            return $"{TokenType} {AccessToken}";
        }
    }
}