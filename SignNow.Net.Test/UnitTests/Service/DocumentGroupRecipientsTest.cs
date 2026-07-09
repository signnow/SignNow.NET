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
    public class DocumentGroupRecipientsTest : SignNowTestBase
    {
        private const string DocumentGroupId = "a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";

        [TestMethod]
        public async Task GetDocumentGroupRecipientsAsync_Success()
        {
            var expectedResponse = new DocumentGroupRecipientsResponse
            {
                Data = new DocumentGroupRecipientsData
                {
                    Recipients = new[]
                    {
                        new DocumentGroupRecipient
                        {
                            Name = "Signer 1",
                            Email = "signer@example.com",
                            Order = 1,
                            Documents = new[]
                            {
                                new DocumentGroupRecipientDocument
                                {
                                    Id = "docId123",
                                    Role = "Signer",
                                    Action = "sign"
                                }
                            }
                        }
                    },
                    Cc = new[] { "cc@example.com" },
                    GeneralExpirationDays = 30,
                    GeneralReminder = new DocumentGroupTemplateReminder
                    {
                        RemindBefore = 5,
                        RemindAfter = 1,
                        RemindRepeat = 3
                    },
                    OrderType = DocumentGroupOrderType.RecipientOrder
                }
            };

            var mockClient = SignNowClientMock(JsonConvert.SerializeObject(expectedResponse));
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);

            var result = await service.GetDocumentGroupRecipientsAsync(DocumentGroupId, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(1, result.Data.Recipients.Count);
            Assert.AreEqual("Signer 1", result.Data.Recipients[0].Name);
            Assert.AreEqual(DocumentGroupOrderType.RecipientOrder, result.Data.OrderType);
        }

        [TestMethod]
        public async Task UpdateDocumentGroupRecipientsAsync_Success()
        {
            var mockClient = SignNowClientMock("{}");
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);
            var request = new UpdateDocumentGroupRecipientsRequest
            {
                Recipients = new List<UpdateDocumentGroupRecipientEntry>
                {
                    new UpdateDocumentGroupRecipientEntry
                    {
                        Name = "Signer 1",
                        Email = "new.signer@example.com",
                        Order = 1,
                        Documents = new List<DocumentGroupRecipientDocument>
                        {
                            new DocumentGroupRecipientDocument
                            {
                                Id = "docId123",
                                Role = "Signer",
                                Action = "sign"
                            }
                        }
                    }
                },
                GeneralExpirationDays = 30,
                OrderType = DocumentGroupOrderType.RecipientOrder
            };

            await service.UpdateDocumentGroupRecipientsAsync(DocumentGroupId, request, CancellationToken.None);
        }
    }
}
