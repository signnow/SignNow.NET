using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Invites
{
    public static partial class InviteExamples
    {
        /// <summary>
        /// Cancel an embedded signing invite
        /// </summary>
        /// <param name="document">signNow document you’d like to cancel embedded invite</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns></returns>
        public static async Task CancelEmbeddedInvite(SignNowDocument document, SignNowContext signNowContext)
        {
            await signNowContext.Invites
                .CancelEmbeddedInviteAsync(document.Id)
                .ConfigureAwait(false);
        }
    }
}
