using SignNow.Net.Model;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface IDocumentService
    {
        Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, bool extractFields = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes a previously uploaded document.
        /// </summary>
        /// <param name="documentId">Document ID (string)</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Operation result object containing `status` of the operation.</returns>
        Task<DeleteDocumentResponse> DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default(CancellationToken));

        Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
