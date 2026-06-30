using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for operations with Document Group Invites in signNow.
    /// Allows sending, retrieving, canceling, resending, and managing signers
    /// for group signing workflows.
    /// </summary>
    public interface IDocumentGroupInvite
    {
        /// <summary>
        /// Sends an invite to sign a document group.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="request">Invite steps with signers and actions.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        Task<GroupInviteResponse> CreateGroupInviteAsync(string documentGroupId, CreateGroupInviteRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the status and details of a document group invite.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="inviteId">ID of the group invite.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        Task<GroupInviteResponse> GetGroupInviteAsync(string documentGroupId, string inviteId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels a document group invite.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="inviteId">ID of the group invite.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        Task CancelGroupInviteAsync(string documentGroupId, string inviteId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Resends invite emails for a document group.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="inviteId">ID of the group invite.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        Task ResendGroupInviteAsync(string documentGroupId, string inviteId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the list of pending invites for a document group.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="inviteId">ID of the group invite.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        Task<PendingGroupInvitesResponse> GetPendingGroupInvitesAsync(string documentGroupId, string inviteId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reassigns a signer in a specific invite step of a document group workflow.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="inviteId">ID of the group invite.</param>
        /// <param name="stepId">ID of the invite step to update.</param>
        /// <param name="request">New signer details.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        Task ReassignSignerAsync(string documentGroupId, string inviteId, string stepId, ReassignSignerRequest request, CancellationToken cancellationToken = default);
    }
}
