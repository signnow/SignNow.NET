using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Invites
{
    public static partial class InviteExamples
    {
        /// <summary>
        /// Create an embedded signing invite to the document for signature.
        /// </summary>
        /// <param name="document">SignNow document you’d like to have signed</param>
        /// <param name="email">The email of the invitee.</param>
        /// <param name="token">Access token</param>
        /// <returns>
        /// <see cref="EmbeddedInviteResponse"/> which contains an invite data.
        /// </returns>
        public static async Task<EmbeddedInviteResponse>
            CreateEmbeddedSigningInviteToSignTheDocument(SignNowDocument document, string email, Token token)
        {
            // using token from the Authorization step
            var signNowContext = new SignNowContext(token);

            // create embedded signing invite
            var invite = new EmbeddedSigningInvite(document);
            invite.AddEmbeddedSigningInvite(
                new EmbeddedInvite
                {
                    Email = email,
                    RoleId = document.Roles[0].Id,
                    SigningOrder = 1
                });

            return await signNowContext.Invites.CreateInviteAsync(document.Id, invite)
                .ConfigureAwait(false);
        }
    }
}
