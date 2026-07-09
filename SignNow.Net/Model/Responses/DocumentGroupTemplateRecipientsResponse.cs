using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// A recipient role assigned to a document group template.
    /// </summary>
    public class DocumentGroupTemplateRecipientRole
    {
        /// <summary>
        /// Unique identifier of the role.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the role, e.g. "Signer 1".
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Position of this role in the signing order.
        /// </summary>
        [JsonProperty("signing_order")]
        public int SigningOrder { get; set; }

        /// <summary>
        /// Email address currently assigned to this role. Optional.
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>
        /// First name of the recipient assigned to this role. Optional.
        /// </summary>
        [JsonProperty("first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the recipient assigned to this role. Optional.
        /// </summary>
        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
    }

    /// <summary>
    /// A single signing step of a document group template, listing the roles assigned to it.
    /// </summary>
    public class DocumentGroupTemplateRecipientStep
    {
        /// <summary>
        /// Position of this step in the overall signing order.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Roles assigned to this step.
        /// </summary>
        [JsonProperty("recipients")]
        public IReadOnlyList<DocumentGroupTemplateRecipientRole> Recipients { get; set; }
    }

    /// <summary>
    /// Recipient data for a document group template.
    /// </summary>
    public class DocumentGroupTemplateRecipientsData
    {
        /// <summary>
        /// Ordered signing steps configured for this template.
        /// </summary>
        [JsonProperty("invite_steps")]
        public IReadOnlyList<DocumentGroupTemplateRecipientStep> InviteSteps { get; set; }
    }

    /// <summary>
    /// Response returned when getting the recipients of a document group template.
    /// </summary>
    public class DocumentGroupTemplateRecipientsResponse
    {
        /// <summary>
        /// Recipient data for the document group template.
        /// </summary>
        [JsonProperty("data")]
        public DocumentGroupTemplateRecipientsData Data { get; set; }
    }
}
