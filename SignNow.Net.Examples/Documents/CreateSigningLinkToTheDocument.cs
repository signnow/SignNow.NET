using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Create a signing link to the document for signature.
        /// </summary>
        /// <param name="documentId">Identity of the document you’d like to have signed</param>
        /// <param name="token">Access token</param>
        /// <returns>
        /// Response with:
        /// <para><see cref="SigningLinkResponse.Url"/> to sign the document via web browser using signNow credentials.</para>
        /// <para><see cref="SigningLinkResponse.AnonymousUrl"/> to sign the document via web browser without signNow credentials.</para>
        /// </returns>
        public static async Task<SigningLinkResponse> CreateSigningLinkToTheDocument(string documentId, Token token)
        {
            // using token from the Authorization step
            var signNowContext = new SignNowContext(token);

            // using `documentId` from the Upload document step
            return await signNowContext.Documents
                .CreateSigningLinkAsync(documentId)
                .ConfigureAwait(false);
        }
    }
}
