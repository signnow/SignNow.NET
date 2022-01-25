using System.Collections.Generic;
using SignNow.Net.Model;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model.EditFields;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for any operations with a Document in SignNow
    /// can be used to create, download, retrieve, delete a document etc.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Retrieves a document detailed data.
        /// </summary>
        /// <param name="documentId">Identity of the document to be viewed.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<SignNowDocument> GetDocumentAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a file to the SignNow account and creates a document.
        /// This method accepts .doc, .docx, .pdf, and .png file types.
        /// </summary>
        /// <param name="documentContent">Document content stream</param>
        /// <param name="fileName">Uploaded document file name</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Operation result object containing ID of the new document.</returns>
        Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a file to the SignNow account, creates a document and extracts simple field tags if any.
        /// This method accepts .doc, .docx and .pdf file types.
        /// </summary>
        /// <param name="documentContent">Document content stream</param>
        /// <param name="fileName">Uploaded document file name</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Operation result object containing ID of the new document.</returns>
        Task<UploadDocumentResponse>UploadDocumentWithFieldExtractAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default);

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
        /// <param name="type">Download document <see cref="DownloadType">type</see></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Operation result object containing File info with Stream file content.</returns>
        Task<DownloadDocumentResponse> DownloadDocumentAsync(string documentId, DownloadType type = DownloadType.PdfCollapsed, CancellationToken cancellationToken = default);

        /// <summary>
        /// Merges two or more documents into one.
        /// </summary>
        /// <param name="documentName">The name of the document with extension that will be created and written to</param>
        /// <param name="documents">Collection of the <see cref="SignNowDocument">documents</see></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Operation result object containing File info with Stream file content.</returns>
        Task<DownloadDocumentResponse> MergeDocumentsAsync(string documentName, IEnumerable<SignNowDocument> documents, CancellationToken cancellationToken = default);

        /// <summary>
        /// Moves a document into specified folder.
        /// </summary>
        /// <param name="documentId">Identity of the document to move to specified folder.</param>
        /// <param name="folderId">of the folder where you'd like to keep this document.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task MoveDocumentAsync(string documentId, string folderId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a document history (action log) data
        /// </summary>
        /// <param name="documentId">Identity of the document to retrieve history list</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>List of Document history items</returns>
        Task<IReadOnlyList<DocumentHistoryResponse>> GetDocumentHistoryAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a one-time use URL for anyone to download the document as a PDF.
        /// </summary>
        /// <param name="documentId">Identity of the document to be downloaded with one-time link</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>link to download specified document in PDF format</returns>
        Task<DownloadLinkResponse> CreateOneTimeDownloadLinkAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a template by flattening an existing document.
        /// </summary>
        /// <param name="documentId">Identity of the document which is the source of a template</param>
        /// <param name="templateName">The new template name</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Returns a new template ID</returns>
        Task<CreateTemplateFromDocumentResponse> CreateTemplateFromDocumentAsync(string documentId, string templateName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates document from template.
        /// </summary>
        /// <param name="templateId">Identity of the template which is the source of a document.</param>
        /// <param name="documentName">The name of new document</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Returns identity of new Document</returns>
        Task<CreateDocumentFromTemplateResponse> CreateDocumentFromTemplateAsync(string templateId, string documentName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds values to fields that the Signers can later edit when they receive the document for signature.
        /// Works only with Text field types.
        /// </summary>
        /// <param name="documentId">Identity of the document to prefill values for.</param>
        /// <param name="fields">Collection of the <see cref="TextField">fields</see></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task PrefillTextFieldsAsync(string documentId, IEnumerable<TextField> fields, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a document by adding/overwriting fields or elements (texts, checks, signatures, hyperlinks, attachments).
        /// </summary>
        /// <param name="documentId">Identity of the document to edit values for.</param>
        /// <param name="fields">Fields – spaces in the document designated for signing and editing (filling in) by the recipient.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<EditDocumentResponse> EditDocumentAsync(string documentId, IEnumerable<IFieldEditable> fields, CancellationToken cancellationToken = default);
    }
}
