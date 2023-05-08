using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Examples.Folders
{
    public static partial class FolderExamples
    {
        /// <summary>
        /// Creates a folder for the user
        /// </summary>
        /// <param name="name">Name of a new folder</param>
        /// <param name="parentId">Identifier for the parent folder that contains this folder</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns></returns>
        public static async Task<FolderIdentityResponse> CreateFolder(string name, string parentId, SignNowContext signNowContext)
        {
            return await signNowContext.Folders
                .CreateFolderAsync(name, parentId)
                .ConfigureAwait(false);
        }
    }
}
