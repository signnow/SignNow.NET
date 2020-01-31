using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
        internal List<Role> Roles { get; private set; }

        /// <summary>
        /// The document <see cref="Signature"/>
        /// </summary>
        [JsonProperty("signatures")]
        internal List<Signature> Signatures { get; private set; }

        /// <summary>
        /// The document freeform invite requests.
        /// </summary>
        [JsonProperty("requests")]
        internal List<FreeformInvite> InviteRequests { get; private set; }

        /// <summary>
        /// The document field invite requests.
        /// </summary>
        [JsonProperty("field_invites")]
        public IReadOnlyCollection<FieldInvite> FieldInvites { get; private set; }


        /// <summary>
        /// The document sign status.
        /// </summary>
        [JsonIgnore]
        public SignStatus Status
        {
            get
            {
                if (IsFreeformInviteSigned() || IsFieldInviteSigned() ) return SignStatus.Completed;

                if (HasFreeformInviteRequests()) return SignStatus.Pending;

                return SignStatus.None;
            }
        }

        private bool HasFreeformInviteRequests()
        {
            if (null == InviteRequests && null == Signatures)
            {
                return false;
            }

            return InviteRequests.Count > 0 && Signatures.Count < InviteRequests.Count;
        }

        /// <summary>
        /// Check if <see cref="FreeformInvite"/> was signed.
        /// </summary>
        /// <returns>True if document was signed via freeform sign request.</returns>
        private bool IsFreeformInviteSigned()
        {
            if (null == InviteRequests || InviteRequests.Count != Signatures.Count || Signatures.Count == 0)
            {
                return false;
            }

            var signed = (from invite in InviteRequests
                join signature in Signatures on invite.Id equals signature.SignatureRequestId
                select invite).Count();

            return signed == InviteRequests.Count
                   && signed == Signatures.Count;
        }

        /// <summary>
        /// Check if <see cref="FieldInvite" /> was signed.
        /// </summary>
        /// <returns>True if document was signed via field (role-based) sign request.</returns>/
        private bool IsFieldInviteSigned()
        {
            // @TODO: Implement sign check for role-based invite
            return false;
        }
    }
}
