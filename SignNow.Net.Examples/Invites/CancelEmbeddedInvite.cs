using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Invites
{
    public static partial class InviteExamples
    {
        /// <summary>
        /// Cancel an embedded signing invite
        /// </summary>
        /// <param name="document">SignNow document you’d like to cancel embedded invite</param>
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task CancelEmbeddedInvite(SignNowDocument document, Token token)
        {
            // using token from the Authorization step
            var signNowContext = new SignNowContext(token);

            await signNowContext.Invites
                .CancelEmbeddedInviteAsync(document.Id)
                .ConfigureAwait(false);
        }
    }
}
