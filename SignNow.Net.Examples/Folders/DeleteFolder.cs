using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Folders
{
    public static partial class FolderExamples
    {
        /// <summary>
        /// Delete folder example
        /// </summary>
        /// <param name="folderId">Id of the folder to delete</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns></returns>
        public static async Task DeleteFolder(string folderId, SignNowContext signNowContext)
        {
            await signNowContext.Folders
                .DeleteFolderAsync(folderId)
                .ConfigureAwait(false);
        }
    }
}
