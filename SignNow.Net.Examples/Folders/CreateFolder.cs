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
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task<FolderIdentityResponse> CreateFolder(string name, string parentId, Token token)
        {
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Folders
                .CreateFolderAsync(name, parentId)
                .ConfigureAwait(false);
        }
    }
}
