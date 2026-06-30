using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    public class PendingGroupInvite
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class PendingGroupInvitesResponse
    {
        [JsonProperty("data")]
        public IReadOnlyList<PendingGroupInvite> Data { get; set; }
    }
}
