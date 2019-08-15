using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public class Token
    {
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }
        [JsonProperty(PropertyName = "last_login")]
        public int LastLogin { get; set; }
    }
}

