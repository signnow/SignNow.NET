using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples.Folders
{
    public static partial class FolderExamples
    {
        /// <summary>
        /// Get all details of a specific folder including a list of all documents in that folder.
        /// </summary>
        /// <param name="folderId">ID of the folder to get details of</param>
        /// <param name="options">Options like: sort, filter, limit, etc...</param>
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task<SignNowFolders> GetFolder(string folderId, GetFolderOptions options, Token token)
        {
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Folders
                .GetFolderAsync(folderId, options)
                .ConfigureAwait(false);
        }
    }
}
