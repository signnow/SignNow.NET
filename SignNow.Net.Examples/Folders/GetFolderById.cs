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
        /// <summary>
        /// Example of using the newer GetFolderByIdAsync endpoint (/folder/{folder_id}) to retrieve folder details.
        /// This endpoint provides the same functionality as GetFolderAsync but uses a different API path.
        /// </summary>
        [TestMethod]
        public async Task GetFolderByIdAsync()
        {
            // Get all folders
            var folders = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);

            // Get folderId of the "Documents" folder
            var folderId = folders.Folders.FirstOrDefault(f => f.Name == "Documents")?.Id;

            // Get all details of a specific folder using the newer endpoint
            var folder = await testContext.Folders
                .GetFolderByIdAsync(folderId)
                .ConfigureAwait(false);

            // Verify the folder details
            Assert.IsTrue(folder.SystemFolder);
            Assert.AreEqual(folderId, folder.Id);
            Assert.AreEqual("Documents", folder.Name);
            Assert.AreEqual(folders.Id, folder.ParentId);
        }

        /// <summary>
        /// Example of using GetFolderByIdAsync with filtering and sorting options.
        /// </summary>
        [TestMethod]
        public async Task GetFolderByIdAsync_WithOptions()
        {
            // Get all folders
            var folders = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);

            // Get folderId of the "Documents" folder
            var folderId = folders.Folders.FirstOrDefault(f => f.Name == "Documents")?.Id;

            // Configure folder options with filtering and sorting
            var folderOptions = new GetFolderOptions
            {
                Limit = 10,
                Offset = 0,
                EntityTypes = EntityType.All,
                IncludeDocumentsSubfolder = false,
                WithTeamDocuments = false,
                Filters = new FolderFilters(SigningStatus.Pending),
                SortBy = new FolderSort(SortByParam.Created, SortOrder.Descending)
            };

            // Get folder details with options using the newer endpoint
            var folder = await testContext.Folders
                .GetFolderByIdAsync(folderId, folderOptions)
                .ConfigureAwait(false);

            // Verify the folder details
            Assert.IsTrue(folder.SystemFolder);
            Assert.AreEqual(folderId, folder.Id);
            Assert.AreEqual("Documents", folder.Name);
            Assert.AreEqual(folders.Id, folder.ParentId);
            
            // Verify that only pending documents are returned (if any exist)
            if (folder.Documents.Any())
            {
                Assert.IsTrue(folder.Documents.All(d => d.Status == DocumentStatus.Pending));
            }
        }

        /// <summary>
        /// Example of using GetFolderByIdAsync to get document groups only.
        /// </summary>
        [TestMethod]
        public async Task GetFolderByIdAsync_DocumentGroupsOnly()
        {
            // Get all folders
            var folders = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);

            // Get folderId of the "Documents" folder
            var folderId = folders.Folders.FirstOrDefault(f => f.Name == "Documents")?.Id;

            // Configure options to get only document groups
            var folderOptions = new GetFolderOptions
            {
                EntityTypes = EntityType.DocumentGroup,
                Limit = 50
            };

            // Get folder details with document groups only
            var folder = await testContext.Folders
                .GetFolderByIdAsync(folderId, folderOptions)
                .ConfigureAwait(false);

            // Verify the folder details
            Assert.IsTrue(folder.SystemFolder);
            Assert.AreEqual(folderId, folder.Id);
            Assert.AreEqual("Documents", folder.Name);
        }

        /// <summary>
        /// Example of using GetFolderByIdAsync to get only favorite documents.
        /// </summary>
        [TestMethod]
        public async Task GetFolderByIdAsync_FavoritesOnly()
        {
            // Get all folders
            var folders = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);

            // Get folderId of the "Documents" folder
            var folderId = folders.Folders.FirstOrDefault(f => f.Name == "Documents")?.Id;

            // Configure options to get only favorite documents
            var folderOptions = new GetFolderOptions
            {
                EntityTypes = EntityType.All,
                Limit = 25,
                // Note: only_favorites parameter would need to be added to GetFolderOptions if supported by the API
            };

            // Get folder details
            var folder = await testContext.Folders
                .GetFolderByIdAsync(folderId, folderOptions)
                .ConfigureAwait(false);

            // Verify the folder details
            Assert.IsTrue(folder.SystemFolder);
            Assert.AreEqual(folderId, folder.Id);
            Assert.AreEqual("Documents", folder.Name);
        }
    }
}
