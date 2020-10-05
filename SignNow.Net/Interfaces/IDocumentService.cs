using System.Collections.Generic;
using SignNow.Net.Model;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for any operations with a Document in SignNow; can be used to create, download, retrieve, delete a document etc.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Retrieves a document detailed data.
        /// </summary>
        /// <param name="documentId">Identity of the document to be viewed.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">If document identity is not valid.</exception>
        Task<SignNowDocument> GetDocumentAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a file to the SignNow account and creates a document. This method accepts .doc, .docx, .pdf, and .png file types.
        /// </summary>
        /// <param name="documentContent">Document content stream</param>
        /// <param name="fileName">Uploaded document file name</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Operation result object containing ID of the new document.</returns>
        Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a file to the SignNow account, creates a document and extracts simple field tags if any.
        /// This method accepts .doc, .docx and .pdf file types.
        /// See <a href="https://campus.barracuda.com/product/signnow/doc/41113461/rest-endpoints-api">REST API Endpoints</a>
        /// </summary>
        /// <param name="documentContent">Document content stream</param>
        /// <param name="fileName">Uploaded document file name</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Operation result object containing ID of the new document.</returns>
        Task<UploadDocumentResponse> UploadDocumentWithFieldExtractAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a document from SignNow account.
        /// </summary>
        /// <param name="documentId">Identity of the document to be removed.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">If document identity is not valid.</exception>
        Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates URL to sign the document containing fillable fields using web browser.
        /// </summary>
        /// <param name="documentId">Identity of the document to create signing link for</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Operation result object containing URL to sign the document using web browser.</returns>
        /// <exception cref="System.ArgumentException">If document identity is not valid.</exception>
        Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a Collapsed/Zipped Document.
        /// </summary>
        /// <param name="documentId">Identity of the document to create signing link for</param>
        /// <param name="type">Download document <see cref="DownloadType">type</see></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Operation result object containing File info with Stream file content.</returns>
        /// <exception cref="System.ArgumentException">If document identity is not valid.</exception>
        Task<DownloadDocumentResponse> DownloadDocumentAsync(string documentId, DownloadType type = DownloadType.PdfCollapsed, CancellationToken cancellationToken = default);

        /// <summary>
        /// Merges two or more documents into one.
        /// </summary>
        /// <param name="documentName">The name of the document with extension that will be created and written to</param>
        /// <param name="documents">Collection of the <see cref="SignNowDocument">documents</see></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Operation result object containing File info with Stream file content.</returns>
        /// <exception cref="System.ArgumentException">If document identity is not valid.</exception>
        Task<DownloadDocumentResponse> MergeDocumentsAsync(string documentName, IEnumerable<SignNowDocument> documents, CancellationToken cancellationToken = default);
    }
}
