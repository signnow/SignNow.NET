using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Specifies the action to be taken upon invite completion.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmailActionsType
    {
        /// <summary>
        /// Send documents and attachments to all recipients
        /// </summary>
        [EnumMember(Value = "documents_and_attachments")]
        DocumentsAndAttachments,

        /// <summary>
        /// Send documents and attachments only to recipients
        /// </summary>
        [EnumMember(Value = "documents_and_attachments_only_to_recipients")]
        DocumentsAndAttachmentsOnlyToRecipients,

        /// <summary>
        /// Do not send documents and attachments
        /// </summary>
        [EnumMember(Value = "without_documents_and_attachments")]
        WithoutDocumentsAndAttachments
    }
}
