using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Folders
{
    public static partial class FolderExamples
    {
        /// <summary>
        /// Get all folders of a user.
        /// </summary>
        /// <param name="token">Access token</param>
        /// <returns>Returns all information about user's folders.</returns>
        public static async Task<SignNowFolders> GetAllFolders(Token token)
        {
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Folders
                .GetAllFoldersAsync()
                .ConfigureAwait(false);
        }
    }
}
