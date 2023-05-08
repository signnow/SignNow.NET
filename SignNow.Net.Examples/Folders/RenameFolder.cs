using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Examples.Folders
{
    public static partial class FolderExamples
    {
        /// <summary>
        /// Rename folder example
        /// </summary>
        /// <param name="name">A new folder's name</param>
        /// <param name="folderId">Id of the folder to rename</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns></returns>
        public static async Task<FolderIdentityResponse> RenameFolder(string name, string folderId, SignNowContext signNowContext)
        {
            return await signNowContext.Folders
                .RenameFolderAsync(name, folderId)
                .ConfigureAwait(false);
        }
    }
}
