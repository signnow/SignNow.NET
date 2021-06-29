using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for any operations with a folders in SignNow
    /// </summary>
    /// <remarks>
    /// Folders serve for storing user's documents. <br />
    /// By default, there are three folders in every account:
    /// <list type="bullet">
    /// <item><description>Documents</description></item>
    /// <item><description>Templates</description></item>
    /// <item><description>Archive</description></item>
    /// </list>
    /// Subfolders can be created only for these three and their child folders.
    /// </remarks>
    public interface IFolderService
    {
        /// <summary>
        /// Returns all folders of a <see cref="User"/>.
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<SignNowFolders> GetAllFoldersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all details of a specific folder including all documents in that folder.
        /// </summary>
        /// <param name="folderId">ID of the folder to get details of</param>
        /// <param name="options">Folder filter and sort options</param>
        /// <param name="cancellation">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<SignNowFolders> GetFolderAsync(string folderId, GetFolderOptions options = default, CancellationToken cancellation = default);

        /// <summary>
        /// Creates a folder for the user.
        /// </summary>
        /// <param name="name">Name of a new folder</param>
        /// <param name="parentId">Identifier for the parent folder that contains this folder.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Returns ID of a new folder.</returns>
        Task<FolderIdentityResponse> CreateFolderAsync(string name, string parentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a folder.
        /// </summary>
        /// <param name="folderId">ID of the folder to delete.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task DeleteFolderAsync(string folderId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Renames a folder.
        /// </summary>
        /// <param name="name">A new folder's name.</param>
        /// <param name="folderId">ID of the folder to rename.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Returns ID of the renamed folder.</returns>
        Task<FolderIdentityResponse> RenameFolderAsync(string name, string folderId, CancellationToken cancellationToken = default);
    }
}
