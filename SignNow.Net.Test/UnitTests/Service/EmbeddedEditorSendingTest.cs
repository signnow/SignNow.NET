using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Service;
using UnitTests;

namespace UnitTests
{
    [TestClass]
    public class EmbeddedEditorSendingTest : SignNowTestBase
    {
        private const string DocumentId = "a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";

        private static string MakeLinkJson(string url = "https://app.signnow.com/editor/abc123")
            => JsonConvert.SerializeObject(new { data = new { link = url } });

        [TestMethod]
        public async Task GenerateEmbeddedEditorLinkAsync_Success()
        {
            var mockClient = SignNowClientMock(MakeLinkJson());
            var service = new UserService(ApiBaseUrl, new Token(), mockClient);
            var options = new EmbeddedEditorOptions { LinkExpiration = 30 };

            var result = await service.GenerateEmbeddedEditorLinkAsync(DocumentId, options, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Link, typeof(Uri));
        }

        [TestMethod]
        public async Task GenerateEmbeddedSendingLinkAsync_Success()
        {
            var mockClient = SignNowClientMock(MakeLinkJson("https://app.signnow.com/sending/abc123"));
            var service = new UserService(ApiBaseUrl, new Token(), mockClient);
            var options = new EmbeddedSendingOptions { LinkExpiration = 60 };

            var result = await service.GenerateEmbeddedSendingLinkAsync(DocumentId, options, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Link, typeof(Uri));
        }

        [TestMethod]
        public async Task GenerateEmbeddedEditorLinkAsync_NoExpiration_Success()
        {
            var mockClient = SignNowClientMock(MakeLinkJson());
            var service = new UserService(ApiBaseUrl, new Token(), mockClient);

            var result = await service.GenerateEmbeddedEditorLinkAsync(DocumentId, new EmbeddedEditorOptions(), CancellationToken.None);

            Assert.IsNotNull(result);
        }
    }
}
