using SignNow.Net.Model;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface IDocumentService
    {
        /// <summary>
        /// Uploads a file to the SignNow account and creates a document. This method accepts .doc, .docx, .pdf, and .png file types.
        /// </summary>
        /// <param name="documentContent">Document content stream</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Operation result object containing ID of the new document.</returns>
        Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Uploads a file to the SignNow account, creates a document and extracts simple field tags if any. This method accepts .doc, .docx and .pdf file types. See <see cref="https://campus.barracuda.com/product/signnow/doc/41113461/rest-endpoints-api"/>
        /// </summary>
        /// <param name="documentContent">Document content stream</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Operation result object containing ID of the new document.</returns>
        Task<UploadDocumentResponse> UploadDocumentWithFieldExtractAsync(Stream documentContent, CancellationToken cancellationToken = default);
        Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default);
        Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default);
    }
}
