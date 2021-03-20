using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents common (freeform or role-based) details of invite.
    /// </summary>
    public abstract class SignNowInvite : ISignNowInviteStatus
    {
        /// <inheritdoc cref="ISignNowInviteStatus"/>
        public virtual string Id { get; internal set; }

        /// <inheritdoc cref="ISignNowInviteStatus"/>
        public virtual string SignerEmail { get; internal set; }

        /// <inheritdoc cref="ISignNowInviteStatus"/>
        public virtual InviteStatus Status { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Created { get; internal set; }

        internal SignNowInvite() { }
    }

    /// <summary>
    /// Represents details of the field invite for the Document.
    /// </summary>
    public sealed class FieldInvite : SignNowInvite
    {
        /// <summary>
        /// Unique identifier of field invite.
        /// </summary>
        [JsonProperty("id")]
        public override string Id { get; internal set; }

        /// <inheritdoc cref="SignNowInvite"/>
        [JsonProperty("email")]
        public override string SignerEmail { get; internal set; }

        /// <summary>
        /// Status of the field invite sign request.
        /// </summary>
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public override InviteStatus Status { get; internal set; }

        /// <summary>
        /// Signer role name.
        /// </summary>
        [JsonProperty("role")]
        public string RoleName { get; internal set; }

        /// <summary>
        /// Signer role identity.
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId { get; internal set; }

        /// <summary>
        /// Timestamp document was updated.
        /// </summary>
        [JsonProperty("updated")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Updated { get; internal set; }

        /// <summary>
        /// Date and time of invite expiration.
        /// </summary>
        [JsonProperty("expiration_time")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime ExpiredOn { get; internal set; }

        /// <summary>
        /// Is embedded signing invite or not.
        /// </summary>
        [JsonProperty("is_embedded")]
        public bool IsEmbedded { get; internal set; }
    }

    /// <summary>
    /// Represents details of freeform invite for the document.
    /// </summary>
    public sealed class FreeformInvite : SignNowInvite
    {
        /// <summary>
        /// Sign invite unique id.
        /// </summary>
        [JsonProperty("unique_id")]
        public override string Id { get; internal set; }

        /// <summary>
        /// Email of user who invited to sign the document.
        /// </summary>
        [JsonProperty("signer_email")]
        public override string SignerEmail { get; internal set; }

        /// <summary>
        /// <see cref="FreeformInvite"/> sign status of current signer.
        /// </summary>
        [JsonIgnore]
        public override InviteStatus Status => SignatureId == null ? InviteStatus.Pending : InviteStatus.Fulfilled;

        /// <summary>
        /// Identity of user who invited to sign the document.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; internal set; }

        /// <summary>
        /// Identity of the signers' signature
        /// </summary>
        [JsonProperty("signature_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SignatureId { get; internal set; }

        /// <summary>
        /// Email of document owner.
        /// </summary>
        [JsonProperty("originator_email")]
        public string Owner { get; internal set; }

        /// <summary>
        /// Is freeform sign invite canceled or not.
        /// </summary>
        [JsonProperty("canceled", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringToBoolJsonConverter))]
        internal bool? IsCanceled { get; set; }
    }
}
