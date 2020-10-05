using System.Collections.Generic;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Merge two documents into one final document
        /// </summary>
        /// <param name="documentName">New Document name with extension</param>
        /// <param name="documentsList">List of the documents to be merged</param>
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task<DownloadDocumentResponse> MergeTwoDocuments(string documentName, IEnumerable<SignNowDocument> documentsList, Token token)
        {
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Documents
                .MergeDocumentsAsync(documentName, documentsList)
                .ConfigureAwait(false);
        }
    }
}
