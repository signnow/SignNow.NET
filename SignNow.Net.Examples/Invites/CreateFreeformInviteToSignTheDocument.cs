using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Invites
{
    public static partial class InviteExamples
    {
        /// <summary>
        /// Create a freeform invite to the document for signature.
        /// </summary>
        /// <param name="document">signNow document you’d like to have signed</param>
        /// <param name="email">The email of the invitee.</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns>
        /// <see cref="InviteResponse"/> which contains an Identity of invite request.
        /// </returns>
        public static async Task<InviteResponse> CreateFreeformInviteToSignTheDocument(SignNowDocument document, string email, SignNowContext signNowContext)
        {
            // Create freeform invite
            var invite = new FreeFormSignInvite(email)
            {
                Message = $"{email} invited you to sign the document {document.Name}",
                Subject = "The subject of the Email"
            };

            // Creating Invite request
            return await signNowContext.Invites
                .CreateInviteAsync(document.Id, invite)
                .ConfigureAwait(false);
        }
    }
}
