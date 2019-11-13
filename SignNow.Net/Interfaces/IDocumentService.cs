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
        /// <param name="fileName">Uploaded document file name</param>
        /// <returns>Operation result object containing ID of the new document.</returns>
        Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a file to the SignNow account, creates a document and extracts simple field tags if any.
        /// This method accepts .doc, .docx and .pdf file types.
        /// See <a href="https://campus.barracuda.com/product/signnow/doc/41113461/rest-endpoints-api">REST API Endpoints</a>
        /// </summary>
        /// <param name="documentContent">Document content stream</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <param name="fileName">Uploaded document file name</param>
        /// <returns>Operation result object containing ID of the new document.</returns>
        Task<UploadDocumentResponse> UploadDocumentWithFieldExtractAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a document from SignNow account.
        /// </summary>
        /// <param name="documentId">Identity of the document to be removed.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates URL to sign the document containing fillable fields using web browser.
        /// </summary>
        /// <param name="documentId">Identity of the document to create signing link for</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Operation result object containing URL to sign the document using web browser.</returns>
        Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a Collapsed/Zipped Document.
        /// </summary>
        /// <param name="documentId">Identity of the document to create signing link for</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Operation result object containing URL to sign the document using web browser.</returns>
        Task<Stream> DownloadDocumentAsync(string documentId, CancellationToken cancellationToken = default);
    }
}
