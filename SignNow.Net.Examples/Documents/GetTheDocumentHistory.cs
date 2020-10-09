using System.Collections.Generic;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Get Document History example
        /// </summary>
        /// <param name="documentId">Identity of the document</param>
        /// <param name="token">Access token</param>
        /// <returns><see cref="DocumentHistoryResponse"/></returns>
        public static async Task<IReadOnlyList<DocumentHistoryResponse>>
            GetTheDocumentHistory(string documentId, Token token)
        {
            // using token from the Authorization step
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Documents
                .GetDocumentHistoryAsync(documentId)
                .ConfigureAwait(false);
        }
    }
}
