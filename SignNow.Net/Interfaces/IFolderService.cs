using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;

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
        /// <param name="cancellation">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<Folder> GetFolderAsync(string folderId, CancellationToken cancellation = default);
    }
}
