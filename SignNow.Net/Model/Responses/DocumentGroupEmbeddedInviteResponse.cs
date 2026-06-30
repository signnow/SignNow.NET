using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Model.Responses.GenericResponses;

namespace SignNow.Net.Model.Responses
{
    public class DocumentGroupEmbeddedInviteData : IdResponse
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InviteStatus Status { get; set; }
    }

    public class DocumentGroupEmbeddedInviteResponse
    {
        [JsonProperty("data")]
        public IReadOnlyList<DocumentGroupEmbeddedInviteData> InviteData { get; internal set; }
    }
}
