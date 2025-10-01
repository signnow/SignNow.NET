using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples
{
    public partial class FolderExamples
    {
        [TestMethod]
        public async Task DeleteFolderAsync()
        {
            // Create some folder for test inside the Documents folder
            var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");
            var folderToDelete = await testContext.Folders
                .CreateFolderAsync("DeleteMe", documentsFolder?.Id)
                .ConfigureAwait(false);

            // Check if test folder exists
            var createdFolder = await testContext.Folders
                .GetFolderAsync(folderToDelete.Id)
                .ConfigureAwait(false);
            Assert.AreEqual(folderToDelete.Id, createdFolder.Id);

            // Delete folder
            await testContext.Folders.DeleteFolderAsync(folderToDelete.Id).ConfigureAwait(false);

            // Check if test folder has been deleted
            var folders = await testContext.Folders
                .GetFolderAsync(documentsFolder?.Id, new GetFolderOptions {IncludeDocumentsSubfolder = false})
                .ConfigureAwait(false);

            Assert.IsFalse(folders.Folders.Any(f => f.Name == "DeleteMe"));
        }
    }
}
