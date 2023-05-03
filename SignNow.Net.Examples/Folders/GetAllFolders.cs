using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Folders
{
    public static partial class FolderExamples
    {
        /// <summary>
        /// Get all folders of a user.
        /// </summary>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns>Returns all information about user's folders.</returns>
        public static async Task<SignNowFolders> GetAllFolders(SignNowContext signNowContext)
        {
            return await signNowContext.Folders
                .GetAllFoldersAsync()
                .ConfigureAwait(false);
        }
    }
}
