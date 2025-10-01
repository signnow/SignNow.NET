using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    public class SendVerificationEmailRequest : JsonHttpContent
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
