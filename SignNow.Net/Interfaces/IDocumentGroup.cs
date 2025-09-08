using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.DocumentGroup;
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

        /// <summary>
        /// Renames document group
        /// </summary>
        /// <param name="newName">New name for the document group.</param>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task RenameDocumentGroupAsync(string newName, string documentGroupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Allows users to move a document group to another folder. When a document group is moved,
        /// all its documents are moved to the same folder except documents in the Deleted folder (Deleted from Trash)
        /// and documents that are located in the Shared folders.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="folderId">ID of the folder to move the document group to. Allowed folder types: Documents, Archive, Shared Documents folders.</param>
        /// <param name="withSharedDocuments">Whether to move shared documents that are in this document group. With this parameter, a document group can only be moved to trash.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task MoveDocumentGroupAsync(string documentGroupId, string folderId, bool withSharedDocuments = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Copy a document group in any status and set a new name to it.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="newName">The name of the new document group copy.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task CopyDocumentGroupAsync(string documentGroupId, string newName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a document group. Documents within the group are not deleted. Document groups cannot be deleted while they have a group invite pending.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task DeleteDocumentGroupAsync(string documentGroupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Download all documents of the document group.
        /// </summary>
        /// <param name="documentGroupId">ID of the Document Group.</param>
        /// <param name="options">Options for download for Document Group.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<DownloadDocumentResponse> DownloadDocumentGroupAsync(string documentGroupId, DownloadOptions options, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a document group template by adding or removing templates and updating routing details.
        /// </summary>
        /// <param name="documentGroupTemplateId">ID of the Document Group Template.</param>
        /// <param name="updateRequest">Request containing template IDs to add/remove and routing details.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<UpdateDocumentGroupTemplateResponse> UpdateDocumentGroupTemplateAsync(string documentGroupTemplateId, UpdateDocumentGroupTemplateRequest updateRequest, CancellationToken cancellationToken = default);
    }
}
