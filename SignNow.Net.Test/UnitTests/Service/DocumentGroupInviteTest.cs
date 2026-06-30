using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using UnitTests;

namespace UnitTests
{
    [TestClass]
    public class DocumentGroupInviteTest : SignNowTestBase
    {
        private const string GroupId = "a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";
        private const string InviteId = "b1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";
        private const string StepId = "c1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";

        private static string MakeInviteResponseJson()
            => JsonConvert.SerializeObject(new
            {
                data = new
                {
                    id = InviteId,
                    status = "pending",
                    created = 1700000000,
                    updated = 1700000000,
                    steps = new[] { new { id = StepId, order = 1, status = "pending" } }
                }
            });

        [TestMethod]
        public async Task CreateGroupInviteAsync_Success()
        {
            var mockClient = SignNowClientMock(MakeInviteResponseJson());
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);
            var request = new CreateGroupInviteRequest
            {
                InviteSteps = new List<GroupInviteStep>
                {
                    new GroupInviteStep
                    {
                        Order = 1,
                        InviteEmails = new List<GroupInviteEmail>
                        {
                            new GroupInviteEmail
                            {
                                Email = "signer@example.com",
                                Role = "Signer 1",
                                RoleId = "roleId123",
                                Order = 1
                            }
                        },
                        InviteActions = new List<GroupInviteAction>
                        {
                            new GroupInviteAction
                            {
                                Email = "signer@example.com",
                                RoleName = "Signer 1",
                                DocumentId = "docId123"
                            }
                        }
                    }
                }
            };

            var result = await service.CreateGroupInviteAsync(GroupId, request, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("pending", result.Data.Status);
        }

        [TestMethod]
        public async Task GetGroupInviteAsync_Success()
        {
            var mockClient = SignNowClientMock(MakeInviteResponseJson());
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);

            var result = await service.GetGroupInviteAsync(GroupId, InviteId, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(InviteId, result.Data.Id);
        }

        [TestMethod]
        public async Task CancelGroupInviteAsync_Success()
        {
            var mockClient = SignNowClientMock("{}");
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);

            await service.CancelGroupInviteAsync(GroupId, InviteId, CancellationToken.None);
        }

        [TestMethod]
        public async Task ResendGroupInviteAsync_Success()
        {
            var mockClient = SignNowClientMock("{}");
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);

            await service.ResendGroupInviteAsync(GroupId, InviteId, CancellationToken.None);
        }

        [TestMethod]
        public async Task GetPendingGroupInvitesAsync_Success()
        {
            var pendingJson = JsonConvert.SerializeObject(new
            {
                data = new[]
                {
                    new { id = "pending1", email = "signer@example.com", role = "Signer 1", status = "pending" }
                }
            });
            var mockClient = SignNowClientMock(pendingJson);
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);

            var result = await service.GetPendingGroupInvitesAsync(GroupId, InviteId, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual("signer@example.com", result.Data[0].Email);
        }

        [TestMethod]
        public async Task ReassignSignerAsync_Success()
        {
            var mockClient = SignNowClientMock("{}");
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);
            var request = new ReassignSignerRequest
            {
                NewSigner = new ReassignSignerInfo
                {
                    Email = "new.signer@example.com",
                    RoleName = "Signer 1"
                }
            };

            await service.ReassignSignerAsync(GroupId, InviteId, StepId, request, CancellationToken.None);
        }
    }
}
