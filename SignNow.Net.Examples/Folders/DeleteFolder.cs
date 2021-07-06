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
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task DeleteFolder(string folderId, Token token)
        {
            var signNowContext = new SignNowContext(token);

            await signNowContext.Folders
                .DeleteFolderAsync(folderId)
                .ConfigureAwait(false);
        }
    }
}
