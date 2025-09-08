using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Services
{
    [TestClass]
    public class DocumentGroupServiceTest : SignNowTestBase
    {
        [TestMethod]
        public async Task CreateDocumentGroupAsyncTest()
        {
            var service = new DocumentGroupService(ApiBaseUrl, new Token(),
                SignNowClientMock("{\"id\":\"add9e5af17ad0876ed1ec327cc86209d0377181d\"}"));

            var documents = new SignNowDocumentFaker().Generate(5);
            var response = await service.CreateDocumentGroupAsync("test group", documents).ConfigureAwait(false);

            Assert.IsInstanceOfType(response, typeof(DocumentGroupCreateResponse));
            Assert.AreEqual("add9e5af17ad0876ed1ec327cc86209d0377181d", response.Id);
        }

        [TestMethod]
        public async Task ThrowsExceptionForWrongParamsTest()
        {
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), SignNowClientMock("{}"));

            var options = new LimitOffsetOptions
            {
                Limit = 0
            };
            var limitException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .GetDocumentGroupsAsync(options)
                    .ConfigureAwait(false)
                ).ConfigureAwait(false);

            StringAssert.Contains(limitException.Message, "Limit must be greater than 0 but less than or equal to 50.");
            Assert.AreEqual("options", limitException.ParamName);

            options.Limit = 2;
            options.Offset = -1;

            var offsetException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .GetDocumentGroupsAsync(options)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            StringAssert.Contains(offsetException.Message, "Offset must be 0 or greater.");
            Assert.AreEqual("options", offsetException.ParamName);
        }

        [TestMethod]
        public async Task ThrowsExceptionForWrongParamsClassTest()
        {
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), SignNowClientMock("{}"));

            var instanceException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .GetDocumentGroupsAsync(new PagePaginationOptions())
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            StringAssert.Contains(instanceException.Message, "Query params does not have 'limit' and 'offset' options. Use \"LimitOffsetOptions\" class.");
            Assert.AreEqual("options", instanceException.ParamName);
        }

        [TestMethod]
        public async Task GetDocumentGroupInfoAsyncTest()
        {
            var jsonResponse = @"{
                ""data"": {
                    ""id"": ""03c74b3083f34ebf8ef40a3039dfb32c85a08437"",
                    ""name"": ""CreateDocumentGroupTest"",
                    ""created"": 1729535107,
                    ""updated"": 1729535107,
                    ""invite_id"": null,
                    ""pending_step_id"": null,
                    ""state"": ""created"",
                    ""sign_as_merged"": null,
                    ""last_invite_id"": null,
                    ""owner_email"": ""signnow-tests@gmail.com"",
                    ""folder_id"": ""e1d8d63ba51c4009ab8241f279c908a0fd5a5e48"",
                    ""share_info"": {
                        ""is_team_shared"": false,
                        ""role"": ""owner"",
                        ""is_personally_shared_to_others"": false
                    },
                    ""documents"": [
                        {
                            ""roles"": [],
                            ""document_name"": ""ForDocumentGroupFile-1"",
                            ""page_count"": 1,
                            ""id"": ""66974a4b421546a69167ba342d1ae94af56ce351"",
                            ""updated"": 1729535105,
                            ""folder_id"": ""e1d8d63ba51c4009ab8241f279c908a0fd5a5e48"",
                            ""owner"": {
                                ""id"": ""40204b3344984733bb16d61f8550f8b5edfd719a"",
                                ""email"": ""signnow-tests@gmail.com""
                            },
                            ""thumbnail"": {
                                ""small"": ""https://api-eval.signnow.com/document/66974a4b421546a69167ba342d1ae94af56ce351/thumbnail?size=small"",
                                ""medium"": ""https://api-eval.signnow.com/document/66974a4b421546a69167ba342d1ae94af56ce351/thumbnail?size=medium"",
                                ""large"": ""https://api-eval.signnow.com/document/66974a4b421546a69167ba342d1ae94af56ce351/thumbnail?size=large""
                            },
                            ""origin_document_id"": null,
                            ""has_unassigned_field"": false,
                            ""has_credit_card_number"": false,
                            ""field_invites"": [],
                            ""shared_with_team"": null,
                            ""settings"": [],
                            ""allow_to_remove"": true
                        }
                    ],
                    ""owner"": {
                        ""id"": ""40204b3344984733bb16d61f8550f8b5edfd719a"",
                        ""email"": ""signnow.tutorial+dotnet@gmail.com"",
                        ""organization"": {
                            ""id"": ""1ef2dc3eef1c2222a8aea48d238968402abc745f""
                        }
                    },
                    ""cc_emails"": [],
                    ""freeform_invite"": {
                        ""id"": null,
                        ""last_id"": null
                    },
                    ""mail_provider"": null
                }
            }";
            var service = new DocumentGroupService(ApiBaseUrl, new Token(),
                SignNowClientMock(jsonResponse));

            var response = await service.GetDocumentGroupInfoAsync("03c74b3083f34ebf8ef40a3039dfb32c85a08437").ConfigureAwait(false);

            Assert.IsInstanceOfType(response, typeof(DocumentGroupInfoResponse));
            Assert.AreEqual("03c74b3083f34ebf8ef40a3039dfb32c85a08437", response.Data.Id);
            Assert.AreEqual("CreateDocumentGroupTest", response.Data.Name);
            Assert.AreEqual("created", response.Data.State);
            Assert.AreEqual("e1d8d63ba51c4009ab8241f279c908a0fd5a5e48", response.Data.FolderId);
            Assert.AreEqual(1, response.Data.Documents.Count);
            Assert.AreEqual("66974a4b421546a69167ba342d1ae94af56ce351", response.Data.Documents[0].Id);
            Assert.AreEqual("ForDocumentGroupFile-1", response.Data.Documents[0].Name);
            Assert.AreEqual("40204b3344984733bb16d61f8550f8b5edfd719a", response.Data.Owner.Id);
        }

        [TestMethod]
        public async Task UpdateDocumentGroupTemplateAsyncTest()
        {
            var jsonResponse = @"{
                ""status"": ""success""
            }";
            var service = new DocumentGroupService(ApiBaseUrl, new Token(),
                SignNowClientMock(jsonResponse));

            var updateRequest = new UpdateDocumentGroupTemplateRequestFaker().Generate();
            var response = await service.UpdateDocumentGroupTemplateAsync("03c74b3083f34ebf8ef40a3039dfb32c85a08437", updateRequest).ConfigureAwait(false);

            Assert.IsInstanceOfType(response, typeof(UpdateDocumentGroupTemplateResponse));
            Assert.AreEqual("success", response.Status);
        }

        [TestMethod]
        public async Task UpdateDocumentGroupTemplateAsyncThrowsExceptionForInvalidIdTest()
        {
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), SignNowClientMock("{}"));
            var updateRequest = new UpdateDocumentGroupTemplateRequestFaker().Generate();

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .UpdateDocumentGroupTemplateAsync("invalid-id", updateRequest)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            StringAssert.Contains(exception.Message, "Invalid format of ID");
            Assert.AreEqual("invalid-id", exception.ParamName);
        }
    }
}
