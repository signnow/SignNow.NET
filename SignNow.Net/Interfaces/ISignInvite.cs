using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for any operations with an Invite in signNow:
    /// creating or canceling the invite to sign a document, checking status of the invite, etc.
    /// </summary>
    public interface ISignInvite
    {
        /// <summary>
        /// Create an invite to sign a document.
        /// </summary>
        /// <param name="documentId">Identity of the document to create signing invite for.</param>
        /// <param name="invite">A simple free form invite or a role-based invite.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<InviteResponse> CreateInviteAsync(string documentId, SignInvite invite, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates embedded signing invites for a document without sending emails.
        /// </summary>
        /// <param name="documentId">Identity of the document to create signing invite for.</param>
        /// <param name="invite">An embedded signing invites options for each document role.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<EmbeddedInviteResponse> CreateInviteAsync(string documentId, EmbeddedSigningInvite invite, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a link for the embedded invite.
        /// </summary>
        /// <param name="documentId">Identity of the document to create signing link invite for.</param>
        /// <param name="options">Embedded invite link create options.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<EmbeddedInviteLinkResponse> GenerateEmbeddedInviteLinkAsync(string documentId, CreateEmbedLinkOptions options, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels a freeform invite sign request.
        /// </summary>
        /// <param name="invite"><see cref="FreeformInvite"/> to cancel signing invite for.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task CancelInviteAsync(FreeformInvite invite, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels a field invite (role-based invite) to a document.
        /// </summary>
        /// <param name="documentId">The Document identity to cancel an fields invitation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task CancelInviteAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels an embedded sign invite to a document.
        /// </summary>
        /// <param name="documentId">The Document identity to cancel an embedded invitation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task CancelEmbeddedInviteAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Resends an invite to sign a document.
        /// </summary>
        /// <param name="fieldInviteId">The role identity to resend the invitation to.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task ResendEmailInviteAsync(string fieldInviteId, CancellationToken cancellationToken = default);
    }
}
