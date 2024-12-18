using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Examples
{
    public partial class FolderExamples
    {
        [TestMethod]
        public async Task GetFolderAsync()
        {
            // Get all folders
            var folders = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);

            // Get folderId of the "Documents" folder
            var folderId = folders.Folders.FirstOrDefault(f => f.Name == "Documents")?.Id;

            var filterBySigningStatus = new GetFolderOptions
            {
                Filters = new FolderFilters(SigningStatus.Pending)
            };

            // Get all details of a specific folder including a list of all documents in that folder
            var folder = await testContext.Folders
                .GetFolderAsync(folderId, filterBySigningStatus)
                .ConfigureAwait(false);

            // Check if folder contains only pending documents
            Assert.IsTrue(folders.Documents.All(d => d.Status == DocumentStatus.Pending));
            Assert.AreEqual(folders.TotalDocuments, folders.Documents.Count);
            Assert.IsTrue(folder.SystemFolder);
            Assert.AreEqual(folderId, folder.Id);
            Assert.AreEqual(folders.Id, folder.ParentId);
        }
    }
}
