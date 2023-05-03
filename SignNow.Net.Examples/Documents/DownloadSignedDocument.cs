using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Downloads signed document
        /// </summary>
        /// <param name="documentId">ID of signed document</param>
        /// <param name="signNowContext">signNow container with services.</param>
        public static async Task<DownloadDocumentResponse> DownloadSignedDocument(string documentId, SignNowContext signNowContext)
        {
            return await signNowContext.Documents
                .DownloadDocumentAsync(documentId, DownloadType.PdfCollapsed)
                .ConfigureAwait(false);
        }
    }
}
