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
    public class DocumentGroupTemplateRecipientsTest : SignNowTestBase
    {
        private const string TemplateGroupId = "a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";

        [TestMethod]
        public async Task GetDocumentGroupTemplateRecipientsAsync_Success()
        {
            var expectedResponse = new DocumentGroupTemplateRecipientsResponse
            {
                Data = new DocumentGroupTemplateRecipientsData
                {
                    InviteSteps = new[]
                    {
                        new DocumentGroupTemplateRecipientStep
                        {
                            Order = 1,
                            Recipients = new[]
                            {
                                new DocumentGroupTemplateRecipientRole
                                {
                                    Id = "roleId123",
                                    Name = "Signer 1",
                                    SigningOrder = 1,
                                    Email = "signer@example.com"
                                }
                            }
                        }
                    }
                }
            };

            var mockClient = SignNowClientMock(JsonConvert.SerializeObject(expectedResponse));
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);

            var result = await service.GetDocumentGroupTemplateRecipientsAsync(TemplateGroupId, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(1, result.Data.InviteSteps.Count);
            Assert.AreEqual("Signer 1", result.Data.InviteSteps[0].Recipients[0].Name);
        }

        [TestMethod]
        public async Task UpdateDocumentGroupTemplateRecipientsAsync_Success()
        {
            var mockClient = SignNowClientMock("{}");
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);
            var request = new UpdateDocumentGroupTemplateRecipientsRequest
            {
                InviteSteps = new List<DocumentGroupTemplateRecipientStep>
                {
                    new DocumentGroupTemplateRecipientStep
                    {
                        Order = 1,
                        Recipients = new[]
                        {
                            new DocumentGroupTemplateRecipientRole
                            {
                                Id = "roleId123",
                                Name = "Signer 1",
                                SigningOrder = 1,
                                Email = "new.signer@example.com"
                            }
                        }
                    }
                }
            };

            await service.UpdateDocumentGroupTemplateRecipientsAsync(TemplateGroupId, request, CancellationToken.None);
        }
    }
}
