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

        /// <summary>
        /// Getting basic information about document groups.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<DocumentGroupInfoResponse> GetDocumentGroupInfoAsync(string documentGroupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns back all document groups the user owns.
        /// The call is paginated by last_updated, so offset and limit query parameters are required
        /// </summary>
        /// <param name="options">Limit and offset query options</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<DocumentGroupsResponse> GetDocumentGroupsAsync(IQueryToString options, CancellationToken cancellationToken = default);
    }
}
