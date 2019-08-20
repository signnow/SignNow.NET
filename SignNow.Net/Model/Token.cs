using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
