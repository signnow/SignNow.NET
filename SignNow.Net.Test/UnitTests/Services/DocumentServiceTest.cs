using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;
using SignNow.Net.Test.FakeModels.EditFields;
using UnitTests;

namespace UnitTests.Services
{
    [TestClass]
    public class DocumentServiceTest : SignNowTestBase
    {
        [TestMethod]
        public async Task PrefillTextFieldAsyncTest()
        {
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock("", HttpStatusCode.NoContent));

            await service.PrefillTextFieldsAsync(Faker.Random.Hash(40), new TextFieldFaker().Generate(1))
                .ConfigureAwait(false);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task EditDocumentAsyncTest()
        {
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock("{\"id\":\"add9e5af17ad0876ed1ec327cc86209d0377181d\"}"));

            var fields = new List<IFieldEditable>() { new TextFieldFaker().Generate() };
            var response = await service
                .EditDocumentAsync(Faker.Random.Hash(40), fields )
                .ConfigureAwait(false);

            Assert.AreEqual("add9e5af17ad0876ed1ec327cc86209d0377181d", response.Id);
        }

        [TestMethod]
        public async Task MoveDocumentAsyncTest()
        {
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock("{\"result\":\"success\"}"));

            await service
                .MoveDocumentAsync(Faker.Random.Hash(40), Faker.Random.Hash(40))
                .ConfigureAwait(false);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task GetRoutingDetailAsyncTest()
        {
            // Use realistic test data that represents actual API response structure
            var jsonResponse = @"{
                ""routing_details"": [
                    {
                        ""default_email"": ""signer1@example.com"",
                        ""inviter_role"": false,
                        ""name"": ""John Doe"",
                        ""role_id"": ""abc123def456ghi789jkl012mno345pqr678stu901vwx234yz"",
                        ""signing_order"": 1
                    }
                ],
                ""cc"": [""cc1@example.com"", ""cc2@example.com""],
                ""cc_step"": [
                    {
                        ""email"": ""cc1@example.com"",
                        ""step"": 1,
                        ""name"": ""CC Recipient 1""
                    }
                ],
                ""invite_link_instructions"": ""Please review and sign this document"",
                ""viewers"": [
                    {
                        ""default_email"": ""viewer1@example.com"",
                        ""name"": ""Viewer 1"",
                        ""signing_order"": 1,
                        ""inviter_role"": false,
                        ""contact_id"": ""def456ghi789jkl012mno345pqr678stu901vwx234yzabc123""
                    }
                ],
                ""approvers"": [
                    {
                        ""default_email"": ""approver1@example.com"",
                        ""name"": ""Approver 1"",
                        ""signing_order"": 1,
                        ""inviter_role"": false,
                        ""expiration_days"": 15,
                        ""authentication"": {
                            ""type"": ""password""
                        }
                    }
                ],
                ""attributes"": {
                    ""brand_id"": ""ghi789jkl012mno345pqr678stu901vwx234yzabc123def456"",
                    ""redirect_uri"": ""https://signnow.com"",
                    ""on_complete"": ""none""
                }
            }";
            
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock(jsonResponse));

            var response = await service
                .GetRoutingDetailAsync(Faker.Random.Hash(40))
                .ConfigureAwait(false);

            // Test actual deserialization and business logic
            Assert.IsNotNull(response);
            
            // Validate routing details structure and content
            Assert.AreEqual(1, response.RoutingDetails.Count);
            var routingDetail = response.RoutingDetails[0];
            Assert.AreEqual("signer1@example.com", routingDetail.DefaultEmail);
            Assert.AreEqual(false, routingDetail.InviterRole);
            Assert.AreEqual("John Doe", routingDetail.Name);
            Assert.AreEqual("abc123def456ghi789jkl012mno345pqr678stu901vwx234yz", routingDetail.RoleId);
            Assert.AreEqual(1, routingDetail.SigningOrder);
            
            // Validate CC list
            Assert.AreEqual(2, response.Cc.Count);
            Assert.AreEqual("cc1@example.com", response.Cc[0]);
            Assert.AreEqual("cc2@example.com", response.Cc[1]);
            
            // Validate CC step structure
            Assert.AreEqual(1, response.CcStep.Count);
            var ccStep = response.CcStep[0];
            Assert.AreEqual("cc1@example.com", ccStep.Email);
            Assert.AreEqual(1, ccStep.Step);
            Assert.AreEqual("CC Recipient 1", ccStep.Name);
            
            // Validate invite link instructions
            Assert.AreEqual("Please review and sign this document", response.InviteLinkInstructions);
            
            // Validate viewers structure
            Assert.AreEqual(1, response.Viewers.Count);
            var viewer = response.Viewers[0];
            Assert.AreEqual("viewer1@example.com", viewer.DefaultEmail);
            Assert.AreEqual("Viewer 1", viewer.Name);
            Assert.AreEqual(1, viewer.SigningOrder);
            Assert.AreEqual(false, viewer.InviterRole);
            Assert.AreEqual("def456ghi789jkl012mno345pqr678stu901vwx234yzabc123", viewer.ContactId);
            
            // Validate approvers structure and authentication
            Assert.AreEqual(1, response.Approvers.Count);
            var approver = response.Approvers[0];
            Assert.AreEqual("approver1@example.com", approver.DefaultEmail);
            Assert.AreEqual("Approver 1", approver.Name);
            Assert.AreEqual(1, approver.SigningOrder);
            Assert.AreEqual(false, approver.InviterRole);
            Assert.AreEqual(15, approver.ExpirationDays);
            Assert.IsNotNull(approver.Authentication);
            Assert.AreEqual(AuthenticationInfoType.Password, approver.Authentication.Type);
            
            // Validate attributes structure
            Assert.IsNotNull(response.Attributes);
            Assert.AreEqual("ghi789jkl012mno345pqr678stu901vwx234yzabc123def456", response.Attributes.BrandId);
            Assert.AreEqual("https://signnow.com/", response.Attributes.RedirectUri.ToString());
            Assert.AreEqual("none", response.Attributes.OnComplete);
        }

        [TestMethod]
        public async Task GetRoutingDetailAsyncThrowsExceptionForInvalidDocumentId()
        {
            var service = new DocumentService(ApiBaseUrl, new Token());

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .GetRoutingDetailAsync("invalidId")
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            var errorMessage = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, "invalidId");
            StringAssert.Contains(exception.Message, errorMessage);
            Assert.AreEqual("invalidId", exception.ParamName);
        }

        [TestMethod]
        public async Task CreateRoutingDetailAsyncTest()
        {
            // Use realistic test data that represents actual API response structure
            var jsonResponse = @"{
                ""routing_details"": [
                    {
                        ""default_email"": ""signer1@example.com"",
                        ""inviter_role"": false,
                        ""name"": ""John Doe"",
                        ""role_id"": ""abc123def456ghi789jkl012mno345pqr678stu901vwx234yz"",
                        ""signing_order"": 1
                    }
                ],
                ""cc"": [""cc1@example.com""],
                ""cc_step"": [
                    {
                        ""email"": ""cc1@example.com"",
                        ""step"": 1,
                        ""name"": ""CC Recipient 1""
                    }
                ],
                ""invite_link_instructions"": ""Please review and sign this document""
            }";
            
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock(jsonResponse));

            var response = await service
                .CreateRoutingDetailAsync(Faker.Random.Hash(40))
                .ConfigureAwait(false);

            // Test actual deserialization and business logic
            Assert.IsNotNull(response);
            
            // Validate routing details structure and content
            Assert.AreEqual(1, response.RoutingDetails.Count);
            var routingDetail = response.RoutingDetails[0];
            Assert.AreEqual("signer1@example.com", routingDetail.DefaultEmail);
            Assert.AreEqual(false, routingDetail.InviterRole);
            Assert.AreEqual("John Doe", routingDetail.Name);
            Assert.AreEqual("abc123def456ghi789jkl012mno345pqr678stu901vwx234yz", routingDetail.RoleId);
            Assert.AreEqual(1, routingDetail.SignerOrder);
            
            // Validate CC list
            Assert.AreEqual(1, response.Cc.Count);
            Assert.AreEqual("cc1@example.com", response.Cc[0]);
            
            // Validate CC step structure
            Assert.AreEqual(1, response.CcStep.Count);
            var ccStep = response.CcStep[0];
            Assert.AreEqual("cc1@example.com", ccStep.Email);
            Assert.AreEqual(1, ccStep.Step);
            Assert.AreEqual("CC Recipient 1", ccStep.Name);
            
            // Validate invite link instructions
            Assert.AreEqual("Please review and sign this document", response.InviteLinkInstructions);
        }

        [TestMethod]
        public async Task CreateRoutingDetailAsyncThrowsExceptionForInvalidDocumentId()
        {
            var service = new DocumentService(ApiBaseUrl, new Token());

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .CreateRoutingDetailAsync("invalidId")
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            var errorMessage = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, "invalidId");
            StringAssert.Contains(exception.Message, errorMessage);
            Assert.AreEqual("invalidId", exception.ParamName);
        }

        [TestMethod]
        public async Task UpdateRoutingDetailAsyncTest()
        {
            // Use realistic test data for request
            var request = new UpdateRoutingDetailRequest
            {
                Id = "template123",
                DocumentId = "doc123",
                Data = new List<RoutingDetailData>
                {
                    new RoutingDetailData
                    {
                        DefaultEmail = "signer1@example.com",
                        InviterRole = false,
                        Name = "John Doe",
                        RoleId = "abc123def456ghi789jkl012mno345pqr678stu901vwx234yz",
                        SignerOrder = 1
                    }
                },
                Cc = new List<string> { "cc1@example.com" },
                CcStep = new List<SignNow.Net.Model.Requests.UpdateRoutingDetailCcStep>
                {
                    new SignNow.Net.Model.Requests.UpdateRoutingDetailCcStep
                    {
                        Email = "cc1@example.com",
                        Step = 1,
                        Name = "CC Recipient 1"
                    }
                },
                InviteLinkInstructions = "Please review and sign this document"
            };

            // Use realistic test data for response
            var jsonResponse = @"{
                ""template_data"": [
                    {
                        ""default_email"": ""signer1@example.com"",
                        ""inviter_role"": false,
                        ""name"": ""John Doe"",
                        ""role_id"": ""abc123def456ghi789jkl012mno345pqr678stu901vwx234yz"",
                        ""signer_order"": 1,
                        ""decline_by_signature"": false
                    }
                ],
                ""cc"": [""cc1@example.com""],
                ""cc_step"": [
                    {
                        ""email"": ""cc1@example.com"",
                        ""step"": 1,
                        ""name"": ""CC Recipient 1""
                    }
                ],
                ""invite_link_instructions"": ""Please review and sign this document"",
                ""viewers"": [
                    {
                        ""default_email"": ""viewer1@example.com"",
                        ""name"": ""Viewer 1"",
                        ""signing_order"": 1,
                        ""inviter_role"": false,
                        ""contact_id"": ""def456ghi789jkl012mno345pqr678stu901vwx234yzabc123""
                    }
                ],
                ""approvers"": [
                    {
                        ""default_email"": ""approver1@example.com"",
                        ""name"": ""Approver 1"",
                        ""signing_order"": 1,
                        ""inviter_role"": false,
                        ""contact_id"": ""approver123""
                    }
                ],
                ""attributes"": {
                    ""brand_id"": ""ghi789jkl012mno345pqr678stu901vwx234yzabc123def456"",
                    ""redirect_uri"": ""https://signnow.com"",
                    ""close_redirect_uri"": ""https://signnow.com/close""
                }
            }";
            
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock(jsonResponse));

            var response = await service
                .UpdateRoutingDetailAsync(Faker.Random.Hash(40), request)
                .ConfigureAwait(false);

            // Test actual deserialization and business logic
            Assert.IsNotNull(response);
            
            // Validate template data structure
            Assert.AreEqual(1, response.TemplateData.Count);
            var templateData = response.TemplateData[0];
            Assert.AreEqual("signer1@example.com", templateData.DefaultEmail);
            Assert.AreEqual(false, templateData.InviterRole);
            Assert.AreEqual("John Doe", templateData.Name);
            Assert.AreEqual("abc123def456ghi789jkl012mno345pqr678stu901vwx234yz", templateData.RoleId);
            Assert.AreEqual(1, templateData.SignerOrder);
            Assert.AreEqual(false, templateData.DeclineBySignature);
            
            // Validate CC list
            Assert.AreEqual(1, response.Cc.Count);
            Assert.AreEqual("cc1@example.com", response.Cc[0]);
            
            // Validate CC step structure
            Assert.AreEqual(1, response.CcStep.Count);
            var ccStep = response.CcStep[0];
            Assert.AreEqual("cc1@example.com", ccStep.Email);
            Assert.AreEqual(1, ccStep.Step);
            Assert.AreEqual("CC Recipient 1", ccStep.Name);
            
            // Validate invite link instructions
            Assert.AreEqual("Please review and sign this document", response.InviteLinkInstructions);
            
            // Validate viewers structure
            Assert.AreEqual(1, response.Viewers.Count);
            var viewer = response.Viewers[0];
            Assert.AreEqual("viewer1@example.com", viewer.DefaultEmail);
            Assert.AreEqual("Viewer 1", viewer.Name);
            Assert.AreEqual(1, viewer.SigningOrder);
            Assert.AreEqual(false, viewer.InviterRole);
            Assert.AreEqual("def456ghi789jkl012mno345pqr678stu901vwx234yzabc123", viewer.ContactId);
            
            // Validate approvers structure
            Assert.AreEqual(1, response.Approvers.Count);
            var approver = response.Approvers[0];
            Assert.AreEqual("approver1@example.com", approver.DefaultEmail);
            Assert.AreEqual("Approver 1", approver.Name);
            Assert.AreEqual(1, approver.SigningOrder);
            Assert.AreEqual(false, approver.InviterRole);
            Assert.AreEqual("approver123", approver.ContactId);
            
            // Validate attributes structure
            Assert.IsNotNull(response.Attributes);
            Assert.AreEqual("ghi789jkl012mno345pqr678stu901vwx234yzabc123def456", response.Attributes.BrandId);
            Assert.AreEqual("https://signnow.com/", response.Attributes.RedirectUri.ToString());
            Assert.AreEqual("https://signnow.com/close", response.Attributes.CloseRedirectUri.ToString());
        }

        [TestMethod]
        public async Task UpdateRoutingDetailAsyncThrowsExceptionForInvalidDocumentId()
        {
            var service = new DocumentService(ApiBaseUrl, new Token());
            var request = new UpdateRoutingDetailRequestFaker().Generate();

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .UpdateRoutingDetailAsync("invalidId", request)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            var errorMessage = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, "invalidId");
            StringAssert.Contains(exception.Message, errorMessage);
            Assert.AreEqual("invalidId", exception.ParamName);
        }

        [TestMethod]
        public async Task UpdateRoutingDetailAsyncThrowsExceptionForNullRequest()
        {
            var service = new DocumentService(ApiBaseUrl, new Token());

            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () => await service
                    .UpdateRoutingDetailAsync(Faker.Random.Hash(40), null)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            Assert.AreEqual("request", exception.ParamName);
        }

        [TestMethod]
        public async Task ThrowsExceptionForWrongParams()
        {
            var service = new DocumentService(ApiBaseUrl, new Token());

            var documentIdException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .MoveDocumentAsync("documentId", Faker.Random.Hash(40))
                    .ConfigureAwait(false)
                ).ConfigureAwait(false);

            var errorMessage1 = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, "documentId");
            StringAssert.Contains(documentIdException.Message, errorMessage1);
            Assert.AreEqual("documentId", documentIdException.ParamName);


            var folderIdException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .MoveDocumentAsync(Faker.Random.Hash(40), "folderId")
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            var errorMessage2 = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, "folderId");
            StringAssert.Contains(folderIdException.Message, errorMessage2);
            Assert.AreEqual("folderId", folderIdException.ParamName);
        }
    }
}
