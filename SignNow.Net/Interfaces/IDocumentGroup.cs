using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for any operations with a Document Groups in signNow
    /// can be used to create, rename, delete, move a document group etc.
    /// </summary>
    public interface IDocumentGroup
    {
        /// <summary>
        /// Creates a document group from a list of document ids
        /// </summary>
        /// <param name="groupName">Name for the Document Group</param>
        /// <param name="documents">The list of the documents to create the document group.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<DocumentGroupCreateResponse> CreateDocumentGroupAsync(string groupName, IEnumerable<SignNowDocument> documents, CancellationToken cancellationToken = default);
    }
}
