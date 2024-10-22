using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model.Responses.GenericResponses;

namespace SignNow.Net.Model.Responses
{
    public class DocumentGroupInfoResponse
    {
        [JsonProperty("data")]
        public DocumentGroupData Data { get; set; }
    }

    public class DocumentGroupData : IdResponse
    {
        /// <summary>
        /// Document Group name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Timestamp of document group creation.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// Timestamp of document group update.
        /// </summary>
        [JsonProperty("updated")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Identity of invite for Document Group.
        /// </summary>
        [JsonProperty("invite_id")]
        public string InviteId { get; set; }

        /// <summary>
        /// Identity of pending step for Document Group.
        /// </summary>
        [JsonProperty("pending_step_id")]
        public string PendingStepId { get; set; }

        /// <summary>
        /// Identity of last invite for Document Group.
        /// </summary>
        [JsonProperty("last_invite_id")]
        public string LastInviteId { get; set; }

        /// <summary>
        /// Document Group state.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Owner email address.
        /// </summary>
        [JsonProperty("owner_email")]
        public string OwnerEmail { get; set; }

        /// <summary>
        /// An ID of folder with document group.
        /// </summary>
        [JsonProperty("folder_id")]
        public string FolderId { get; set; }

        [JsonProperty("documents")]
        public IReadOnlyList<GroupDocumentsInfo> Documents { get; set; }

        /// <summary>
        /// Document owner info.
        /// </summary>
        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        /// <summary>
        /// CC emails list.
        /// </summary>
        [JsonProperty("cc_emails")]
        public IReadOnlyList<string> CcEmails { get; set; }

        /// <summary>
        /// Freeform invite info.
        /// </summary>
        [JsonProperty("freeform_invite")]
        public FreeFormInviteInfo FreeFormInvite { get; set; }
    }

    public class GroupDocumentsInfo : IdResponse
    {
        /// <summary>
        /// List with document roles.
        /// </summary>
        [JsonProperty("roles")]
        public IReadOnlyList<string> Roles { get; set; }

        /// <summary>
        /// Document name.
        /// </summary>
        [JsonProperty("document_name")]
        public string Name { get; set; }

        /// <summary>
        /// Pages count.
        /// </summary>
        [JsonProperty("page_count")]
        public int Pages { get; set; }

        /// <summary>
        /// Timestamp of document update.
        /// </summary>
        [JsonProperty("updated")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Updated { get; set; }

        /// <summary>
        /// An ID of folder with document.
        /// </summary>
        [JsonProperty("folder_id")]
        public string FolderId { get; set; }

        /// <summary>
        /// Document owner info.
        /// </summary>
        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        /// <summary>
        /// Document thumbnails
        /// </summary>
        [JsonProperty("thumbnail")]
        public Thumbnail Thumbnail { get; set; }

        /// <summary>
        /// An ID of document origin.
        /// </summary>
        [JsonProperty("origin_document_id")]
        public string OriginDocumentId { get; set; }

        /// <summary>
        /// Is the document has unassigned field.
        /// </summary>
        [JsonProperty("has_unassigned_field")]
        public bool HasUnassignedField { get; set; }

        /// <summary>
        /// Is the document has a credit card number.
        /// </summary>
        [JsonProperty("has_credit_card_number")]
        public bool HasCreditCardNumber { get; set; }

        /// <summary>
        /// Is the document can be removed from document group.
        /// </summary>
        [JsonProperty("allow_to_remove")]
        public bool IsAllowedToRemove { get; set; }
    }

    public class FreeFormInviteInfo : IdResponse
    {
        [JsonProperty("last_id")]
        public string LastId { get; set; }
    }
}
