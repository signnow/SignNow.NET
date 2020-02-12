using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Internal.Model;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents SignNow document object.
    /// <remarks>
    /// Document is the fundamental unit of every e-Signature operation. It contains:
    ///     Metadata: file name, size, extension, ID;
    ///     Fields, elements (texts, checks and signatures),
    ///     Invites, status of the invites,
    ///     <see cref="Role" />,
    ///     Timestamps (date created, date updated)
    /// </remarks>
    /// </summary>
    public class SignNowDocument
    {
        /// <summary>
        /// Identity of specific document.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// An id of original document (if document is a copy).
        /// </summary>
        [JsonProperty("origin_document_id")]
        public string OriginDocumentId { get; set; }

        /// <summary>
        /// Identity of user that uploaded document.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Identity of user who created document.
        /// </summary>
        [JsonProperty("origin_user_id")]
        public string OriginUserId { get; set; }

        /// <summary>
        /// Name of document.
        /// </summary>
        [JsonProperty("document_name")]
        public string Name { get; set; }

        /// <summary>
        /// Original filename with document format (.pdf, .doc, etc...).
        /// </summary>
        [JsonProperty("original_filename")]
        public string OriginalName { get; set; }

        /// <summary>
        /// Amount of pages in the document.
        /// </summary>
        [JsonProperty("page_count")]
        [JsonConverter(typeof(StringToIntJsonConverter))]
        public int PageCount { get; set; }

        /// <summary>
        /// Timestamp document was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// Timestamp document was updated.
        /// </summary>
        [JsonProperty("updated")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Email of document owner.
        /// </summary>
        [JsonProperty("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// Is document a template or not.
        /// </summary>
        [JsonProperty("template")]
        public bool IsTemplate { get; set; }

        /// <summary>
        /// The document signer roles.
        /// </summary>
        [JsonProperty("roles")]
        internal List<Role> Roles { get; private set; } = new List<Role>();

        /// <summary>
        /// The document <see cref="Signature"/>
        /// </summary>
        [JsonProperty("signatures")]
        internal List<Signature> Signatures { get; private set; } = new List<Signature>();

        /// <summary>
        /// The document <see cref="Field"/>
        /// </summary>
        [JsonProperty("fields")]
        internal List<Field> Fields { get; private set; } = new List<Field>();

        /// <summary>
        /// The document freeform invite requests.
        /// </summary>
        [JsonProperty("requests")]
        internal List<FreeformInvite> InviteRequests { get; private set; } = new List<FreeformInvite>();

        /// <summary>
        /// The document field invite requests.
        /// </summary>
        [JsonProperty("field_invites")]
        public IReadOnlyCollection<FieldInvite> FieldInvites { get; private set; } = new List<FieldInvite>();

        /// <summary>
        /// Provides common details of any kind of invites (freeform or role-based)
        /// </summary>
        [JsonIgnore]
        public IReadOnlyCollection<ISignNowInviteStatus> InvitesStatus
        {
            get
            {
                if (InviteRequests.Count > 0) return InviteRequests;
                if (FieldInvites.Count > 0) return FieldInvites;

                return _emptyInvites;
            }
        }

        /// <summary>
        /// The document sign status.
        /// </summary>
        [JsonIgnore]
        public DocumentStatus Status
        {
            get
            {
                if (InvitesStatus.Any(i => i.Status == InviteStatus.Pending))
                {
                    return DocumentStatus.Pending;
                }

                if (InvitesStatus.Count > 0 && InvitesStatus.All(i => i.Status == InviteStatus.Fulfilled))
                {
                    return DocumentStatus.Completed;
                }

                return DocumentStatus.NoInvite;
            }
        }

        private static readonly IReadOnlyCollection<ISignNowInviteStatus> _emptyInvites = new SignNowInvite[0];
    }
}
