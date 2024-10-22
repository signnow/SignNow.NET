using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.Responses
{
    public class DocumentGroupsResponse
    {
        [JsonProperty("document_groups")]
        public IReadOnlyList<DocumentGroups> Data { get; set; }

        /// <summary>
        /// Total documents count in document group.
        /// </summary>
        [JsonProperty("document_group_total_count")]
        public int TotalCount { get; set; }
    }

    public class DocumentGroups
    {
        /// <summary>
        /// An ID of folder with document group.
        /// </summary>
        [JsonProperty("folder_id")]
        public string FolderId { get; set; }

        /// <summary>
        /// Timestamp of document group update.
        /// </summary>
        [JsonProperty("last_updated")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Document Group name.
        /// </summary>
        [JsonProperty("group_name")]
        public string Name { get; set; }

        /// <summary>
        /// An ID of document group.
        /// </summary>
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        /// <summary>
        /// Identity of invite for Document Group.
        /// </summary>
        [JsonProperty("invite_id")]
        public string InviteId { get; set; }

        /// <summary>
        /// Invite status
        /// </summary>
        [JsonProperty("invite_status")]
        public string InviteStatus { get; set; }

        [JsonProperty("documents")]
        public IReadOnlyList<GroupDocumentsInfo> Documents { get; set; }

        /// <summary>
        /// Is the document full declined.
        /// </summary>
        [JsonProperty("is_full_declined")]
        public bool IsFullDeclined { get; set; }

        /// <summary>
        /// Is the document embedded.
        /// </summary>
        [JsonProperty("is_embedded")]
        public bool IsEmbedded { get; set; }

        /// <summary>
        /// Freeform invite info.
        /// </summary>
        [JsonProperty("freeform_invite")]
        public FreeFormInviteInfo FreeFormInvite { get; set; }

        /// <summary>
        /// Document Group state.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Is the document has guest signer.
        /// </summary>
        [JsonProperty("has_guest_signer")]
        public bool HasGuestSigner { get; set; }

        /// <summary>
        /// Is the document has signing group.
        /// </summary>
        [JsonProperty("has_signing_group")]
        public bool HasSigningGroup { get; set; }
    }
}
