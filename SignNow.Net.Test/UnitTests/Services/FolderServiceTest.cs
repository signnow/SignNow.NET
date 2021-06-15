using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class FolderServiceTest : SignNowTestBase
    {
        [TestMethod]
        public void GetAllFolders()
        {
            var fakeFolders = new SignNowFoldersFaker().Generate();
            var foldersJson = TestUtils.SerializeToJsonFormatted(fakeFolders);

            var service = new FolderService(ApiBaseUrl, new Token(), SignNowClientMock(foldersJson));

            var foldersResponse = service
                .GetAllFoldersAsync()
                .Result;

            Assert.AreEqual(fakeFolders.Id, foldersResponse.Id);
            Assert.AreEqual(fakeFolders.UserId, foldersResponse.UserId);
            Assert.AreEqual(fakeFolders.Documents.Count, foldersResponse.Documents.Count);
        }
    }
}
