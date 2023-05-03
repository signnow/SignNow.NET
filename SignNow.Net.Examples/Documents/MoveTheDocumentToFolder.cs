using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Move document to specified folder example
        /// </summary>
        /// <param name="documentId">Id of the document to move</param>
        /// <param name="folderId">Id of the folder where you'd like to keep this document</param>
        /// <param name="signNowContext">signNow container with services.</param>
        public static async Task MoveTheDocumentToFolder(string documentId, string folderId, SignNowContext signNowContext)
        {
            await signNowContext.Documents
                .MoveDocumentAsync(documentId, folderId)
                .ConfigureAwait(false);
        }
    }
}
