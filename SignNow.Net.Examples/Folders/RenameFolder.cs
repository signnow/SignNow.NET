using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class FolderExamples
    {
        [TestMethod]
        public async Task RenameFolderAsync()
        {
            // Creates folder inside Documents folder for test
            var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);

            // Get Documents folder
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");

            // create a folder for test
            var folderForRename = await testContext.Folders
                .CreateFolderAsync("noname", documentsFolder?.Id)
                .ConfigureAwait(false);

            // Rename previously created folder
            var renameFolder = await testContext.Folders
                .RenameFolderAsync("ItsRenamedFolder", folderForRename.Id)
                .ConfigureAwait(false);

            var renamed = await testContext.Folders.GetFolderAsync(renameFolder.Id).ConfigureAwait(false);

            // Check if folder renamed
            Assert.AreEqual("ItsRenamedFolder", renamed.Name);
            Assert.AreEqual(folderForRename.Id, renamed.Id);

            // Finally - delete test folder
            await testContext.Folders.DeleteFolderAsync(renamed.Id).ConfigureAwait(false);
        }
    }
}
