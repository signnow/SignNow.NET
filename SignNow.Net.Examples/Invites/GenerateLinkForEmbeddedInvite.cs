using System.Linq;
using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples.Invites
{
    public static partial class InviteExamples
    {
        /// <summary>
        /// Create Link for Embedded Signing Invite
        /// </summary>
        /// <param name="document">signNow document you'd like to have signed</param>
        /// <param name="expires">In how many minutes the link expires, ranges from 15 to 45 minutes or null</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns></returns>
        public static async Task<EmbeddedInviteLinkResponse>
            GenerateLinkForEmbeddedInvite(SignNowDocument document, int expires, SignNowContext signNowContext)
        {
            var options = new CreateEmbedLinkOptions
            {
                FieldInvite = document.FieldInvites.First(),
                LinkExpiration = (uint)expires
            };

            return await signNowContext.Invites
                .GenerateEmbeddedInviteLinkAsync(document.Id, options).ConfigureAwait(false);
        }

    }
}
