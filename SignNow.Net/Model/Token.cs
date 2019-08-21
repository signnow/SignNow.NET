using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public class Token
    {
        public string GetAccessToken()
        {
            return "token";
        }
        
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}

