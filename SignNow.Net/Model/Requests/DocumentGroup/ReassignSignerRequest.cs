using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    public class ReassignSignerInfo
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role_name")]
        public string RoleName { get; set; }
    }

    public class ReassignSignerRequest : JsonHttpContent
    {
        [JsonProperty("new_signer")]
        public ReassignSignerInfo NewSigner { get; set; }
    }
}
