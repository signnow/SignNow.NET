using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples.Folders
{
    public static partial class FolderExamples
    {
        public static async Task<SignNowFolders> GetFolder(string folderId, GetFolderOptions options, Token token)
        {
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Folders
                .GetFolderAsync(folderId, options)
                .ConfigureAwait(false);
        }
    }
}
