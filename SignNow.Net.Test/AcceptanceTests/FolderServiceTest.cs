using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class FolderServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task GetFolders()
        {
            var folders = await SignNowTestContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);

            Assert.IsInstanceOfType(folders, typeof(SignNowFolders));
            Assert.AreEqual("Root", folders.Name);
            Assert.IsTrue(folders.SystemFolder);

            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Documents"));
            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Archive"));
            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Templates"));
        }

        [TestMethod]
        public async Task GetFolderAsync_OriginalEndpoint()
        {
            var root = await SignNowTestContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");

            // Test the original endpoint
            var folder = await SignNowTestContext.Folders
                .GetFolderAsync(documentsFolder?.Id)
                .ConfigureAwait(false);

            Assert.IsInstanceOfType(folder, typeof(SignNowFolders));
            Assert.AreEqual(documentsFolder?.Id, folder.Id);
            Assert.AreEqual("Documents", folder.Name);
            Assert.IsTrue(folder.SystemFolder);
        }

        [TestMethod]
        public async Task GetFolderAsync_WithOptions()
        {
            var root = await SignNowTestContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");

            var options = new GetFolderOptions
            {
                Limit = 5,
                Offset = 0,
                EntityTypes = EntityType.All,
                IncludeDocumentsSubfolder = false
            };

            // Test the original endpoint with options
            var folder = await SignNowTestContext.Folders
                .GetFolderAsync(documentsFolder?.Id, options)
                .ConfigureAwait(false);

            Assert.IsInstanceOfType(folder, typeof(SignNowFolders));
            Assert.AreEqual(documentsFolder?.Id, folder.Id);
            Assert.AreEqual("Documents", folder.Name);
            Assert.IsTrue(folder.SystemFolder);
        }

        [TestMethod]
        public async Task GetFolderByIdAsync_NewEndpoint()
        {
            var root = await SignNowTestContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");

            var folderById = await SignNowTestContext.Folders
                .GetFolderByIdAsync(documentsFolder?.Id)
                .ConfigureAwait(false);

            Assert.IsInstanceOfType(folderById, typeof(SignNowFolders));
            Assert.AreEqual(documentsFolder?.Id, folderById.Id);
            Assert.AreEqual("Documents", folderById.Name);
            Assert.IsTrue(folderById.SystemFolder);
        }

        [TestMethod]
        public async Task GetFolderByIdAsync_WithOptions()
        {
            var root = await SignNowTestContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");

            var options = new GetFolderOptions
            {
                Limit = 5,
                Offset = 0,
                EntityTypes = EntityType.All,
                IncludeDocumentsSubfolder = false
            };

            var folderById = await SignNowTestContext.Folders
                .GetFolderByIdAsync(documentsFolder?.Id, options)
                .ConfigureAwait(false);

            Assert.IsInstanceOfType(folderById, typeof(SignNowFolders));
            Assert.AreEqual(documentsFolder?.Id, folderById.Id);
            Assert.AreEqual("Documents", folderById.Name);
            Assert.IsTrue(folderById.SystemFolder);
        }

        [TestMethod]
        public async Task CanCreatesFolderAsync()
        {
            var testFolderName = $"testCanCreatesFolderAsync_{Faker.Hacker.Random.Word()}";
            var root = await SignNowTestContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");

            // Create new folder
            var createNewFolder = await SignNowTestContext.Folders
                .CreateFolderAsync(testFolderName, documentsFolder?.Id)
                .ConfigureAwait(false);

            Assert.AreEqual(createNewFolder.Id.ValidateId(), createNewFolder.Id);

            // Check if new folder exists
            var checkNewFolderExists = await SignNowTestContext.Folders
                .GetFolderAsync(documentsFolder?.Id, new GetFolderOptions {IncludeDocumentsSubfolder = false})
                .ConfigureAwait(false);
            var testFolder = checkNewFolderExists.Folders.FirstOrDefault(f => f.Name == testFolderName);
            Assert.AreEqual(testFolderName, testFolder?.Name);


            // Rename newly created folder
            var renamedFolderRequest = await SignNowTestContext.Folders
                .RenameFolderAsync(testFolderName + "_renamed", testFolder?.Id)
                .ConfigureAwait(false);

            var renamedFolder = await SignNowTestContext.Folders
                .GetFolderAsync(renamedFolderRequest.Id)
                .ConfigureAwait(false);
            Assert.AreEqual(renamedFolderRequest.Id, renamedFolder.Id);
            Assert.AreEqual(testFolderName + "_renamed", renamedFolder.Name);


            // Delete test folder
            await SignNowTestContext.Folders
                .DeleteFolderAsync(renamedFolder.Id)
                .ConfigureAwait(false);
            var checkFolderDeleted = await SignNowTestContext.Folders
                .GetFolderAsync(documentsFolder?.Id)
                .ConfigureAwait(false);

            Assert.AreEqual(0, checkFolderDeleted.Folders.Count(f => f.Name == testFolderName + "_renamed"));
        }
    }
}
