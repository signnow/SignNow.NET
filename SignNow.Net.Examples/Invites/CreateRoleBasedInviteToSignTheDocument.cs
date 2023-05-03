using System.Linq;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Invites
{
    public static partial class InviteExamples
    {
        /// <summary>
        /// Create a role-based invite to the document for signature.
        /// </summary>
        /// <param name="document">signNow document with fields you’d like to have signed</param>
        /// <param name="email">The email of the invitee.</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns><see cref="InviteResponse"/> without any Identity of invite request.</returns>
        public static async Task<InviteResponse> CreateRoleBasedInviteToSignTheDocument(SignNowDocument document, string email, SignNowContext signNowContext)
        {
            // Create role-based invite
            var invite = new RoleBasedInvite(document)
            {
                Message = $"{email} invited you to sign the document {document.Name}",
                Subject = "The subject of the Email"
            };

            // Creates options for signers
            var signer = new SignerOptions(email, invite.DocumentRoles().First())
                {
                    ExpirationDays = 15,
                    RemindAfterDays = 7,
                }
                .SetAuthenticationByPassword("***PASSWORD_TO_OPEN_THE_DOCUMENT***");

            // Attach signer to existing roles in the document
            invite.AddRoleBasedInvite(signer);

            // Creating Invite request
            return await signNowContext.Invites
                .CreateInviteAsync(document.Id, invite)
                .ConfigureAwait(false);
        }
    }
}
