using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples
{
    public partial class FolderExamples: ExamplesBase
    {
        [TestMethod]
        public async Task CreateFolderAsync()
        {
            // Get Root folder and Documents folder
            var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");

            var timestamp = (long)(DateTime.Now - UnixEpoch).TotalSeconds;
            // Note: You should use different folder name for each example run
            var myFolderName = $"CreateFolderExample_{timestamp}";

            // Creating new folder
            var createNewFolder = await testContext.Folders
                .CreateFolderAsync(myFolderName, documentsFolder?.Id)
                .ConfigureAwait(false);

            Assert.IsNotNull(createNewFolder.Id);

            // Check if new folder exists
            var checkNewFolderExists = await testContext.Folders
                .GetFolderAsync(documentsFolder?.Id, new GetFolderOptions {IncludeDocumentsSubfolder = false})
                .ConfigureAwait(false);

            var myFolder = checkNewFolderExists.Folders.FirstOrDefault(f => f.Name == myFolderName);
            Assert.AreEqual(myFolderName, myFolder?.Name);

            // clean up
            await testContext.Folders.DeleteFolderAsync(myFolder?.Id).ConfigureAwait(false);
        }
    }
}
