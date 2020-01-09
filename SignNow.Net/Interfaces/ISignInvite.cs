using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Send a freeform or role-based invite
    /// </summary>
    public interface ISignInvite
    {
        /// <summary>
        /// Create an invite to sign a document.
        /// </summary>
        /// <param name="documentId">Identity of the document to create signing invite for</param>
        /// <param name="invite">A simple free form invite or a role-based invite</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        Task<InviteResponse> CreateInviteAsync(string documentId, SignInvite invite, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels an freeform invite to a document.
        /// </summary>
        /// <param name="inviteId">Identity of the invite to cancel signing invite for</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        Task CancelInviteAsync(string inviteId, CancellationToken cancellationToken = default);
    }
}
