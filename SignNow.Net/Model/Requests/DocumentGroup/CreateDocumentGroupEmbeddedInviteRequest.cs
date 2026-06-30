using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    public class DocumentGroupEmbeddedInviteSigner
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        [JsonProperty("order")]
        public uint SigningOrder { get; set; }

        [JsonProperty("auth_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbeddedAuthType AuthMethod { get; set; } = EmbeddedAuthType.None;
    }

    public class CreateDocumentGroupEmbeddedInviteRequest : JsonHttpContent
    {
        [JsonProperty("invites")]
        public IList<DocumentGroupEmbeddedInviteSigner> Invites { get; set; } = new List<DocumentGroupEmbeddedInviteSigner>();
    }
}
