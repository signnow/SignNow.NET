using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class FolderServiceTest : SignNowTestBase
    {
        private const string FolderId = "2e1290474698e9c7fa4e0ecbbd26ea03c9df94ce";

        [TestMethod]
        public async Task GetAllFoldersAsync()
        {
            var fakeFolders = new SignNowFoldersFaker().Generate();
            var foldersJson = TestUtils.SerializeToJsonFormatted(fakeFolders);

            var service = new FolderService(ApiBaseUrl, new Token(), SignNowClientMock(foldersJson));

            var foldersResponse = await service
                .GetAllFoldersAsync()
                .ConfigureAwait(false);

            Assert.AreEqual(fakeFolders.Id, foldersResponse.Id);
            Assert.AreEqual(fakeFolders.UserId, foldersResponse.UserId);
            Assert.AreEqual(fakeFolders.Documents.Count, foldersResponse.Documents.Count);
        }

        [TestMethod]
        public async Task CreateFolderAsync()
        {
            var createResponse = new FolderIdentityResponse { Id = FolderId };
            var responseJson = TestUtils.SerializeToJsonFormatted(createResponse);

            var service = new FolderService(ApiBaseUrl, new Token(), SignNowClientMock(responseJson));

            var folderResponse = await service
                .CreateFolderAsync("testFolderName", FolderId)
                .ConfigureAwait(false);

            Assert.AreEqual(FolderId, folderResponse.Id);
        }

        [TestMethod]
        public void DeleteFolderAsync()
        {
            var service = new FolderService(ApiBaseUrl, new Token(), SignNowClientMock(""));

            var task = service.DeleteFolderAsync(FolderId);

            Assert.IsFalse(task.IsFaulted);
        }

        [TestMethod]
        public async Task RenameFolderAsync()
        {
            var parentFolderId = FolderId.Replace('2', '5');
            var renameResponse = new FolderIdentityResponse { Id = FolderId };
            var responseJson = TestUtils.SerializeToJsonFormatted(renameResponse);

            var service = new FolderService(ApiBaseUrl, new Token(), SignNowClientMock(responseJson));

            var folderResponse = await service
                .RenameFolderAsync("testFolderName", parentFolderId)
                .ConfigureAwait(false);

            Assert.AreEqual(FolderId, folderResponse.Id);
        }
    }
}
