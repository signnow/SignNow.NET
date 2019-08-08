using SignNow.Net.Model;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interface
{
    public interface IDocumentService
    {
        Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, bool extractFields = true, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default(CancellationToken));

        Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
