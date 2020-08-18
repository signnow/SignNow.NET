using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Extensions;
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

        /// <summary>
        /// The list with emails of copy receivers.
        /// </summary>
        [JsonProperty("cc", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> Cc => CcList;

        [JsonIgnore]
        protected HashSet<string> CcList { get; } = new HashSet<string>();

        internal SignInvite() { }

        /// <summary>
        /// Add an Email to CC list.
        /// </summary>
        /// <param name="email">Email of copy receiver.</param>
        /// <exception cref="ArgumentException">when an email is not valid.</exception>
        public void AddCcRecipients(string email)
        {
            CcList.Add(email.ValidateEmail());
        }

        /// <inheritdoc cref="AddCcRecipients(string)"/>
        /// <param name="emails">Emails list of copy receivers.</param>
        public void AddCcRecipients(IEnumerable<string> emails)
        {
            Guard.ArgumentNotNull(emails, nameof(emails));
            foreach (var email in emails)
            {
                AddCcRecipients(email);
            }
        }
    }

    /// <summary>
    /// Freeform invite - an invitation to sign a document which doesn't contain any fillable fields.
    /// </summary>
    public sealed class FreeFormSignInvite : SignInvite
    {
        /// <summary>
        /// An email of signer`s that you would like to send the invite to.
        /// </summary>
        [JsonProperty("to")]
        public string Recipient { get; private set; }

        /// <summary>
        /// Constructs a new <see cref="FreeFormSignInvite"/>
        /// </summary>
        /// <param name="to">The email of the invitee.</param>
        public FreeFormSignInvite(string to)
        {
            Recipient = to;
        }

        /// <inheritdoc cref="FreeFormSignInvite(string)"/>
        /// <param name="to">The email of the invitee.</param>
        /// <param name="cc">The email of copy receiver.</param>
        /// <exception cref="ArgumentException">for not valid <paramref name="cc"/> email address.</exception>
        public FreeFormSignInvite(string to, string cc) : this(to)
        {
            AddCcRecipients(cc);
        }

        /// <inheritdoc cref="FreeFormSignInvite(string)"/>
        /// <param name="to">The email of the invitee.</param>
        /// <param name="cc">The emails list of copy receivers.</param>
        /// <exception cref="ArgumentException">for not valid <paramref name="cc"/> email address.</exception>
        public FreeFormSignInvite(string to, IEnumerable<string> cc) : this(to)
        {
            AddCcRecipients(cc);
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
        /// <exception cref="ArgumentNullException"><see cref="SignNowDocument"/> cannot be null.</exception>
        /// <exception cref="ArgumentException">Document <see cref="SignNowDocument.Roles"/> cannot be empty.</exception>
        public RoleBasedInvite(SignNowDocument document)
        {
            Guard.ArgumentNotNull(document, nameof(document));

            if (document.Roles.Count == 0)
            {
                throw new ArgumentException(ExceptionMessages.NoFillableFieldsWithRole);
            }

            ExistingDocumentRoles = document.Roles;
        }

        /// <inheritdoc cref="RoleBasedInvite(SignNowDocument)"/>
        /// <param name="document">SignNow document for which an invitation to sign should be sent.</param>
        /// <param name="cc">The email of copy receiver.</param>
        /// <exception cref="ArgumentException">for not valid <paramref name="cc"/> email address.</exception>
        public RoleBasedInvite(SignNowDocument document, string cc): this(document)
        {
            AddCcRecipients(cc);
        }

        /// <inheritdoc cref="RoleBasedInvite(SignNowDocument)"/>
        /// <param name="document">SignNow document for which an invitation to sign should be sent.</param>
        /// <param name="cc">The emails list of copy receivers.</param>
        /// <exception cref="ArgumentException">for not valid <paramref name="cc"/> email address.</exception>
        public RoleBasedInvite(SignNowDocument document, IEnumerable<string> cc): this(document)
        {
            AddCcRecipients(cc);
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

            var originalRole = ExistingDocumentRoles.Find(p => p.Name == options.SignerRole.Name);

            if (null == originalRole)
            {
                throw new SignNowException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.CannotAddRole, options.SignerRole.Name));
            }

            var uniqueContent = RecipientList.TrueForAll(item => item.SignerRole.Name != options.SignerRole.Name);

            if (uniqueContent)
            {
                RecipientList.Add(options);
            }
        }
    }
}
