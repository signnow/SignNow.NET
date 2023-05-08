using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Create a one-time use URL for anyone to download the document as a PDF.
        /// </summary>
        /// <param name="documentId">Identity of the document</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns><see cref="DownloadLinkResponse"/></returns>
        public static async Task<DownloadLinkResponse>
            CreateOneTimeLinkToDownloadTheDocument(string documentId, SignNowContext signNowContext)
        {
            return await signNowContext.Documents
                .CreateOneTimeDownloadLinkAsync(documentId)
                .ConfigureAwait(false);
        }
    }
}
