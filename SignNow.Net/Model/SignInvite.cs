using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Helpers;

namespace SignNow.Net.Model
{
    public abstract class SignInvite
    {
        /// <summary>
        /// The subject of the email.
        /// <remarks>
        ///     If <see cref="Subject"/> is null - default subject will be used:
        ///     `sender.email@signnow.com` Needs Your Signature
        /// </remarks>
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// The message body of the email invite.
        /// <remarks>
        ///     If <see cref="Message"/> is null - default message will be used:
        ///     `sender.email@signnow.com` invited you to sign `DocumentName`
        /// </remarks>
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        internal SignInvite()
        {
        }
    }

    /// <summary>
    /// Freeform invite - an invitation to sign a document which doesn't contain any fillable fields.
    /// </summary>
    public sealed class FreeFormInvite : SignInvite
    {
        /// <summary>
        /// An email of signer`s that you would like to send the invite to.
        /// </summary>
        [JsonProperty("to")]
        public string Recipient { get; private set; }

        /// <summary>
        /// Constructs a new <see cref="FreeFormInvite"/>
        /// </summary>
        /// <param name="to">The email of the invitee.</param>
        public FreeFormInvite(string to)
        {
            Recipient = to;
        }
    }

    /// <summary>
    /// Role-based invite - an invitation to sign a document
    /// which contains at least one fillable field assigned to one role.
    /// </summary>
    public sealed class RoleBasedInvite : SignInvite
    {
        /// <summary>
        /// Represent recipients for which an invitation to sign should be sent.
        /// </summary>
        [JsonProperty("to")]
        private List<SignerOptions> RecipientList { get; set; } = new List<SignerOptions>();

        private List<Role> ExistingDocumentRoles { get; set; }

        /// <summary>
        /// Construct Role-based invite.
        /// </summary>
        /// <param name="document">SignNow document for which an invitation to sign should be sent.</param>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="SignNowDocument"/> cannot be null.
        /// </exception>
        public RoleBasedInvite(SignNowDocument document)
        {
            Guard.ArgumentNotNull(document, nameof(document));
            ExistingDocumentRoles = document.Roles;
        }

        /// <summary>
        /// Return Roles read only collection for current document.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Role> DocumentRoles()
        {
            return ExistingDocumentRoles;
        }

        /// <summary>
        /// Add user role with options to Role-based invite.
        /// </summary>
        /// <param name="options">Role <see cref="SignerOptions"/></param>
        /// <exception cref="ArgumentNullException"><see cref="Role"/> cannot be null.</exception>
        /// <exception cref="SignNowException">Allowed only <see cref="Role"/> which is exists in current Document</exception>
        public void AddRoleBasedInvite(SignerOptions options)
        {
            Guard.ArgumentNotNull(options, nameof(options));

            var originalRole = ExistingDocumentRoles.Find(p => p.Name == options.RoleName);

            if (null == originalRole)
            {
                throw new SignNowException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.CannotAddRole, options.RoleName));
            }

            var uniqueContent = RecipientList.TrueForAll(item => item.RoleName != options.RoleName);

            if (uniqueContent)
            {
                RecipientList.Add(options);
            }
        }
    }
}
