using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Folders
{
    public static partial class FolderExamples
    {
        public static async Task<Folder> GetFolder(string folderId, Token token)
        {
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Folders
                .GetFolderAsync(folderId)
                .ConfigureAwait(false);
        }
    }
}
