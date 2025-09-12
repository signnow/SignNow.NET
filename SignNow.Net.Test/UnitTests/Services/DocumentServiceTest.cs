using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
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
            var fakeResponse = new GetRoutingDetailResponseFaker().Generate();
            var jsonResponse = TestUtils.SerializeToJsonFormatted(fakeResponse);
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock(jsonResponse));

            var response = await service
                .GetRoutingDetailAsync(Faker.Random.Hash(40))
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.RoutingDetails);
            Assert.IsNotNull(response.Cc);
            Assert.IsNotNull(response.CcStep);
            Assert.IsNotNull(response.InviteLinkInstructions);
            Assert.IsNotNull(response.Viewers);
            Assert.IsNotNull(response.Approvers);
            Assert.IsNotNull(response.Attributes);
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
        public async Task PostRoutingDetailAsyncTest()
        {
            var fakeResponse = new PostRoutingDetailResponseFaker().Generate();
            var jsonResponse = TestUtils.SerializeToJsonFormatted(fakeResponse);
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock(jsonResponse));

            var response = await service
                .PostRoutingDetailAsync(Faker.Random.Hash(40))
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.RoutingDetails);
            Assert.IsNotNull(response.Cc);
            Assert.IsNotNull(response.CcStep);
            Assert.IsNotNull(response.InviteLinkInstructions);
        }

        [TestMethod]
        public async Task PostRoutingDetailAsyncThrowsExceptionForInvalidDocumentId()
        {
            var service = new DocumentService(ApiBaseUrl, new Token());

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .PostRoutingDetailAsync("invalidId")
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            var errorMessage = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, "invalidId");
            StringAssert.Contains(exception.Message, errorMessage);
            Assert.AreEqual("invalidId", exception.ParamName);
        }

        [TestMethod]
        public async Task PutRoutingDetailAsyncTest()
        {
            var fakeRequest = new PutRoutingDetailRequestFaker().Generate();
            var fakeResponse = new PutRoutingDetailResponseFaker().Generate();
            var jsonResponse = TestUtils.SerializeToJsonFormatted(fakeResponse);
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock(jsonResponse));

            var response = await service
                .PutRoutingDetailAsync(Faker.Random.Hash(40), fakeRequest)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.TemplateData);
            Assert.IsNotNull(response.Cc);
            Assert.IsNotNull(response.CcStep);
            Assert.IsNotNull(response.InviteLinkInstructions);
            Assert.IsNotNull(response.Viewers);
            Assert.IsNotNull(response.Approvers);
            Assert.IsNotNull(response.Attributes);
        }

        [TestMethod]
        public async Task PutRoutingDetailAsyncThrowsExceptionForInvalidDocumentId()
        {
            var service = new DocumentService(ApiBaseUrl, new Token());
            var request = new PutRoutingDetailRequestFaker().Generate();

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .PutRoutingDetailAsync("invalidId", request)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            var errorMessage = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, "invalidId");
            StringAssert.Contains(exception.Message, errorMessage);
            Assert.AreEqual("invalidId", exception.ParamName);
        }

        [TestMethod]
        public async Task PutRoutingDetailAsyncThrowsExceptionForNullRequest()
        {
            var service = new DocumentService(ApiBaseUrl, new Token());

            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () => await service
                    .PutRoutingDetailAsync(Faker.Random.Hash(40), null)
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
