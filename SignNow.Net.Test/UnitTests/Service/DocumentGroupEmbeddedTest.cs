using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using UnitTests;

namespace UnitTests
{
    [TestClass]
    public class DocumentGroupEmbeddedTest : SignNowTestBase
    {
        private const string GroupId = "a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";
        private const string EmbeddedInviteId = "b1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";

        private static string MakeLinkJson(string url = "https://app.signnow.com/group/abc")
            => JsonConvert.SerializeObject(new { data = new { link = url } });

        [TestMethod]
        public async Task CreateDocumentGroupEmbeddedInviteAsync_Success()
        {
            var expectedJson = JsonConvert.SerializeObject(new
            {
                data = new[]
                {
                    new { id = "inviteId123", email = "s@example.com", role_id = "r1", order = 1, status = "pending" }
                }
            });
            var mockClient = SignNowClientMock(expectedJson);
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);
            var request = new CreateDocumentGroupEmbeddedInviteRequest
            {
                Invites = new List<DocumentGroupEmbeddedInviteSigner>
                {
                    new DocumentGroupEmbeddedInviteSigner
                    {
                        Email = "s@example.com",
                        RoleId = "r1",
                        SigningOrder = 1
                    }
                }
            };

            var result = await service.CreateDocumentGroupEmbeddedInviteAsync(GroupId, request, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.InviteData.Count);
        }

        [TestMethod]
        public async Task GenerateDocumentGroupEmbeddedInviteLinkAsync_Success()
        {
            var mockClient = SignNowClientMock(MakeLinkJson());
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);
            var options = new CreateDocumentGroupEmbedLinkOptions { LinkExpiration = 30 };

            var result = await service.GenerateDocumentGroupEmbeddedInviteLinkAsync(GroupId, EmbeddedInviteId, options, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Link, typeof(Uri));
        }

        [TestMethod]
        public async Task CancelDocumentGroupEmbeddedInviteAsync_Success()
        {
            var mockClient = SignNowClientMock("{}");
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);

            await service.CancelDocumentGroupEmbeddedInviteAsync(GroupId, CancellationToken.None);
        }

        [TestMethod]
        public async Task GenerateDocumentGroupEmbeddedEditorLinkAsync_Success()
        {
            var mockClient = SignNowClientMock(MakeLinkJson("https://app.signnow.com/group-editor/abc"));
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);

            var result = await service.GenerateDocumentGroupEmbeddedEditorLinkAsync(
                GroupId,
                new EmbeddedEditorOptions { LinkExpiration = 30 },
                CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Link, typeof(Uri));
        }

        [TestMethod]
        public async Task GenerateDocumentGroupEmbeddedSendingLinkAsync_Success()
        {
            var mockClient = SignNowClientMock(MakeLinkJson("https://app.signnow.com/group-sending/abc"));
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);

            var result = await service.GenerateDocumentGroupEmbeddedSendingLinkAsync(
                GroupId,
                new EmbeddedSendingOptions { LinkExpiration = 30 },
                CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Link, typeof(Uri));
        }
    }
}
