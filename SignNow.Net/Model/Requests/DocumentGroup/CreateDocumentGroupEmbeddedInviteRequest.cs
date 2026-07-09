using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// A signer to add to an embedded signing invite for a document group.
    /// </summary>
    public class DocumentGroupEmbeddedInviteSigner
    {
        /// <summary>
        /// Email address of the signer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Unique identifier of the role this signer fulfills.
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        /// <summary>
        /// Position of this signer in the signing order.
        /// </summary>
        [JsonProperty("order")]
        public uint SigningOrder { get; set; }

        /// <summary>
        /// Authentication method the signer must pass before signing.
        /// </summary>
        [JsonProperty("auth_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbeddedAuthType AuthMethod { get; set; } = EmbeddedAuthType.None;
    }

    /// <summary>
    /// Request to create an embedded signing invite for a document group without sending emails.
    /// </summary>
    public class CreateDocumentGroupEmbeddedInviteRequest : JsonHttpContent
    {
        /// <summary>
        /// Signers to include in the embedded signing invite.
        /// </summary>
        [JsonProperty("invites")]
        public IList<DocumentGroupEmbeddedInviteSigner> Invites { get; set; } = new List<DocumentGroupEmbeddedInviteSigner>();
    }
}
