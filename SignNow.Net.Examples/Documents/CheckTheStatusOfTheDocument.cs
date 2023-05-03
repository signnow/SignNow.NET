using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Check the Status of the Document
        /// <para>Can be one of:</para>
        /// <list type="bullet">
        /// <item><description><see cref="DocumentStatus.NoInvite"/></description></item>
        /// <item><description><see cref="DocumentStatus.Pending"/></description></item>
        /// <item><description><see cref="DocumentStatus.Completed"/></description></item>
        /// </list>
        /// </summary>
        /// <param name="documentId">Identity of the document</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns><see cref="DocumentStatus"/></returns>
        public static async Task<DocumentStatus> CheckTheStatusOfTheDocument(string documentId, SignNowContext signNowContext)
        {
            var document = await signNowContext.Documents
                .GetDocumentAsync(documentId).ConfigureAwait(false);

            return document.Status;
        }
    }
}
